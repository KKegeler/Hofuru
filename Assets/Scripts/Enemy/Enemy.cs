using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private float dt;
    private bool timer;
    private float seconds;
    private EnemyMachine stateMachine;
    private float animationSpeed;

	// Use this for initialization
	void Start () {
        dt = 0.0f;
        timer = false;
        stateMachine = GetComponent<EnemyMachine>();
        animationSpeed = -1.0f;
    }

    public void Update()
    {
        if (timer)
        {
            dt += Time.deltaTime;
            if(dt >= seconds)
            {
                timer = false;
                Unfreeze();
            }
        }
    }

    public void Freeze()
    {
        stateMachine.FreezeMachine(); // disables behaviour
        stateMachine.enabled = false;
        Animator anim = GetComponent<Animator>();
        if (animationSpeed == -1.0f)
        {
            animationSpeed = anim.speed;
        }
        anim.speed = 0.0f;
    }

    public void Unfreeze()
    {
        if (stateMachine)
        {
            stateMachine.enabled = true;
            stateMachine.UnfreezeMachine(); // enables behaviour
        }
        GetComponent<Animator>().speed = animationSpeed;
        animationSpeed = -1.0f;
    }

    public void FreezeForSeconds(float sec)
    {
        seconds = sec;
        dt = 0.0f;
        Freeze();
        timer = true;
    }
	
}
