using System.Collections;
using System.Collections.Generic;
using Framework.Messaging;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;

    private Death dc;

    // Use this for initialization
    void Start() {
        dc = this.gameObject.GetComponent<Death>();
        if (this.currentHealth > this.maxHealth) {
            this.currentHealth = this.maxHealth;
        }
    }
    
    public void ReduceHealth(float value) {
        this.currentHealth -= value;
        if (this.gameObject.GetComponent<PlayerMovement>())
        {
            MessagingSystem.Instance.QueueMessage(new HealthMessage(currentHealth));
        }
        if (this.currentHealth <= 0) {
            dc.HandleDeath();
        }
    }
}
