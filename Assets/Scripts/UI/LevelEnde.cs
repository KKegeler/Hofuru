using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnde : MonoBehaviour {

	public void lvlEnde()
	{
		int activ = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(activ+1);
	}
}
