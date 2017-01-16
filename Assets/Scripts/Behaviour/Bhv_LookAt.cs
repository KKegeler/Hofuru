using UnityEngine;
using System.Collections;

public class Bhv_LookAt : MonoBehaviour {

    [HideInInspector] public Transform target;
    [HideInInspector] public bool rightOrientated = false;

    private Rigidbody2D me;

    public void Start()
    {
        me = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(target.position.x - me.position.x < 0)
        {
            // TODO Look left animation
            // Dummy
            GetComponent<SpriteRenderer>().flipX = rightOrientated; // if rightOrientated then flip, else do not flip
            // TODO Activate left collider and disable right
        }
        else
        {
            // TODO Look right animation
            // Dummy
            GetComponent<SpriteRenderer>().flipX = !rightOrientated; // if rightOrientated then do not flip, else flip
            // TODO Activate right collider and disable left
        }
    }

}
