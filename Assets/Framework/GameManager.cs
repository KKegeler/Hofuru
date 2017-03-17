using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Framework;
using Framework.Messaging;
using Framework.Log;


/// <summary>
/// Manages the GameState
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
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
                Time.timeScale = 0;
                MessagingSystem.Instance.QueueMessage(new PauseMessage(true));
                GameObjectBank.Instance.mainCamera.gameObject.SetActive(false);
                break;

            case GameState.INGAME:
                if (_gameState != GameState.DEFAULT)
                {
                    yield return SceneManager.UnloadSceneAsync("MainMenu");
                    GameObjectBank.Instance.mainCamera.gameObject.SetActive(true);
                }

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

    new public void WakeUp()
    {
        DataSerializer.Load();
    }

}

/// <summary>
/// The state the game is in
/// </summary>
public enum GameState
{
    DEFAULT = -1,
    PAUSE,
    INGAME,
    GAME_OVER,
    WIN
}