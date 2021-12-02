using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Parser;
using System.Linq;
using UnityEngine.EventSystems;

// This is an example of how to interface with the dialogue parser, you can
// rewrite this script as needed to fit the needs of your game. You should not
// need to directly edit the parser itself, instead just use these functions to
// interface with the parser:

// Make a new parser:
// Parser p = new Parser(TextAsset blob, gameObject listener);

// Set the label you'd like the parser to run:
// p.SetLabel(string labelName);

// Tell the parser a choice was made on a menu screen:
// p.RegisterChoice(int choice);

// Move the paser to the next line -- if the parser is at the end of the script,
// this returns null.
// p.Advance();

public class DialogueController : MonoBehaviour {
    [SerializeField]
    [Tooltip("The .json file that holds your script data.")]
    // To convert a script to .json, use this site:
    // https://fragrant-harbor-90377.herokuapp.com/
    // For an examples of the raw script format, look in "Assets/ExampleScripts/".
    // If you need an editor mode for your raw script files, the script format
    // works with .rpy modes for most formats. If you don't already have an
    // editor of choice, Atom works well and has a nice .rpy mode :)
    private TextAsset JSONFile;

    [SerializeField]
    [Tooltip("Determines the amount of spacing between each choice in a menu.")]
    static float choiceSpacing = 25.0f;

    [SerializeField]
    [Tooltip("If checked, this will keep the last shown dialogue on screen when a menu pops up.")]
    private bool showDialogueOnChoice = false;

    [SerializeField]
    [Tooltip("If checked, letters will show up one at a time.")]
    private bool typewritterEffect = false;

    [SerializeField]
    [Tooltip("If typewritter_effect is checked, this determines the character per second shown.")]
    [Range(1, 500)]
    private int CPS = 30;

    [SerializeField]
    [Tooltip("The inital label name the parse will be set to.")]
    private string initialLabel = "start";

    [SerializeField]
    [Tooltip("If checked, the dialogue parser will run on Start.")]
    private bool runOnStart = false;

    private Parser parser;

    private GameObject dialogueBox;
    private GameObject nameBox;
    private GameObject menuBox;

    private Text dialogueText;
    private Text nameText;

    // Instead of instatiationg on the fly we're adding all choice buttons to the scene
    // from the start - make sure you have enough choice GameObjects for your choices
    // with the most options!
    private List<Choice> choices;
    private float choiceHeight;

    // I should add a CTC.
    private float text_dt = 0f;
    private int text_index = 0;

    private DialogueNode currentNode;
    private DialogueTracker tracker;

    private enum State {
        Inactive,
        DialoguePrinting,
        DialoguePrinted,
        Menu
    }

    private State state = State.Inactive;

    private struct Choice {
        public GameObject obj { get; }
        public Text text { get; }
        public RectTransform rect { get; }

        public Choice(GameObject game_obj) {
            obj = game_obj;
            text = game_obj.transform.GetComponentInChildren<Text>();
            rect = game_obj.GetComponent<RectTransform>();
        }
    }

    void Start() {
        SetUp();
    }

    private void SetUp() {
        tracker = GetComponent<DialogueTracker>();
        parser = new Parser(JSONFile, tracker);
        parser.SetLabel(initialLabel);

        dialogueBox = transform.Find("Canvas/DialogueBox").gameObject;
        nameBox = transform.Find("Canvas/NameBox").gameObject;
        menuBox = transform.Find("Canvas/MenuBox").gameObject;

        dialogueText = dialogueBox.GetComponentInChildren<Text>();
        nameText = nameBox.GetComponentInChildren<Text>();
        choices = new List<Choice>();

        foreach (Transform t in menuBox.transform) {
            choices.Add(new Choice(t.gameObject));
        }

        choiceHeight = choices[0].obj.transform.Find("BG").gameObject.GetComponent<RectTransform>().rect.height;

        ClearAll();
        // If you have runOnStart set to false, then you can call UpdateDialogue()
        // yourself to show the dialogue boxes.
        if (runOnStart) {
            UpdateDialogue();
        }
    }

    private void EndDialgoue() {
        // Called when we finish the current script.
        ClearAll();
        state = State.Inactive;
        // If you had to interface with other game objects when dialgue ends to
        // resume normal gameplay, this is where you'd do that.
    }

    private void UpdateDialogue() {
        Node node = parser.Advance();
        if (node == null) {
            EndDialgoue();
        } else if (node.type == NodeType.Dialogue) {
            SetDialogue(node);
        } else if (node.type == NodeType.Menu) {
            SetMenu(node);
        }
    }

    private void SetDialogue(Node node) {
        DialogueNode dialogue_node = node as DialogueNode;
        dialogueBox.SetActive(true);
        
        if (dialogue_node.caption == "") {
            nameBox.SetActive(false);
        } else {
            nameBox.SetActive(true);
            nameText.text = dialogue_node.caption;
        }

        if (typewritterEffect) {
            PrintDialogeuStart(node as DialogueNode);
        } else {
            PrintDialogeuEnd(node as DialogueNode);
        }
    }

    private void PrintDialogeuStart(DialogueNode node) {
        state = State.DialoguePrinting;
        text_dt = 0f;
        text_index = 1;
        currentNode = node;
        dialogueText.text = currentNode.text.Substring(0, text_index);
    }

    private void PrintDialogeuEnd(DialogueNode node) {
        state = State.DialoguePrinted;
        dialogueText.text = node.text;

    }

    private void SetMenu(Node node) {
        if (!showDialogueOnChoice) {
            dialogueBox.SetActive(false);
            nameBox.SetActive(false);
        }

        state = State.Menu;
        MenuNode menu_node = node as MenuNode;
        float y = (choiceHeight * menu_node.choices.Count + (menu_node.choices.Count - 1) * choiceSpacing) / 2;

        for (int i=0; i < menu_node.choices.Count; i++) {
            choices[i].obj.SetActive(true);
            choices[i].text.text = menu_node.choices[i].choice;
            choices[i].rect.anchoredPosition = new Vector2(0, y);
            y -= choiceHeight + choiceSpacing;
        }
    }

    private void Update() {
        // If you want different user inputs to forward the script, add them here.
        bool input = new bool[] { Input.GetMouseButtonUp(0),
                                  Input.GetKeyDown("space"),
                                  Input.GetKeyDown("return") }.Contains(true);

        if (state == State.DialoguePrinting) {
            if (input) {
                PrintDialogeuEnd(currentNode);
            } else {
                text_index = Mathf.Min((int)Mathf.Floor(text_dt/(1f/CPS)),
                                                        currentNode.text.Length);
                dialogueText.text = currentNode.text.Substring(0, text_index);
                text_dt += Time.deltaTime;
                if (text_index == currentNode.text.Length) {
                    PrintDialogeuEnd(currentNode);
                }
            }
        } else if (state == State.DialoguePrinted && input) {
            UpdateDialogue();
        }
    }

    public void ChoiceCallback(int selection) {
        // This should be attached to the Choice game objects, via their button
        // components. They should all return an int corresponding with their
        // order, starting with 0.
        EventSystem.current.SetSelectedGameObject(null);
        ClearAll();
        parser.RegisterChoice(selection);
        UpdateDialogue();
    }

    private void ClearAll() {
        // Hide ALL dialgue pop ups.
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);
        ClearMenu();
    }

    private void ClearMenu() {
        // Hides just the menu and associated pop ups.
        foreach (Choice c in choices) {
            c.obj.SetActive(false);
        }
    }
}
