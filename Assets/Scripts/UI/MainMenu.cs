using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Framework.Messaging;
using Framework.Pool;

public class MainMenu : Window {

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
        if(pauseMsg.isPaused == true)
        {
            continueButton.gameObject.SetActive(true);
            mainCam.GetComponent<AudioListener>().enabled = false;
        }else
        {
            continueButton.gameObject.SetActive(false);
            mainCam.GetComponent<AudioListener>().enabled = true;
        }
        return false;
    }

	public void startGame()
    {
        Time.timeScale = 1;
        PoolManager.Instance.ResetPool();
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
