using UnityEngine;
using System.Collections;
using Framework.Pool;

public class Fly : PoolObject {

    public float speed;
    public float rotationSpeed;
    private Vector3 flyDir;
    private bool isFlying;
    private Transform privTranse;
    private Rigidbody2D rigiBody;
    private Collider2D privCollider;
    private bool canFreeze;

    private void Awake()
    {
        this.privTranse = this.GetComponent<Transform>();
        this.rigiBody = this.GetComponent<Rigidbody2D>();
        this.privCollider = this.GetComponent<Collider2D>();
        this.canFreeze = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (isFlying) {
            //this.privTranse.position = this.privTranse.position + (flyDir * speed * Time.deltaTime);
            this.privTranse.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void DoFly(float x, float y) {
        this.flyDir = new Vector3(x, y, 0).normalized;
        this.rigiBody.AddForce(new Vector2(x, y).normalized * speed, ForceMode2D.Impulse);
        isFlying = true;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (canFreeze && collision.collider.GetComponent<Enemy>()) {
            canFreeze = false;
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            this.rigiBody.bodyType = RigidbodyType2D.Kinematic;
            this.rigiBody.velocity = Vector2.zero;
            this.isFlying = false;
            this.gameObject.transform.SetParent(enemy.gameObject.transform);
            GetComponent<BoxCollider2D>().enabled = false;
            StopAllCoroutines();         
            StartCoroutine(FreezeRoutine(5f, enemy));
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if (collision.collider.GetComponent<Ground>() != null) {
            this.isFlying = false;
            this.rigiBody.bodyType = RigidbodyType2D.Static;
            this.privCollider.enabled = false;
        }
    }

    private IEnumerator FreezeRoutine(float duration, Enemy enemy) {

        enemy.AddAttachedShuriken(this.gameObject);
        enemy.Freeze();
        Rigidbody2D enemyRigi = enemy.gameObject.GetComponent<Rigidbody2D>();
        enemyRigi.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(duration);
        enemyRigi.bodyType = RigidbodyType2D.Dynamic;
        enemy.DestroyAllAttachedShuriken();
        enemy.Unfreeze();
    }

}