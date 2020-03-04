using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Game,
    Pause,
    GameOver,
    Win
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState GameState;

    public bool DEV = true;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void Play()
    {
        ChangeGameState(GameState.Game);
    }

    public void Pause()
    {
        ChangeGameState(GameState.Pause);
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void Win()
    {
        ChangeGameState(GameState.Win);
    }

    // Change current state of the game
    public void ChangeGameState(GameState newState)
    {
        GameState = newState;
    }
}
