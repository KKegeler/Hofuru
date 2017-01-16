using UnityEngine;
using System.Collections;

public class Bhv_Seek : MonoBehaviour
{

    [HideInInspector] public Transform target;
    [HideInInspector] public float speed;

    private Rigidbody2D me;
    private Vector2 direction;
    private float distanceSqr;
    private Animator animator;

    public void Start()
    {
        me = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        direction = (Vector2)target.position - me.position;
        distanceSqr = direction.sqrMagnitude;
        if (distanceSqr > 0.01f) // walk until near enough
        {
            direction.y = 0.0f; // Ignore y component, only walk in x direction
            direction.Normalize();
            direction = me.position + (direction * speed * Time.deltaTime);
            me.MovePosition(direction);
        }
        animator.SetFloat("speed", speed);
    }

}
