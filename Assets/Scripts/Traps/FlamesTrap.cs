using UnityEngine;
using Framework.Messaging;
using Framework.Audio;

public class FlamesTrap : MonoBehaviour
{
    AudioSource auds;

    public ParticleSystem flam;
    float time = 5f;
    float wait = 5f;

    //public GameObject player;
    void Start() {
        this.auds = GetComponent<AudioSource>();
    }
    void Update()
    {
        time -= Time.deltaTime;
        flammen(time);
    }

   /// <summary>
   /// Sent when another object enters a trigger collider attached to this
   /// object (2D physics only).
   /// </summary>
   /// <param name="other">The other Collider2D involved in this collision.</param>
   void OnTriggerStay2D(Collider2D col)
   {
       if(col.GetComponent<Health>() && flam.isPlaying){
           float damage = 50*Time.deltaTime;
            col.GetComponent<Health>().ReduceHealth(damage);
       }
   }

    private void flammen(float timer)
    {
        if (timer > 0)
        {
            flam.Play();
            AudioManager.Instance.PlaySfx("Flamme", auds);
            wait = 2f;
        }
        else if (timer <= 0)
        {
            flam.Stop();
            wait -= Time.deltaTime;
            if(wait <= 0)
            {
                time = 2f;
            }
            
        }
    }
}
