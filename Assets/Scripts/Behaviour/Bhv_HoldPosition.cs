using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_HoldPosition : MonoBehaviour
{

    [HideInInspector]
    public Vector2 position;
    private float alpha; // 0.05f is pretty good, position and me.position aren't that different after all

    private Rigidbody2D me;

    // Use this for initialization
    void Start()
    {
        me = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().SetFloat("speed", 0.0f);
        alpha = 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (me.bodyType != RigidbodyType2D.Static)
            me.MovePosition(alpha * position + (1.0f - alpha) * me.position);
    }
}
