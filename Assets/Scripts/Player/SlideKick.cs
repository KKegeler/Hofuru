using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideKick : MonoBehaviour {

    private PlayerMovement pm;
    private Collider2D slideKickTrigger;

	// Use this for initialization
	void Start () {
        this.pm = GameObjectBank.Instance.player.GetComponent<PlayerMovement>();
        this.slideKickTrigger = GameObjectBank.Instance.slideKickTrigger.GetComponent<Collider2D>();

    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.F2)) {
            pm.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 1000, ForceMode2D.Impulse);
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (pm.DoesSlide() && other.GetComponent<Enemy>()) {
            DisableTriggerRoutine(0.3f);
            Enemy enemy = other.GetComponent<Enemy>();
            Vector2 dir;
            Vector2 dir2;
            enemy.FreezeForSeconds(1);
            
            dir = (other.transform.position - this.transform.position);
            dir.x *= 5;
            dir = dir.normalized;
            dir2 = new Vector2(-dir.x, 0);
            dir2 = dir2.normalized;
            pm.InterruptSlide();
            pm.GetComponent<Rigidbody2D>().AddForce(dir2 * 250, ForceMode2D.Impulse);
            pm.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 250, ForceMode2D.Impulse);
            other.GetComponent<Rigidbody2D>().AddForce(dir * 250, ForceMode2D.Impulse);
        }
    }

    private void DisableTriggerForSeconds(float duration) {
        StartCoroutine(DisableTriggerRoutine(duration));
    }

    private IEnumerator DisableTriggerRoutine(float duration) {
        slideKickTrigger.enabled = false;
        yield return new WaitForSeconds(duration);
        slideKickTrigger.enabled = true;
    }
}
