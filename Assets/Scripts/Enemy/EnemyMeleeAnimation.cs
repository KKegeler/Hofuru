using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAnimation : MonoBehaviour {
    
    private Animator animator;
    private float time;
    private bool play;

    // Use this for initialization
    void Start () {
        animator = transform.parent.GetComponent<Animator>();
        time = 0.0f;
        play = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (play)
        {
            if(time <= 0.0f)
            {
                animator.SetBool("doesMelee", false);
                play = false;
                return;
            }
            time -= Time.deltaTime;
        }
	}

    public void PlayAnimation()
    {
        time = 0.0066f; // animation duration is about 0.01 and animation speed is 1.5
        play = true;
        animator.SetBool("doesMelee", true);
    }
}
