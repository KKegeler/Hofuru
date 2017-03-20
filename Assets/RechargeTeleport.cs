using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeTeleport : MonoBehaviour {

    [SerializeField]
    private float reloadTime;

    Image teleportReloader;
    PlayerStats ps;

    // Use this for initialization
    void Start() {
        this.ps = GetComponent<PlayerStats>();
        this.teleportReloader = GameObjectBank.Instance.teleportReloader;
        teleportReloader.enabled = false;
    }

    public void StartReload() {
        StopAllCoroutines();
        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine() {
        for (float i = 1; i <= 360; i++) {
            teleportReloader.enabled = true;
            Debug.Log(i / 360f);
            teleportReloader.fillAmount = i / 360;
            yield return new WaitForSeconds(reloadTime / 360);
        }
        ps.AddTeleport(1);
        teleportReloader.enabled = false;
    }
}
