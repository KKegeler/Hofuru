using UnityEngine;
using System.Collections;
using System;

public class FlamesTrap : MonoBehaviour
{
    public ParticleSystem flam;
    float time = 5f;
    float wait = 5f;


    void Update()
    {
        time -= Time.deltaTime;
        flammen(time);
        
        
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
