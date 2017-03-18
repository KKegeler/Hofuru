using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using Framework;
using Framework.Messaging;
using Framework.Pool;
using Framework.Log;
using UnityEngine.UI;

/// <summary>
/// Manages the GameState
/// </summary>
public class GameManager : SingletonAsComponent<GameManager>
{
    #region Variables
    private static GameState _gameState = GameState.DEFAULT;
    private static GameState _oldState = GameState.DEFAULT;
    private static LevelState _levelState = LevelState.DEFAULT;
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

    public LevelState LState
    {
        get { return _levelState; }
        set { EvaluateLevel(value); }
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
                GameObjectBank.Instance.uicam.gameObject.SetActive(false);
                break;

            case GameState.INGAME:
                if (_gameState != GameState.DEFAULT && _oldState != GameState.GAME_OVER)
                {
                    yield return SceneManager.UnloadSceneAsync("MainMenu");
                    GameObjectBank.Instance.mainCamera.gameObject.SetActive(true);
                    GameObjectBank.Instance.uicam.gameObject.SetActive(true);
                }

                Time.timeScale = 1;
                break;

            case GameState.GAME_OVER:
                GreyscaleEffect.Instance.ActivateEffect();
                GameObjectBank.Instance.gameOver.SetActive(true);
                EventSystem.current.SetSelectedGameObject(
                    GameObjectBank.Instance.retry.gameObject);
                break;

            case GameState.WIN:
                EventSystem.current.SetSelectedGameObject(
                    GameObjectBank.Instance.nextLevel.gameObject);
                break;

            default:
                CustomLogger.LogErrorFormat("Unknown Gamestate: {0}\n", _gameState);
                break;
        }

        _gameState = newState;
    }

    private void EvaluateLevel(LevelState newState)
    {
        PoolManager.Instance.ResetPool();

        switch (newState)
        {
            case LevelState.LEVEL_1:
                SceneManager.LoadScene("Level_1");
                break;

            case LevelState.LEVEL_2:
                SceneManager.LoadScene("Level_2");
                break;

            case LevelState.LEVEL_3:
                SceneManager.LoadScene("Level_3");          
                break;

            case LevelState.END:
                GameObjectBank.Instance.nextLevel.GetComponentInChildren<Text>().text =
                    "Back to Menu";
                GameObjectBank.Instance.nextLevel.onClick.AddListener(delegate ()
                { SceneManager.LoadScene("MainMenu"); });

                Debug.Log("You reached the end!\n");
                break;

            default:
                CustomLogger.LogErrorFormat("Unknown LevelState: {0}!\n", newState);
                break;
        }

        _levelState = newState;
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

/// <summary>
/// The level the player is in
/// </summary>
public enum LevelState
{
    DEFAULT = -1,
    LEVEL_1,
    LEVEL_2,
    LEVEL_3,
    END
}