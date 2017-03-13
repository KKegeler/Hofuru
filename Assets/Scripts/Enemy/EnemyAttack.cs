using Framework.Pool;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public float damage;
    //public float meleeForce;
    public float minDelay;
    public float maxDelay;

    private float attackTime;
    private float halfAnimTime;
    private bool animPlaying;
    private bool inRange;

    public GameObject bloodParticle;
    
    private Rigidbody2D rbPlayer;
    private Health hcPlayer;
    private GameObject blood;
    private EnemyMeleeAnimation animation;
    private Animator animator;

    // Use this for initialization
    void Start () {
        rbPlayer = GameObjectBank.Instance.player.GetComponent<Rigidbody2D>();
        hcPlayer = GameObjectBank.Instance.player.GetComponent<Health>();
        animation = GetComponent<EnemyMeleeAnimation>();
        animator = transform.parent.GetComponent<Animator>();
        blood = GameObjectBank.Instance.blut;
        attackTime = 0.15f; // a little reaktion time
        halfAnimTime = EnemyMeleeAnimation.animTime * 0.8f;
        animPlaying = false;
    }
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("doesMelee", inRange);
        if (inRange)
        {
            if (attackTime > 0.0f)
            {
                attackTime -= Time.deltaTime;
                return;
            }
            animation.PlayAnimation();
            // new delay
            attackTime = Random.Range(minDelay, maxDelay);
        }
	}

    public void DealDamage()
    {
        Vector2 dir = Vector2.right;
        if (transform.parent.position.x > rbPlayer.position.x)
        {
            dir = Vector2.left;
        }
        hcPlayer.ReduceHealth(damage);
        //Instantiate(GameObjectBank.Instance.blut, other.transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(-100, 100))));
        PoolManager.Instance.ReuseObject2(blood, rbPlayer.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-100, 101))));
        //rbPlayer.AddForce(dir * meleeForce, ForceMode2D.Impulse);
        Instantiate(bloodParticle, rbPlayer.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>())
        {
            // its the player
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>())
        {
            // its the player
            inRange = false;
        }
    }
}
