using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameObjectBank : MonoBehaviour {
    public static GameObjectBank instance;
    public GameObject player;
    public GameObject gameController;
    public GameObject mainCamera;
    public GameObject playerMeleeTrigger;
    public GameObject teleportTarget;
    public GameObject blut;
    public Text hud_ShurikenCounter;
    public Shader greyscaleShader;

    public GameObject[] prefabsForPooling;

    public void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
