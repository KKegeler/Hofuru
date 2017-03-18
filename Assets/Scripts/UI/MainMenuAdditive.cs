using UnityEngine;
using UnityEngine.SceneManagement;
using Framework.Pool;

public class MainMenuAdditive : MonoBehaviour
{
    public void Continue()
    {
        GameManager.Instance.State = GameState.INGAME;    
    }

    public void Restart()
    {
        PoolManager.Instance.ResetPool();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.State = GameState.INGAME;  
    }

}