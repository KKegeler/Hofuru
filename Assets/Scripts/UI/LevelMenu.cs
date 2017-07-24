using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : Window {

	public void LoadLevel1(){
        Time.timeScale = 1;
        GameManager.Instance.LState = LevelState.LEVEL_1;
        GameManager.Instance.State = GameState.INGAME; 
	}

	public void LoadLevel2(){
        Time.timeScale = 1;
        GameManager.Instance.LState = LevelState.LEVEL_2;
        GameManager.Instance.State = GameState.INGAME; 
	}

	public void LoadLevel3(){
        Time.timeScale = 1;
        GameManager.Instance.LState = LevelState.LEVEL_3;
        GameManager.Instance.State = GameState.INGAME; 
	}

	public void LoadGYM_KI(){
        Time.timeScale = 1;
        GameManager.Instance.LState = LevelState.GYM_KI;
        GameManager.Instance.State = GameState.INGAME; 
	}

	public void Back(){
		manager.Open(0);
	}

}
