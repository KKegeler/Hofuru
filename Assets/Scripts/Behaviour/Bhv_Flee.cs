using UnityEngine;
using System.Collections;

public class Bhv_Flee : MonoBehaviour {

    [HideInInspector] public Transform threat;
    [HideInInspector] public float speed;
    [HideInInspector] public float maxDistance;

    private Rigidbody2D me;
    private Vector2 direction;
    private float distanceSqr;
    private Animator animator;
    
    void Start()
    {
        me = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
        
    public void FixedUpdate()
    {
        direction = me.position - (Vector2)threat.position;
        distanceSqr = direction.sqrMagnitude;
        if(distanceSqr < maxDistance * maxDistance) // if not at max Distance then run!
        {
            direction.Normalize();
            Vector2 newVelo = new Vector2(direction.x * speed, me.velocity.y);
            me.velocity = newVelo;
        }
        else
        {
            me.velocity = new Vector2(0.0f, me.velocity.y);
        }
        animator.SetFloat("speed", speed);
    }

}
