using UnityEngine;
using System.Collections;

public class Bhv_Seek : MonoBehaviour
{

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public float speed;
    public float dist = 0.01f;

    private Rigidbody2D me;
    private EnemyGroundCheck groundCheck;
    private Vector2 direction;
    private float distanceSqr;
    private Animator animator;

    public void Start()
    {
        me = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<EnemyGroundCheck>();
    }

    public void FixedUpdate()
    {
        direction = (Vector2)target.position - me.position;
        distanceSqr = direction.sqrMagnitude;
        if ((distanceSqr > dist) && (groundCheck.grounded)) // walk until near enough
        {
            direction.Normalize();
            Vector2 newVelo = new Vector2(direction.x * speed, (me.velocity.y != float.NaN) ? me.velocity.y : 0.0f);
            if (me.bodyType != RigidbodyType2D.Static)
                me.velocity = newVelo;
        }
        animator.SetFloat("speed", speed);
    }

}
