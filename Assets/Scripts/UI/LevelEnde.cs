using Framework.Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnde : MonoBehaviour {

	public void lvlEnde()
	{
		int activ = SceneManager.GetActiveScene().buildIndex;
        PoolManager.Instance.ResetPool();
		SceneManager.LoadScene(activ+1);
	}
}
