using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Bhv_Seek seek = gameObject.AddComponent<Bhv_Seek>();
        seek.target = GameObjectBank.Instance.player.transform;
        seek.speed = 15.0f;
        seek.dist = 50.0f;
        Destroy(this);
	}
	
}
