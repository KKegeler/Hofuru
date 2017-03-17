using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Pool;

public class SpikeDeath : MonoBehaviour {

    [SerializeField]
    private GameObject bloodParticle;
    private List<GameObject> victims;

    void Start() {
        this.victims = new List<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(victims.Contains(other.gameObject)) {
            return;
        }
        victims.Add(other.gameObject);
        Health hc = other.gameObject.GetComponent<Health>();
        if (hc) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            hc.ReduceHealth(10000f);
            if (enemy) {
                BoxCollider2D col = enemy.GetComponent<BoxCollider2D>();
                col.enabled = true;
                col.size = new Vector2(col.size.x, 0.1f);
                col.offset = new Vector2(col.offset.x, col.offset.y - 2);
            }
            PoolManager.Instance.ReuseObject2(GameObjectBank.Instance.blut, this.transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-100, 101))));
            PoolManager.Instance.ReuseObject(bloodParticle, other.transform.position, Quaternion.identity);
        }

    }
}
