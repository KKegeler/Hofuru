using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_TrapEvasion : MonoBehaviour {

    public bool left = false;
    private Rigidbody2D body;
    private Jump jump;

    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>();
        jump = GetComponentInParent<Jump>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((null != collision.transform.parent) && (collision.transform.parent.tag.Equals("trap")))
        {
            float dir = body.velocity.x;
            if (left && dir < 0.0f) jump.JumpLeft();
            else if (!left && dir > 0.0f) jump.JumpRight();
        }
    }

}
