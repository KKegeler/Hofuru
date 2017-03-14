using UnityEngine;
using System.Collections;
using Framework.Messaging;
using UnityEngine.SceneManagement;

public class MainMenuAdditive : MonoBehaviour
{
    public void Continue()
    {
        GameManager.Instance.State = GameState.INGAME;    
    }

}