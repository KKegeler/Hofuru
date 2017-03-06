using UnityEngine;
using System.Collections;

public class ShurikenThrow : MonoBehaviour {

    public GameObject shurikenPrefab;
    private GameObject player;
    private bool isMeasuering;
    private bool canShoot;
    private float delta;
    private PlayerStats pStats;
    

    // Use this for initialization
    void Start() {
        this.pStats = GameObjectBank.Instance.GetComponent<PlayerStats>();
        player = GameObjectBank.Instance.player;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Shoot") && pStats.GetCurrentShurikenCount() > 0) {
            this.pStats.DecreaseCurrSchurikenCount();
            this.Throw(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }    
    }

    private void Throw(float x, float y) {
        Vector3 pos = player.transform.position + new Vector3(x, y, 0).normalized * 3;
        GameObject shuriken = (GameObject)Instantiate(shurikenPrefab, pos, Quaternion.identity);
        shuriken.GetComponent<Fly>().DoFly(x, y);
    }
}
