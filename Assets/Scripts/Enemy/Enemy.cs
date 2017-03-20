using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private float dt;
    private bool timer;
    private float seconds;
    private EnemyMachine stateMachine;
    private float animationSpeed;
    private List<GameObject> attachedShuriken;
    private Rigidbody2D rigi;

	// Use this for initialization
	void Start () {
        this.rigi = GetComponent<Rigidbody2D>();
        this.attachedShuriken = new List<GameObject>();
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
        rigi.bodyType = RigidbodyType2D.Static;
        if (stateMachine)
        {
            stateMachine.FreezeMachine(); // disables behaviour
            stateMachine.enabled = false;
        }
        Animator anim = GetComponent<Animator>();
        if (animationSpeed == -1.0f)
        {
            animationSpeed = anim.GetFloat("speed");
        }
        anim.SetFloat("speed", 0.0f);
    }

    public void Unfreeze()
    {
        if (stateMachine)
        {
            stateMachine.enabled = true;
            stateMachine.UnfreezeMachine(); // enables behaviour
            rigi.bodyType = RigidbodyType2D.Dynamic;
        }
        GetComponent<Animator>().SetFloat("speed", animationSpeed);
        animationSpeed = -1.0f;
    }

    public void FreezeForSeconds(float sec)
    {
        seconds = sec;
        dt = 0.0f;
        Freeze();
        timer = true;
    }

    public void AddAttachedShuriken(GameObject shuriken) {
        this.attachedShuriken.Add(shuriken);
    }

    public void DestroyAllAttachedShuriken() {
        foreach(GameObject shuriken in attachedShuriken) {
            shuriken.gameObject.SetActive(false);
        }
    }
	
}
