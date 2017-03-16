using UnityEngine;
using UnityEngine.SceneManagement;
using Framework.Pool;

public class PauseMenu : MonoBehaviour {

	public void backToMenu()
    {
        Time.timeScale = 1;
        PoolManager.Instance.ResetPool();
        SceneManager.LoadScene(1);
    }
}
