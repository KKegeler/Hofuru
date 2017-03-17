using UnityEngine;

public class TeleportPickUp : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int _amount;
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            PlayerStats ps = GameObjectBank.Instance.gameObject.GetComponent<PlayerStats>();
            ps.AddTeleport(_amount);
            gameObject.SetActive(false);
        }
    }

}