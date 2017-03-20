using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Audio;

public class BlinkBlink : MonoBehaviour {
    SpriteRenderer sr;
    AudioSource auds;

	// Use this for initialization
	void Start () {
        this.sr = GetComponent<SpriteRenderer>();
        this.auds = GetComponent<AudioSource>();
        StartCoroutine(BlinkBlinkRoutine());
	}
	
    IEnumerator BlinkBlinkRoutine() {
        while (true) {
            sr.color = Color.red;
            AudioManager.Instance.PlaySfx("BombeBlip", auds);
            yield return new WaitForSeconds(0.3f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
