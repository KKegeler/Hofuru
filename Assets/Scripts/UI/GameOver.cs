﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	private Scene[] scenes;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		//scenes = SceneManager.sceneCount;
		
	}

	public void ende()
	{
		Scene activ = SceneManager.GetActiveScene();
		SceneManager.LoadScene(activ.name);
	}
}
