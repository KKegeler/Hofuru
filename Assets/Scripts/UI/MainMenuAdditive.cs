using UnityEngine;
using System.Collections;
using Framework.Messaging;
using UnityEngine.SceneManagement;

public class MainMenuAdditive : MonoBehaviour
{
    public void Continue()
    {
        StartCoroutine(Unload());
    }

    private IEnumerator Unload()
    {
        yield return SceneManager.UnloadSceneAsync("MainMenu");

        Time.timeScale = 1;
        MessagingSystem.Instance.QueueMessage(new PauseMessage(false));
    }

}