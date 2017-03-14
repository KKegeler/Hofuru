using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Framework.Messaging;
using Framework.Pool;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MainMenu : Window {

    public Button continueButton;
    public Camera mainCam;
   
    public Button[] buttons;

    public GameObject playButton;

    public Button optionsButton;
    
    public Canvas menu;

    public bool onSelectBool;


    void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(PauseMessage), PauseHandler);
        continueButton.gameObject.SetActive(false);
        /*for(int i = 0;i<buttons.Length;i++){
            buttons[i].animator.Stop();
        }
        buttons[1].animator.Play("Highlighted");*/
        
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

    public void pressed()
    {
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }

     private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
        {
            MessagingSystem.Instance.DetachListener(typeof(PauseMessage), PauseHandler);
        }
    }
}
