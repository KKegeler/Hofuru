using UnityEngine;

public class MainMenuAdditive : MonoBehaviour
{
    public void Continue()
    {
        GameManager.Instance.State = GameState.INGAME;    
    }

}