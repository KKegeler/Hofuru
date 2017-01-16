using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public void backToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
