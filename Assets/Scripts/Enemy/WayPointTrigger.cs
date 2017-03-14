using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointTrigger : MonoBehaviour {

    [HideInInspector] public PatrolState pState;
    [HideInInspector] public GameObject attachedEnemy;
    [HideInInspector] public bool active = false;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(active && collider.gameObject == attachedEnemy)
        {
            pState.WayPointReachedCallback();
        }
    }

}
