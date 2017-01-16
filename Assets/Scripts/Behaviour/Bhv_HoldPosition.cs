using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_HoldPosition : MonoBehaviour {

    [HideInInspector] public Vector2 position;

    private Rigidbody2D me;

	// Use this for initialization
	void Start () {
        me = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        me.MovePosition(position);
	}
}
