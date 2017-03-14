using UnityEngine;
using UnityEngine.SceneManagement;
using Framework.Messaging;

public class Pause : MonoBehaviour {

    private bool isPaused = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(PauseMessage), PauseHandler);
    }

    public bool PauseHandler(BaseMessage msg){
        PauseMessage pauseMsg = (PauseMessage)msg;

        isPaused = pauseMsg.isPaused;

        return false;
    }
    public void Paused()
    {
        MessagingSystem.Instance.QueueMessage(new PauseMessage(true));       

        Time.timeScale = 0;
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }
    // Update is called once per frame
    void Update () {
        if ((Input.GetKeyDown(KeyCode.Escape) || 
            Input.GetKeyDown(KeyCode.Joystick1Button7)) 
            && !isPaused)
        {
            Paused();
        }
    }
}
