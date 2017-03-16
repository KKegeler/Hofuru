using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFreeArea : MonoBehaviour {

    private Teleport teleport;
    private GameObject teleportTarget;

	// Use this for initialization
	void Start () {
        this.teleport = GameObjectBank.Instance.player.GetComponent<Teleport>();
        this.teleportTarget = GameObjectBank.Instance.teleportTarget;
	}

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject == teleportTarget) {
            teleport.SetCanPort(false);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == teleportTarget) {
            teleport.SetCanPort(true);
        }
    }
	
	
}
