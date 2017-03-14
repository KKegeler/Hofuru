using UnityEngine;
using Framework.Messaging;
using UnityEngine.SceneManagement;

public class MainMenuAdditive : MonoBehaviour {

	public void Continue()
	{
		Time.timeScale = 1;
		MessagingSystem.Instance.QueueMessage(new PauseMessage(false));
		Scene menu = SceneManager.GetSceneByName("MainMenu");
		SceneManager.UnloadSceneAsync(menu);
		
	}
}
