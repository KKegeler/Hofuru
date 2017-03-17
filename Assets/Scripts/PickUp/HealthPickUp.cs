using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int _amount;
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.AddHealth(_amount);
            gameObject.SetActive(false);
        }
    }

}