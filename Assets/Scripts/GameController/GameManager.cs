﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Framework;
using Framework.Messaging;
using Framework.Log;

/// <summary>
/// Everything
/// </summary>
public class GameManager : SingletonAsComponent<GameManager>
{
    #region Variables
    private static GameState _gameState = GameState.DEFAULT;
    private static GameState _oldState = GameState.DEFAULT;
    #endregion

    #region Properties
    public static GameManager Instance
    {
        get { return (GameManager)_Instance; }
    }

    public GameState State
    {
        get { return _gameState; }
        set
        {
            if (_gameState != value)
                StartCoroutine(OnGameStateChange(value));
        }
    }

    public GameState OldState
    {
        get { return _oldState; }
    }
    #endregion

    private IEnumerator OnGameStateChange(GameState newState)
    {
        _oldState = _gameState;

        switch (newState)
        {
            case GameState.PAUSE:
                MessagingSystem.Instance.QueueMessage(new PauseMessage(true));

                Time.timeScale = 0;
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
                break;

            case GameState.INGAME:
                if (_gameState != GameState.DEFAULT)
                    yield return SceneManager.UnloadSceneAsync("MainMenu");

                Time.timeScale = 1;
                break;

            case GameState.GAME_OVER:
                GreyscaleEffect.Instance.ActivateEffect();
                DataSerializer.Save();
                break;

            case GameState.WIN:
                DataSerializer.Save();
                break;

            default:
                CustomLogger.LogFormat("Unknown Gamestate: {0}\n", _gameState);
                break;
        }

        _gameState = newState;
    }

}

public enum GameState
{
    DEFAULT = -1,
    PAUSE,
    INGAME,
    GAME_OVER,
    WIN
}