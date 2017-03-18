using Framework.Pool;
using UnityEngine;

public class PlayerMeleeHandling : MonoBehaviour {

    public int damage;
    private GameObject player;
    public float meleeForce;
    public GameObject bloodParticle;
    private GameObject blood;

    void Start() {
        this.player = GameObjectBank.Instance.player;
        blood = GameObjectBank.Instance.blut;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Enemy>() == null) {
            return;
        }
        Rigidbody2D otherRigi = other.GetComponent<Rigidbody2D>();
        Health hc = other.gameObject.GetComponent<Health>();
        Vector2 dir = Vector2.right;
        other.GetComponent<Enemy>().FreezeForSeconds(1);
        if (player.transform.position.x > other.gameObject.transform.position.x) {
            dir = Vector2.left;
        }
        hc.ReduceHealth(damage);
        this.GetComponent<Collider2D>().enabled = false;

        PoolManager.Instance.ReuseObject2(blood, other.transform.position, 
            Quaternion.Euler(new Vector3(0, 0, Random.Range(-100, 101))));

        otherRigi.AddForce(dir * meleeForce, ForceMode2D.Impulse);
        //Instantiate(bloodParticle, other.transform.position, Quaternion.identity);
        PoolManager.Instance.ReuseObject(bloodParticle, other.transform.position, 
            Quaternion.identity);
    }
}
