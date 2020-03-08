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

public enum CursorTypes
{
    Normal,
    Interact
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState GameState;

    public bool DEV = true;

    public Player Player;

    [SerializeField]
    private Texture2D _normalCursor;
    [SerializeField]
    private Texture2D _interactCursor;
    [HideInInspector]
    public GameObject FollowingFalling;

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
        ChangeCursor(CursorTypes.Normal);
        // LevelManager.Instance.Init();
    }

    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        if (FollowingFalling != null) Destroy(FollowingFalling);
    }

    public void Play()
    {
        ChangeGameState(GameState.Game);
        LevelManager.Instance.Init();
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

    // Change cursor
    public void ChangeCursor(CursorTypes type)
    {
        //Texture2D newTexture = _normalCursor;
        //switch (type)
        //{
        //    case CursorTypes.Normal:
        //        newTexture = _normalCursor;
        //        break;
        //    case CursorTypes.Interact:
        //        newTexture = _interactCursor;
        //        break;
        //}
        //Cursor.SetCursor(newTexture, new Vector2(0, 0), CursorMode.Auto);
    }
}
