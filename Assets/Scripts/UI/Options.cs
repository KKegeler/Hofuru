﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

private Animator anim;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();
        anim.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}