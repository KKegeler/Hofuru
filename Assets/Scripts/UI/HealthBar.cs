using Framework.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Slider healthslider;

	// Use this for initialization
	void Start ()
    {
        MessagingSystem.Instance.AttachListener(typeof(HealthMessage), HealthHandler);
	}

    public bool HealthHandler(BaseMessage msg)
    {
        HealthMessage castMsg = (HealthMessage)msg;
        healthslider.value = castMsg.health;
        return true;
    }

    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
        {
            MessagingSystem.Instance.DetachListener(typeof(HealthMessage), HealthHandler);
        }
    }

}
