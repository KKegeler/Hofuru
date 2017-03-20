using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Framework.Messaging;
using Framework.Pool;

namespace StartUp
{
    /// <summary>
    /// Start the game
    /// </summary>
    public class StartUpManager : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;

            GameManager.Instance.WakeUp();
            MessagingSystem.Instance.WakeUp();
            PoolManager.Instance.WakeUp();

            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(2f);
            yield return SceneManager.LoadSceneAsync("MainMenu");
        }

    }
}