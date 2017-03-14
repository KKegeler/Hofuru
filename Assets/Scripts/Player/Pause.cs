using UnityEngine;

public class Pause : MonoBehaviour
{
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Joystick1Button7))
            && GameManager.Instance.State != GameState.PAUSE)
        {
            GameManager.Instance.State = GameState.PAUSE;
        }
    }

}