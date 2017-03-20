﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextLevel : MonoBehaviour {
    private Animator playerAnimator;

    void Start() {
        this.playerAnimator = GameObjectBank.Instance.player.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerDeath>()) {
            playerAnimator.speed = 0f;
            GameObjectBank.Instance.nextLevelScreen.SetActive(true);
            GameObjectBank.Instance.player.GetComponent<PlayerMovement>().enabled = false;
            GameObjectBank.Instance.player.GetComponent<Pause>().enabled = false;
            GameObjectBank.Instance.player.GetComponent<Teleport>().enabled = false;
            GameObjectBank.Instance.player.GetComponent<MeleeAttack>().enabled = false;
            EventSystem.current.SetSelectedGameObject(GameObjectBank.Instance.nextLevel.gameObject);
    
        }
    }

}
