using UnityEngine;
using UnityEngine.UI;

public class GameObjectBank : MonoBehaviour {
    #region Variables
    private static GameObjectBank _instance;

    public GameObject player;
    public GameObject gameController;
    public GameObject mainCamera;
    public GameObject playerMeleeTrigger;
    public GameObject teleportTarget;
    public GameObject blut;
    public Text hud_ShurikenCounter;
    public Shader greyscaleShader;
    #endregion

    #region Properties
    public static GameObjectBank Instance
    {
        get { return _instance; }
    }
    #endregion

    public void Awake() {
        if (!_instance)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }
}
