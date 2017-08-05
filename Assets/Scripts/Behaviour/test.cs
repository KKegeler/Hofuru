using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public float force = 196.2f;
    private Jump jump;
    private Rigidbody2D body;
    private bool isJumping = false;

	// Use this for initialization
	void Start () {
        jump = GetComponent<Jump>();
        body = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update() {
        if (isJumping && body.velocity.y <= 0.0f)
        {
            Debug.Log(body.position.y - 5f);
            isJumping = false;
        }
        if (Input.GetButtonDown("TEST_KI"))
        {
            jump.JumpTo(Vector2.up, force);
            isJumping = true;
        }
	}
}
