using UnityEngine;
using System.Collections;

public class ExplosionTrap : MonoBehaviour {

    public ParticleSystem exp;
    public GameObject player;

    /// <summary>
    /// Wenn der Trigger ausgelöst wird,wird das Partikelsystem abgespielt und das GameObject wird gelöscht
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject == player)
        {
            Vector2 target = col.gameObject.transform.position;
            Vector2 bomb = gameObject.transform.position;
            Vector2 direction = target - bomb;
            Rigidbody2D rig = col.gameObject.GetComponent<Rigidbody2D>();

            direction.Normalize();

            //rig.AddForce(direction*1000,ForceMode2D.Impulse);
            rig.AddForce(new Vector2(direction.x * 1000, direction.y * 1000), ForceMode2D.Impulse);
            gameObject.SetActive(false);
            exp.Play();
            Destroy(gameObject, 0.8f);
        }
        
    }
}
