﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : Death {

    Animator animator;
    PlayerMovement pm;
    Teleport tp;
    ShurikenThrow st;

    private bool isDead;

    // Use this for initialization
    void Start() {
        this.animator = this.gameObject.GetComponent<Animator>();
        this.pm = this.gameObject.GetComponent<PlayerMovement>();
        this.tp = this.gameObject.GetComponent<Teleport>();
        this.st = this.gameObject.GetComponent<ShurikenThrow>();
        this.isDead = false;
    }

    // Update is called once per frame
    void Update() {
        if (isDead) {
            if (this.pm.GetIsGrounded()) {
                this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                this.enabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.F1) && this.gameObject.GetComponent<PlayerMovement>()) {
            this.gameObject.GetComponent<Health>().ReduceHealth(14);
        }
    }

    public override void HandleDeath() {
        DataSerializer.Save();
        GreyscaleEffect.Instance.BlendToGrey();

        this.isDead = true;
        this.pm.enabled = false;
        if (tp.IsTimeFreezing()) {
            tp.EndTimeFreeze();
        }
        this.tp.enabled = false;
        this.st.enabled = false;
        this.animator.SetBool("isDead", true);
    }
}
