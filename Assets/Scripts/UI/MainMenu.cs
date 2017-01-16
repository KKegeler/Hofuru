using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Window {

    public Button continueButton;

    public void continueGame()
    {
        //SceneManager.LoadScene(1,LoadSceneMode.Additive);
    }

	public void startGame()
    {
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void options()
    {
        manager.Open(1);
    }
}
