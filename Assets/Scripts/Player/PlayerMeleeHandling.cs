using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHandling : MonoBehaviour {

    private GameObject player;
    public float meleeForce;
    public GameObject bloodParticle;

    void Start() {
        this.player = GameObjectBank.instance.player;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Enemy>() == null) {
            return;
        }
        Rigidbody2D otherRigi = other.GetComponent<Rigidbody2D>();
        Health hc = other.gameObject.GetComponent<Health>();
        Vector2 dir = Vector2.right;
        if (player.transform.position.x > other.gameObject.transform.position.x) {
            dir = Vector2.left;
        }
        hc.ReduceHealth(60);
        this.GetComponent<Collider2D>().enabled = false;
        Instantiate(GameObjectBank.instance.blut, other.transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(-100, 100))));
        otherRigi.AddForce(dir * meleeForce, ForceMode2D.Impulse);
        Instantiate(bloodParticle, other.transform.position, Quaternion.identity);
    }
}
