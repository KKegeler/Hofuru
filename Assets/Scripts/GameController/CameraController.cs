using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float followEffort;

    private GameObject player;
    private Camera mainCamera;
    private Vector3 newCamPos;

    // Use this for initialization
    void Start() {
        this.player = GameObjectBank.instance.player;
        this.mainCamera = GameObjectBank.instance.mainCamera.GetComponent<Camera>();
        newCamPos = new Vector3(0.0f, 0.0f, mainCamera.transform.position.z);
    }

    void FixedUpdate() {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Screen.width / Screen.height;

        newCamPos.x = Mathf.Clamp((followEffort * player.transform.position.x + (1.0f - followEffort) * mainCamera.transform.position.x), width / 2, 300);
        newCamPos.y = Mathf.Clamp((followEffort * player.transform.position.y + (1.0f - followEffort) * mainCamera.transform.position.y), height / 2, 300);
        mainCamera.transform.position = newCamPos;
    }
}
