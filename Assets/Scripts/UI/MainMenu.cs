using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Framework.Messaging;
using Framework.Pool;

public class MainMenu : Window
{

    public Button continueButton;
    public Camera mainCam;

    public Animation animationMenu;

    void Start()
    {
        GameManager.Instance.WakeUp();
        MessagingSystem.Instance.AttachListener(typeof(PauseMessage), PauseHandler);
        continueButton.gameObject.SetActive(false);
    }

    public bool PauseHandler(BaseMessage msg)
    {
        PauseMessage pauseMsg = (PauseMessage)msg;
        if (pauseMsg.pause)
        {
            GameState oldState = GameManager.Instance.OldState;
            if (oldState != GameState.GAME_OVER && oldState != GameState.WIN)
                continueButton.gameObject.SetActive(true);

            mainCam.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            continueButton.gameObject.SetActive(false);
            mainCam.GetComponent<AudioListener>().enabled = true;
        }

        return true;
    }

    public void startGame()
    {
        Time.timeScale = 1;
        PoolManager.Instance.ResetPool();
        GameManager.Instance.State = GameState.INGAME;
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	    Application.Quit();
#endif
    }

    public void options()
    {

        manager.Open(1);
    }

    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
        {
            MessagingSystem.Instance.DetachListener(typeof(PauseMessage), PauseHandler);
        }
    }
}
