using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamage : MonoBehaviour
{

    private GameObject bomb;

    // Use this for initialization
    void Start()
    {
        this.bomb = gameObject.transform.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rigi = other.gameObject.GetComponent<Rigidbody2D>();
        Health hc = other.gameObject.GetComponent<Health>();
        PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();

        if (rigi != null)
        {
            Vector2 direction = (Vector2)other.gameObject.transform.position - (Vector2)this.gameObject.transform.position;
            direction.y *= 10;
            direction.Normalize();

            rigi.bodyType = RigidbodyType2D.Dynamic;

            if (pm && pm.DoesSlide())
            {
                pm.InterruptSlideAndThrow(Vector2.up, 500f);
            }
            else if (other.GetComponent<Fly>())
            {
                rigi.AddForce(direction * 250f, ForceMode2D.Impulse);
            }
            else
            {
                rigi.AddForce(direction * 500f, ForceMode2D.Impulse);
            }

            if (hc != null)
            {
                hc.ReduceHealth(60f);
            }

            Destroy(bomb, 0.3f);
        }
    }
}
