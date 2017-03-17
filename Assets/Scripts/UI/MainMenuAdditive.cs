using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAdditive : MonoBehaviour
{
    public void Continue()
    {
        GameManager.Instance.State = GameState.INGAME;    
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.State = GameState.INGAME;  
    }

}