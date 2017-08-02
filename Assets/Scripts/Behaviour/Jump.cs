using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    private Rigidbody2D body;
    private EnemyGroundCheck groundCheck;
    private Vector2 jumpDirLeft;
    private Vector2 jumpDirRight;
    public float jumpForce = 350.0f;

    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>(); // this script is part of the TrapEvasion
        groundCheck = transform.parent.GetComponentInChildren<EnemyGroundCheck>();
        float a = 60.0f * Mathf.Deg2Rad;
        float cos = Mathf.Cos(a);
        float sin = Mathf.Sin(a);
        jumpDirLeft = new Vector2(-cos, sin);
        jumpDirRight = new Vector2(cos, sin);
    }

    public void JumpLeft()
    {
        groundCheck.grounded = false;
        body.velocity = Vector2.zero;
        body.AddForce(jumpDirLeft * jumpForce, ForceMode2D.Impulse);
    }

    public void JumpRight()
    {
        groundCheck.grounded = false;
        body.velocity = Vector2.zero;
        body.AddForce(jumpDirRight * jumpForce, ForceMode2D.Impulse);
    }

    public void JumpLeft(float force)
    {
        float tmp = jumpForce;
        jumpForce = force;
        JumpLeft();
        jumpForce = tmp;
    }

    public void JumpRight(float force)
    {
        float tmp = jumpForce;
        jumpForce = force;
        JumpRight();
        jumpForce = tmp;
    }

}
