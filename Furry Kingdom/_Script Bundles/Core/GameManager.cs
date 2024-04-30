using Furry;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnUI;
    public static event Action OnPlay;
    public static event Action OnCompleteLvl;
    public static event Action OnPause;

    public static GameManager Instance;
    public static bool IsPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public enum GameStates
    {
        UI,
        Play,
        CompleteLvl,
        Paused
    }
    public static GameStates GameState { get; private set; }

    private void Start()
    {
        ProceduralLevelGenerator.OnLevelGenerated += Init;
    }

    /// <summary>
    /// Pauses the game, sets the game state to "Pause" and sends out a notification to that effect.
    /// </summary>
    private void UI()
    {
        GameState = GameStates.Paused;
        OnPause?.Invoke();
        //   PauseGameTime();
    }
    private void Play()
    {

    }
    private void CompleteLvl()
    {

    }
    private void Paused()
    {
        GameState = GameStates.Paused;
        OnPause?.Invoke();
        //   PauseGameTime();
    }
    private void Init()
    {
        Play();
    }
}
