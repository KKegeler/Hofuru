using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideKick : MonoBehaviour {

    private PlayerMovement pm;

	// Use this for initialization
	void Start () {
        this.pm = GameObjectBank.instance.player.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (pm.DoesSlide() && other.GetComponent<Enemy>()) {
            Vector2 dir;
            dir = (other.transform.position - this.transform.position);
            dir.x *= 10;
            dir = dir.normalized;
            Debug.Log("dir: " + dir);
            other.GetComponent<Rigidbody2D>().AddForce(dir * 250, ForceMode2D.Impulse);
            /// TODO: FREEZE
        }
    }
}
