using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 pos = GameObjectBank.Instance.Background.transform.GetChild(2).position;
        GameObjectBank.Instance.Background.transform.GetChild(2).position = new Vector3(transform.position.x, pos.y, pos.z);
	}
}
