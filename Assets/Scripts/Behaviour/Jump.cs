using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    private Rigidbody2D body;
    private EnemyGroundCheck groundCheck;
    private Vector2 jumpDirLeft;
    private Vector2 jumpDirRight;
    private const float jumpForce = 300.0f;

    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>(); // this script is part of the TrapEvasion
        groundCheck = (transform.parent != null) ? transform.parent.GetComponentInChildren<EnemyGroundCheck>() : null;
        float a = 60.0f * Mathf.Deg2Rad;
        float cos = Mathf.Cos(a);
        float sin = Mathf.Sin(a);
        jumpDirLeft = new Vector2(-cos, sin);
        jumpDirRight = new Vector2(cos, sin);
    }

    public void JumpLeft(float force = jumpForce)
    {
        JumpTo(jumpDirLeft, force);
    }

    public void JumpRight(float force = jumpForce)
    {
        JumpTo(jumpDirRight, force);
    }
    
    public void JumpTo(Vector2 direction, float force)
    {
        if(null != groundCheck) groundCheck.grounded = false;
        body.velocity = Vector2.zero;
        body.AddForce(direction * force, ForceMode2D.Impulse);
    }

}
