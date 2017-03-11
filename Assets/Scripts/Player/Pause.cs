using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour {

    public GameObject pauseMenu;
    public bool isPaused = false;
    public GameObject firstSelected;

    //Methode um auf das EventSystem zugreifen zu können
    protected EventSystem eventSystem
    {
        get { return GameObjectBank.Instance.eventSystem; }
    }

    public virtual void OnFocus()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }
    }
}
