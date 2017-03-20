using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBlink : MonoBehaviour {
    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        this.sr = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkBlinkRoutine());
	}
	
    IEnumerator BlinkBlinkRoutine() {
        while (true) {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
