using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public void ende()
	{
        StartCoroutine(WaitForIngame());
	}

    private IEnumerator WaitForIngame()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        GameManager.Instance.State = GameState.INGAME;
        GameManager.Instance.RestartLevel();
    }
}
