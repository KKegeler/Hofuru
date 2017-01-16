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
        
    public void Update()
    {
        direction = me.position - (Vector2)threat.position;
        distanceSqr = direction.sqrMagnitude;
        if(distanceSqr < maxDistance * maxDistance) // if not at max Distance then run!
        {
            direction.y = 0.0f; // Ignore y component, only walk in x direction
            direction.Normalize();
            direction = me.position + (direction * speed * Time.deltaTime);
            me.MovePosition(direction);
        }
        else
        {
            me.velocity = Vector2.up * me.velocity.y;
        }
        Debug.Log(speed);
        animator.SetFloat("speed", speed);
    }

}
