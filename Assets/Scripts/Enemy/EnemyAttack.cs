using Framework.Pool;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public float damage;
    //public float meleeForce;
    public float minDelay;
    public float maxDelay;

    private float attackTime;
    private bool inRange;

    public GameObject bloodParticle;
    
    private Rigidbody2D rbPlayer;
    private Health hcPlayer;
    private GameObject blood;
    private EnemyMeleeAnimation animation;

    // Use this for initialization
    void Start () {
        rbPlayer = GameObjectBank.Instance.player.GetComponent<Rigidbody2D>();
        hcPlayer = GameObjectBank.Instance.player.GetComponent<Health>();
        animation = GetComponent<EnemyMeleeAnimation>();
        blood = GameObjectBank.Instance.blut;
        attackTime = Random.Range(minDelay, maxDelay);
    }
	
	// Update is called once per frame
	void Update () {
        if (inRange)
        {
            if (attackTime > 0.0f)
            {
                attackTime -= Time.deltaTime;
                return;
            }
            // attack animation
            animation.PlayAnimation();
            // deal damage
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
            // new delay
            attackTime = Random.Range(minDelay, maxDelay);
        }
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
