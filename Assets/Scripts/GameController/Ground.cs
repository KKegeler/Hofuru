using UnityEngine;

public class Ground : MonoBehaviour {
    void Start() {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = 0;
        }
    }
}