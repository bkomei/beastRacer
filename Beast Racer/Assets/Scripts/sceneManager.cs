using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scriptable Objects/Scene")]

public class sceneManager : ScriptableObject
{
    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    // Start is called before the first frame update
   
    void Awake()
    {
        
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Intro:
                HandleIntro();
                break;
            case GameState.Tutorial:
                break;
            case GameState.TutorialGame:
                break;
            case GameState.Prologue:
                break;
            case GameState.Qualifiers:
                break;
            case GameState.QualifiersWin:
                break;
            case GameState.QualifiersLose:
                break;
            case GameState.Chapter1:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleIntro()
    {
        throw new NotImplementedException();
    }

    public enum GameState
    {
        Intro,
        Tutorial,
        TutorialGame,
        Prologue,
        Qualifiers,
        QualifiersWin,
        QualifiersLose,
        Chapter1

    }

    void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void OnDestroy()
    {
        
    }
}
