using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

    //private Collider2D trigger;
    private PlayerMovement pm;

    // Use this for initialization
    void Start() {
        //trigger = GetComponent<Collider2D>();
        pm = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Ground>() != null) {
            pm.SetisGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        pm.SetisGrounded(false);
    }
}
