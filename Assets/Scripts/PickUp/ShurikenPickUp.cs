using UnityEngine;

public class ShurikenPickUp : MonoBehaviour
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
            ps.AddSchuriken(_amount);
            gameObject.SetActive(false);
        }
    }

}