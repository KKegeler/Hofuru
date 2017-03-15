using UnityEngine;
using UnityEngine.SceneManagement;
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

            SceneManager.LoadScene(1);
        }

    }
}