using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Framework.Messaging;
using Framework.Pool;
using UnityEngine.EventSystems;

public class MainMenu : Window
{

    public Button continueButton;
    public Camera mainCam;

    public Animation animationMenu;

    void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(PauseMessage), PauseHandler);
        continueButton.gameObject.SetActive(false);
    }

    public bool PauseHandler(BaseMessage msg)
    {        
        PauseMessage pauseMsg = (PauseMessage)msg;

        if (pauseMsg.pause)
        {
            mainCam.GetComponent<AudioListener>().enabled = false;

            GameState oldState = GameManager.Instance.OldState;
            if (oldState != GameState.GAME_OVER && oldState != GameState.WIN)
            {
                continueButton.gameObject.SetActive(true);
                //GameObjectBank.Instance.canvas.SetActive(false);
                //EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
                firstSelected = continueButton.gameObject;
                EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().Play("Highlighted");
                //eventSystem.SetSelectedGameObject(continueButton.gameObject);
                //eventSystem.currentSelectedGameObject.GetComponent<Animator>().Play("Highlited");             
            }

            //GameObjectBank.Instance.eventSystem.SetSelectedGameObject(continueButton.gameObject);
            //eventSystem.SetSelectedGameObject(continueButton.gameObject);           
        }
        else
        {
            continueButton.gameObject.SetActive(false);
            mainCam.GetComponent<AudioListener>().enabled = true;
            //GameObjectBank.Instance.canvas.SetActive(true);
        }

        return true;
    }

    public void startGame()
    {
        Time.timeScale = 1;
        PoolManager.Instance.ResetPool();
        GameManager.Instance.State = GameState.INGAME;
        SceneManager.LoadScene(2);
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

    public void highscores(){
        manager.Open(2);
    }
    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
        {
            MessagingSystem.Instance.DetachListener(typeof(PauseMessage), PauseHandler);
        }
    }
}
