using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public void ende()
	{
		Scene activ = SceneManager.GetActiveScene();
		SceneManager.LoadScene(activ.name);
	}
}
