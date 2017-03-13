using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameObjectBank : MonoBehaviour {

    private static GameObjectBank _instance;

    public GameObject player;
    public GameObject gameController;
    public Camera mainCamera;
    public GameObject playerMeleeTrigger;
    public GameObject teleportTarget;
    public GameObject blut;
    public Text hud_ShurikenCounter;
    public Shader greyscaleShader;
    public EventSystem eventSystem;
    public GameObject groundChecker;

    public static GameObjectBank Instance
    {
        get { return _instance; }
    }

    public void Awake() {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }
}
