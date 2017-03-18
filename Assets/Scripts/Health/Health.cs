using System.Collections;
using System.Collections.Generic;
using Framework.Messaging;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;

    private SpriteRenderer sr; 
    private Death dc;

    // Use this for initialization
    void Start() {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        dc = this.gameObject.GetComponent<Death>();
        if (this.currentHealth > this.maxHealth) {
            this.currentHealth = this.maxHealth;
        }
    }
    
    public void ReduceHealth(float value) {
        StopAllCoroutines();
        if (this.gameObject.GetComponent<PlayerMovement>()) {
            CameraShake.Instance.ShakeCamera(0.3f, 5f, 5f);
        }
        StartCoroutine(HitBlinkRoutine());
        this.currentHealth -= value;
        if (this.gameObject.GetComponent<PlayerMovement>())
        {
            MessagingSystem.Instance.QueueMessage(new HealthMessage(currentHealth));
        }
        if (this.currentHealth <= 0) {
            StopAllCoroutines();
            sr.color = Color.white;
            dc.HandleDeath();
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        MessagingSystem.Instance.QueueMessage(new HealthMessage(currentHealth));
    }

    private IEnumerator HitBlinkRoutine() {
        for (int i = 0; i < 5; i++) {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
