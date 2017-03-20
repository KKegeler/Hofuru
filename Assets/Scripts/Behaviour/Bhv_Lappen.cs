using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_Lappen : MonoBehaviour
{

    private Rigidbody2D me;
    private Animator anim;

    // Use this for initialization
    void Start()
    {
        me = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (anim.GetBool("isGrounded"))
        {
            if (me.bodyType != RigidbodyType2D.Static)
                me.velocity = Vector2.zero;

            me.bodyType = RigidbodyType2D.Static;
            Destroy(this); // destroy bhv_Lappen
        }
        /*else
        {
            if (me.bodyType != RigidbodyType2D.Static)
                me.velocity = Vector2.up * me.velocity.y;
        }*/
    }
}
