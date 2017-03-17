using UnityEngine;
using Framework.Pool;

public class ShurikenThrow : MonoBehaviour {

    public GameObject shurikenPrefab;
    private GameObject player;
    private bool isMeasuering;
    private bool canShoot;
    private float delta;
    private PlayerStats pStats;
    private Teleport tp;
    

    // Use this for initialization
    void Start() {
        this.pStats = GameObjectBank.Instance.GetComponent<PlayerStats>();
        player = GameObjectBank.Instance.player;
        tp = player.GetComponent<Teleport>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Shoot") && pStats.GetCurrentShurikenCount() > 0 && tp.GetIsActive()) {
            this.pStats.DecreaseCurrSchurikenCount();
            this.Throw(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }    
    }

    private void Throw(float x, float y) {
        Vector3 pos = player.transform.position + new Vector3(x, y, 0).normalized * 3;
        //GameObject shuriken = (GameObject)Instantiate(shurikenPrefab, pos, Quaternion.identity);
        GameObject shuriken = PoolManager.Instance.ReuseObject2(shurikenPrefab, pos, Quaternion.identity);
        shuriken.GetComponent<Fly>().DoFly(x, y);
    }

}
