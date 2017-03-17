using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [Range(0f, 1f)]
    public float followEffort;

    private Transform player;
    private Transform camTf;
    private Camera mainCamera;
    private Vector3 newCamPos;
    private float originalSize;
    private IEnumerator zoomInRoutine;
    private IEnumerator resetZoomRoutine;
    private float height;
    private float width;

    private float maxWidth;
    private float maxHeight;

    // Use this for initialization
    void Start() {
        this.resetZoomRoutine = ResetZoomRoutine(0.05f);
        this.player = GameObjectBank.Instance.player.transform;
        this.mainCamera = GameObjectBank.Instance.mainCamera;
        camTf = mainCamera.transform;
        this.originalSize = mainCamera.orthographicSize;
        newCamPos = new Vector3(0.0f, 0.0f, mainCamera.transform.position.z);

        height = mainCamera.orthographicSize * 2;
        width = height * Screen.width / Screen.height;
        height /= 2;
        width /= 2;

        Transform levelEnd = GameObjectBank.Instance.levelEnd.transform;
        maxWidth = levelEnd.position.x;
        maxHeight = levelEnd.position.y;
    }

    void FixedUpdate() {
        height = mainCamera.orthographicSize * 2;
        width = height * Screen.width / Screen.height;
        height *= 0.5f;
        width *= 0.5f;
        newCamPos.x = Mathf.Clamp((followEffort * player.position.x + (1.0f - followEffort) * camTf.position.x), width, maxWidth - width);
        newCamPos.y = Mathf.Clamp((followEffort * player.position.y + (1.0f - followEffort) * camTf.position.y), height, maxHeight - height);
        camTf.position = newCamPos;
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
