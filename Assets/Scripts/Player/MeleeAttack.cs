using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {
    public float damage;

    private float meleeCoolDownTime;
    private BoxCollider2D playerMeleeTrigger;
    private GameObject player;
    private Animator animator;
    private bool meleeDisabled;
    private bool canAttack;
    private float time;

    // Use this for initialization
    void Start() {
        this.meleeCoolDownTime = 0.35f;
        playerMeleeTrigger = GameObjectBank.instance.playerMeleeTrigger.GetComponent<BoxCollider2D>();
        player = GameObjectBank.instance.player;
        animator = player.GetComponent<Animator>();
        meleeDisabled = false;
        canAttack = true;
        time = 0f;
    }

    // Update is called once per frame
    void Update() {
        if (!meleeDisabled) {
            if (Input.GetButtonDown("Melee") && canAttack) {
                canAttack = false;
                this.playerMeleeTrigger.enabled = true;
                animator.SetBool("doesMelee", true);
            }
        }

        if (!canAttack) {
            if (time <= meleeCoolDownTime) {
                time += Time.deltaTime;
            } else {
                time = 0;
                canAttack = true;
                this.playerMeleeTrigger.enabled = false;
                animator.SetBool("doesMelee", false);
            }
        }
    }



    public void DisableMelee() {
        this.meleeDisabled = true;
    }

    public void EnableMelee() {
        this.meleeDisabled = false;
    }
}
