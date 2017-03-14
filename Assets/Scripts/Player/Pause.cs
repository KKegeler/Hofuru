using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Framework.Messaging;

public class Pause : MonoBehaviour {

    public void Paused()
    {
        MessagingSystem.Instance.QueueMessage(new PauseMessage(true));
        Time.timeScale = 0;
        SceneManager.LoadScene(0,LoadSceneMode.Additive);
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            Paused();
        }
    }
}
