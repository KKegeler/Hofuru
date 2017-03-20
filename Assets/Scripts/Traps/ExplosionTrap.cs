using UnityEngine;
using Framework.Messaging;
using Framework.Audio;

public class ExplosionTrap : MonoBehaviour {

    public ParticleSystem exp;
    public GameObject damageArea;
 
    /// <summary>
    /// Wenn der Trigger ausgelöst wird,wird das Partikelsystem abgespielt und das GameObject wird gelöscht
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<CanTriggerBomb>())
        {
            CameraShake.Instance.ShakeCamera(0.6f, 20f, 7.5f);
            damageArea.SetActive(true);
            gameObject.SetActive(false);
            exp.Play();
            AudioManager.Instance.PlaySfx("Explosion");    
        }

    }
}
