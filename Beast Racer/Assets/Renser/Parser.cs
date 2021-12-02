using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class Parser {

    private Dictionary<string, List<Node>> script;
    //private string label = "";
    private GameObject reciever;
    public bool canAdvance = false;
    private List<StateFrame> state = new List<StateFrame>();
    private DialogueTracker listener;

    #region Data types.
    public static class Action {
        public const String Jump = "jump";
        public const String Call = "call";
        public const String Return = "return";
        public const String Signal = "signal";
    }

    public enum NodeType {
        Label,
        Dialogue,
        Menu,
        Choice,
        Action
    }

    public class Node {
        public NodeType type;

        public override string ToString() => string.Format("Node <{0}>", type);
    }

    public class DialogueNode : Node {
        public string caption;
        public string text;

        public DialogueNode(string cap, string txt) {
            caption = cap;
            text = txt;
            type = NodeType.Dialogue;
        }
    }

    public class ActionNode : Node {
        public string action;
        public string label;
        public string message;

        public ActionNode(string act, string lbl, string msg){
            action = act;
            label = lbl;
            message = msg;
            type = NodeType.Action;
        }
    }

    public class MenuNode : Node {
        public List<ChoiceNode> choices;

        public MenuNode(List<ChoiceNode> choi){
            choices = choi;
            type = NodeType.Menu;
        }
    }

    public class ChoiceNode : Node {
        public string choice;
        public List<Node> result;

        public ChoiceNode(string choi, List<Node> res) {
            choice = choi;
            result = res;
            type = NodeType.Choice;
        }
    }

    private class StateFrame {
        public string label;
        //public Node node;
        public List<int> state = new List<int>();

        public StateFrame(string label_name) {
            label = label_name;
            //node = currentNode;
            //state = new List<int>();
            state.Add(-1);
        }
    }
    #endregion

    public Parser(TextAsset blob, DialogueTracker tracker) {
        ParseBlob(blob);
        listener = tracker;
    }

    #region Interaface.
    public void SetLabel(string label_name) {
        //label = label_name;
        state = new List<StateFrame>();
        state.Add(new StateFrame(label_name));
        // TODO Reste state.
    }

    public Node Advance() {
        return GetNextLine();
    }

    public void RegisterChoice(int selection) {
        state[state.Count - 1].state.Add(selection);
        state[state.Count - 1].state.Add(-1);
    }

    private Node GetLine() {
        StateFrame frame = state[state.Count - 1];
        Node node = script[frame.label][frame.state[0]];

        for (int i = 1; i < frame.state.Count; i++) {
            if (node.type == NodeType.Menu) {
                MenuNode menu = node as MenuNode;
                node = menu.choices[frame.state[i]];
            } else if (node.type == NodeType.Choice) {
                ChoiceNode choice = node as ChoiceNode;
                if (choice.result.Count <= frame.state[i]) {
                    // Implicit return out of a choices.
                    state[state.Count - 1].state.RemoveAt(state[state.Count - 1].state.Count - 1);
                    state[state.Count - 1].state.RemoveAt(state[state.Count - 1].state.Count - 1);
                    return GetNextLine();
                } else {
                    node = choice.result[frame.state[i]];
                }
            } else {
                Debug.Log("Panic.");
            }
        }

        return node;
    }
    #endregion

    #region Dialogue tree traversal.
    private Node TraverseAction(Node node) {
        ActionNode act = node as ActionNode;
        switch (act.action) {
            case Action.Jump:
                SetLabel(act.label);
                break;
            case Action.Call:
                state.Add(new StateFrame(act.label));
                break;
            case Action.Return:
                // Return the whole label.
                state.RemoveAt(state.Count - 1);
                break;
            case Action.Signal:
                // I should check this later
                if (listener != null) { listener.SendMessage("DialogueSignal", act.message); }
                break;
        }
        return GetNextLine();
    }

    private Node GetNextLine() {
        // Check for full script return.
        if (state.Count.Equals(0)) {
            return null;
        }

        StateFrame frame = state[state.Count-1];

        // Actually advance the script.
        frame.state[frame.state.Count - 1] += 1;
        if (script[frame.label].Count <= frame.state[0]) {
            // Implicit return out aof a whole label.
            state.RemoveAt(state.Count - 1);
            return GetNextLine();
        }
        Node node = script[frame.label][frame.state[0]];
        for (int i = 1; i < frame.state.Count; i++) {
            if (node.type == NodeType.Menu) {
                MenuNode menu = node as MenuNode;
                node = menu.choices[frame.state[i]];
            } else if (node.type == NodeType.Choice) {
                ChoiceNode choice = node as ChoiceNode;
                if (choice.result.Count <= frame.state[i]) {
                    // Implicit return out of a choices.
                    state[state.Count - 1].state.RemoveAt(state[state.Count - 1].state.Count - 1);
                    state[state.Count - 1].state.RemoveAt(state[state.Count - 1].state.Count - 1);
                    return GetNextLine();
                } else {
                    node = choice.result[frame.state[i]];
                }
            } else {
                Debug.Log("Panic.");
            }
        }

        if (node.type == NodeType.Action) { node = TraverseAction(node); }

        return node;
    }
    #endregion

    #region Parsing utilities.
    private void ParseBlob(TextAsset blob) {
        script = new Dictionary<string, List<Node>>();

        JObject obj = JObject.Parse(blob.ToString());
        foreach (KeyValuePair<string, JToken> label_data in obj) {
            ParseLabel(label_data);
        }
    }


    private void ParseLabel(KeyValuePair<string, JToken> label_data) {
        // Debug.Log(label_data.Key);
        script.Add(label_data.Key, ParseContent(label_data.Value as JArray));
    }

    private List<Node> ParseContent(JArray content) {
        List<Node> nodes = new List<Node>();
        foreach (JToken item in content.Children()) {
            nodes.Add(ParseNode(item));
        }
        return nodes;
    }

    private MenuNode ParseMenu(JToken token) {
        MenuNode node = new MenuNode(new List<ChoiceNode>());

        foreach (JToken item in (token as JArray).Children()) {
            node.choices.Add(new ChoiceNode(item["choice"].Value<string>(),
                                            ParseContent(item["result"].Value<JArray>())));
        }
        return node;
    }
    private Node ParseNode(JToken item) {
        Node node = null;
        if (item.Type == JTokenType.Array) {
            // Debug.Log("We have menu.");
            node = ParseMenu(item);
        } else if (item["caption"] != null) {
            // Debug.Log("We have dialogue.");
            node = new DialogueNode(item["caption"].Value<string>(),
                                    item["text"].Value<string>());
        } else if (item["action"] != null) {
                node = new ActionNode(item["action"].Value<string>(),
                                      item["label"] != null ? item["label"].Value<string>() : null,
                                      item["message"] != null ? item["message"].Value<string>() : null);
            // Debug.Log("We have an action.");
        } else {
            Debug.Log("We have a mystery.");
        }
        return node;
    }
    #endregion
}
