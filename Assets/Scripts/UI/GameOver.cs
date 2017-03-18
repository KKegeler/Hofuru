using Framework.Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public void ende()
	{
		Scene activ = SceneManager.GetActiveScene();
        PoolManager.Instance.ResetPool();
        GameManager.Instance.State = GameState.INGAME;
		SceneManager.LoadScene(activ.name);
	}
}
