using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAnimation : MonoBehaviour {

    [HideInInspector] public const float animTime = 0.555f; // animation duration is 0.833 and animation speed is 1.5

    private Animator animator;
    //private float time;
    //private bool play;
    
    // Use this for initialization
    void Start () {
        animator = transform.parent.GetComponent<Animator>();
        //time = 0.0f;
        //play = false;
    }
	
	//// Update is called once per frame
	//void Update () {
    //    if (play)
    //    {
    //        if(time <= 0.0f)
    //        {
    //            animator.SetBool("doesAttack", false);
    //            play = false;
    //            return;
    //        }
    //        time -= Time.deltaTime;
    //    }
	//}

    public void PlayAnimation()
    {
        //time = animTime;
        //play = true;
        animator.SetBool("doesAttack", true);
    }
}
