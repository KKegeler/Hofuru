using UnityEngine;
using Framework.Pool;

/// <summary>
/// Spawns PickUps
/// </summary>
public class PickUpSpawner : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject _shurikenPickUp;
    #endregion

    /// <summary>
    /// Spawns a PickUp
    /// </summary>
    /// <param name="pos">Position</param>
    public void Spawn(Vector3 pos)
    {
        int rand = Random.Range(0, 11);
        if (rand != 5)
            return;

        float rand2 = Random.Range(0f, 1f);
        pos.y = 2;

        if (rand2 <= 0.5f)
            PoolManager.Instance.ReuseObject(_shurikenPickUp, pos, Quaternion.identity);
        else
            Debug.Log("Hier könnte ihr HealthPickUp liegen!\n");
    }

}