﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Messaging;

public class EnemyDeath : Death {
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

    public override void HandleDeath() {
        MessagingSystem.Instance.QueueMessage(new ScoreMessage(100));
        for(int i = 0; i < colliders.Length; i++) {
            Collider2D col = (Collider2D)colliders[i];
            col.enabled = false;
        }
        this.rb.bodyType = RigidbodyType2D.Static;
        this.col.enabled = false;
        //this.sr.color = new Color(0, 0, 0, 0.25f);
        this.animator.SetBool("isDead", true);
        em.DisableMachine();
    }
}
