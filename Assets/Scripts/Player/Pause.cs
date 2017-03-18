using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{


    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Joystick1Button7))
            && GameManager.Instance.State != GameState.PAUSE && GameManager.Instance.State != GameState.GAME_OVER)
        {
            GameManager.Instance.State = GameState.PAUSE;
            
        }
    }

}