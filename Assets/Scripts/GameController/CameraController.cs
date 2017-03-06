using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float followEffort;

    private GameObject player;
    private Camera mainCamera;
    private Vector3 newCamPos;
    private float originalSize;
    private IEnumerator zoomInRoutine;
    private IEnumerator resetZoomRoutine;
    private AnimationCurve zoomInCurve;

    // Use this for initialization
    void Start() {
        this.resetZoomRoutine = ResetZoomRoutine(0.05f);
        this.player = GameObjectBank.Instance.player;
        this.mainCamera = GameObjectBank.Instance.mainCamera.GetComponent<Camera>();
        this.originalSize = mainCamera.orthographicSize;
        newCamPos = new Vector3(0.0f, 0.0f, mainCamera.transform.position.z);
    }

    void FixedUpdate() {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Screen.width / Screen.height;

        newCamPos.x = Mathf.Clamp((followEffort * player.transform.position.x + (1.0f - followEffort) * mainCamera.transform.position.x), width / 2, 300);
        newCamPos.y = Mathf.Clamp((followEffort * player.transform.position.y + (1.0f - followEffort) * mainCamera.transform.position.y), height / 2, 300);
        mainCamera.transform.position = newCamPos;
    }

    public void ZoomIn(float duration, float minValue) {
        StopCoroutine(this.resetZoomRoutine);
        this.zoomInRoutine = ZoomInRoutine(duration, minValue);
        StartCoroutine(this.zoomInRoutine);
    }

    IEnumerator ZoomInRoutine(float duration, float minValue) {
        float stepTime = duration / 50;
        float reduceAmount = (originalSize - minValue) / 50;
        while (this.mainCamera.orthographicSize >= minValue) {
            this.mainCamera.orthographicSize -= reduceAmount;
            yield return new WaitForSecondsRealtime(stepTime);
        }
        if(this.mainCamera.orthographicSize <= minValue) {
            this.mainCamera.orthographicSize = minValue;
        }
    }

    IEnumerator ResetZoomRoutine(float duration) {
        float stepTime = duration / 20;
        float amount = (originalSize - mainCamera.orthographicSize) / 20;
        while (this.mainCamera.orthographicSize <= originalSize) {
            this.mainCamera.orthographicSize += amount;
            yield return new WaitForSecondsRealtime(stepTime); ;
        }
        if (this.mainCamera.orthographicSize >= originalSize) {
            this.mainCamera.orthographicSize = originalSize;
        }
    }

    public void ResetZoom() {
        StopCoroutine(this.zoomInRoutine);
        this.resetZoomRoutine = ResetZoomRoutine(0.05f);
        StartCoroutine(this.resetZoomRoutine);
    }
}
