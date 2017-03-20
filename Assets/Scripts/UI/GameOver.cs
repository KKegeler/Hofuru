using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public void ende()
	{
        StartCoroutine(WaitForIngame());
	}

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private IEnumerator WaitForIngame()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        GameManager.Instance.State = GameState.INGAME;
        GameManager.Instance.RestartLevel();
    }
}
