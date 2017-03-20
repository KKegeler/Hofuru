using System.Collections;
using UnityEngine;
using Framework.Messaging;

public class EnemyDeath : Death
{
    //private bool isDead;
    Rigidbody2D rb;
    Collider2D col;
    //SpriteRenderer sr;
    EnemyMachine em;
    Animator animator;
    Component[] colliders;
    EnemyGroundCheck groundCheck;

    void Start()
    {
        col = this.gameObject.GetComponent<Collider2D>();
        em = GetComponent<EnemyMachine>();
        animator = this.gameObject.GetComponent<Animator>();
        this.colliders = this.gameObject.GetComponentsInChildren<Collider2D>();
        groundCheck = GetComponentInChildren<EnemyGroundCheck>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void HandleDeath()
    {
        em.DisableMachine();

        rb.bodyType = RigidbodyType2D.Dynamic;

        MessagingSystem.Instance.QueueMessage(new ScoreMessage(100));
      
        this.animator.SetBool("isDead", true);

        EnemyAttack ea = GetComponentInChildren<EnemyAttack>();
        if (ea)
            ea.enabled = false;

        StartCoroutine(WaitForGrounded());
    }

    private IEnumerator WaitForGrounded()
    {
        while (!groundCheck.grounded)
            yield return new WaitForEndOfFrame();

        

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = (Collider2D)colliders[i];
            collider.enabled = false;
        }

        for (int i = 0; i < transform.childCount; ++i)
            transform.GetChild(i).gameObject.SetActive(false);

        col.enabled = false;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        enabled = false;
    }
}