using UnityEngine;
using System.Collections;
using Framework.Audio;

public class Teleport : MonoBehaviour {

    public float targetSpeed;
    public float maxDistance;

    private GameObject player;
    private GameObject teleportTarget;
    private PlayerMovement pm;
    private MeleeAttack ma;
    private TimeController timeController;
    private CameraController cameraController;
    private PlayerStats ps;
    private bool canPort;
    private bool isActive;

    void Start() {
        canPort = true;
        isActive = false;
        player = this.gameObject;
        timeController = GameObjectBank.Instance.GetComponent<TimeController>();
        cameraController = GameObjectBank.Instance.GetComponent<CameraController>();
        ps = GameObjectBank.Instance.GetComponent<PlayerStats>();
        teleportTarget = GameObjectBank.Instance.teleportTarget;
        pm = player.GetComponent<PlayerMovement>();
        ma = player.GetComponent<MeleeAttack>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonUp("TeleportCall") || ps.GetCurrentTimefreezeTime() <= 0) {
            isActive = false;
            canPort = true;
            pm.EnableMovement();
            ma.EnableMelee();
            teleportTarget.SetActive(false);
            timeController.SetOriginTime();
            cameraController.ResetZoom();
        }
        if (Input.GetButtonDown("TeleportCall") && ps.GetCurrentTimefreezeTime() > ps.minTimeNeededToFreeze) {
            isActive = true;
            if (isActive) {
                pm.DisableMovement();
                // ma.DisableMelee();
                teleportTarget.SetActive(true);
                teleportTarget.transform.position = player.transform.position;
                timeController.FreezeTime(0.4f, 0.02f, 20);
                cameraController.ZoomIn(0.2f, 17);
                //teleportTarget.GetComponent<BoxCollider2D>().size = player.GetComponent<BoxCollider2D>().size;             
            }

        }
        if (isActive) {
            //Vector3 moveDelta = new Vector3(Input.GetAxis("HorizontalR"), Input.GetAxis("VerticalR") * -1, 0) * Time.unscaledDeltaTime * targetSpeed;
            if (Vector3.Distance(teleportTarget.transform.position, player.transform.position) > maxDistance + 3 || !canPort || ps.GetCurrentTeleportCount() < 1) {
                teleportTarget.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else {
                teleportTarget.GetComponent<SpriteRenderer>().color = Color.white;
            }
            teleportTarget.transform.position = player.transform.position + new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * maxDistance;
            //teleportTarget.transform.Translate(moveDelta);
            ps.ReduceCurrentFreezeTime(Time.unscaledDeltaTime);
        }

        if (isActive && Input.GetButtonDown("TeleportConfirm") && canPort && ps.GetCurrentTeleportCount() > 0) {
            AudioManager.Instance.PlaySfx("Teleport");
            Vector3 newPos = teleportTarget.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            newPos.z = 0;
            player.transform.position = newPos;
            pm.EnableMovement();
            // ma.EnableMelee();
            isActive = false;
            teleportTarget.SetActive(false);
            timeController.SetOriginTime();
            cameraController.ResetZoom();
            ps.DecreaseCurrTeleportCount();
        }
        if (!isActive && ps.GetCurrentTimefreezeTime() < ps.maxTimefreezeTime) {
            ps.InCreaseCurrentFreezeTime(Time.unscaledDeltaTime / 2);
        }
    }

    public void EndTimeFreeze() {
        isActive = false;
        pm.EnableMovement();
        ma.EnableMelee();
        teleportTarget.SetActive(false);
        timeController.SetOriginTime();
        cameraController.ResetZoom();
    }

    public bool IsTimeFreezing() {
        return isActive;
    }

    public void SetCanPort(bool value) {
        this.canPort = value;
    }

    public bool GetIsActive() {
        return this.isActive;
    }
}
