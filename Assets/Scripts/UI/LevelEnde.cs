using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnde : MonoBehaviour {

	public void lvlEnde()
	{
		string activ = SceneManager.GetActiveScene().name;

        switch (activ)
        {
            case "Level_1":
                GameManager.Instance.LState = LevelState.LEVEL_2;
                break;

            case "Level_2":
                GameManager.Instance.LState = LevelState.LEVEL_3;
                break;

            case "Level_3":
                GameManager.Instance.LState = LevelState.END;
                break;
        }
	}
}
