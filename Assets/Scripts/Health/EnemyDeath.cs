using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Messaging;

public class EnemyDeath : Death {
    private bool isDead;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sr;
    EnemyMachine em;
    Animator animator;
    Component[] colliders;

    // Use this for initialization
    void Start() {
        col = this.gameObject.GetComponent<Collider2D>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        em = GetComponent<EnemyMachine>();
        animator = this.gameObject.GetComponent<Animator>();
        this.colliders = this.gameObject.GetComponentsInChildren<Collider2D>();
    }

    void Update() {
        if (isDead) {
            if (this.animator.GetBool("isGrounded")) {
                this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                this.enabled = false;
            }
        }
    }

    public override void HandleDeath() {
        MessagingSystem.Instance.QueueMessage(new ScoreMessage(100));

        for(int i = 0; i < colliders.Length; i++) {
            Collider2D col = (Collider2D)colliders[i];
            col.enabled = false;
        }
        this.isDead = true;
        this.col.enabled = false;
        //this.sr.color = new Color(0, 0, 0, 0.25f);
        this.animator.SetBool("isDead", true);
        em.DisableMachine();
        GetComponentInChildren<EnemyAttack>().enabled = false;

        for (int i = 0; i < transform.childCount; ++i)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}
