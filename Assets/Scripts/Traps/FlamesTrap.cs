using UnityEngine;
using Framework.Messaging;

public class FlamesTrap : MonoBehaviour
{
    public ParticleSystem flam;
    float time = 5f;
    float wait = 5f;

    //public GameObject player;
    private Health hcPlayer;
    void Start()
    {
        hcPlayer = GameObjectBank.Instance.player.GetComponent<Health>();
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
       if(col.gameObject == GameObjectBank.Instance.player && flam.isPlaying){
           float damage = 50*Time.deltaTime;
            hcPlayer.ReduceHealth(damage);
       }
   }

    private void flammen(float timer)
    {
        if (timer > 0)
        {
            flam.Play();
            
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
