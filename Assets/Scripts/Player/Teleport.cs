using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

    public float targetSpeed;
    public float maxDistance;

    private GameObject player;
    private GameObject teleportTarget;
    private PlayerMovement pm;
    private MeleeAttack ma;
    private TimeController timeController;
    private bool isActive;

    void Start() {
        isActive = false;
        player = this.gameObject;
        timeController = GameObjectBank.instance.GetComponent<TimeController>();
        teleportTarget = GameObjectBank.instance.teleportTarget;
        pm = player.GetComponent<PlayerMovement>();
        ma = player.GetComponent<MeleeAttack>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonUp("TeleportCall")) {
            isActive = false;
            pm.EnableMovement();
            ma.EnableMelee();
            teleportTarget.SetActive(false);
            timeController.SetOriginTime();
        }
        if (Input.GetButtonDown("TeleportCall")) {
            isActive = true;
            if (isActive) {
                pm.DisableMovement();
                // ma.DisableMelee();
                teleportTarget.SetActive(true);
                teleportTarget.transform.position = player.transform.position;
                timeController.FreezeTime(0.3f, 0.05f, 10);
                teleportTarget.GetComponent<BoxCollider2D>().size = player.GetComponent<BoxCollider2D>().size;
            }

        }
        if (isActive) {
            //Vector3 moveDelta = new Vector3(Input.GetAxis("HorizontalR"), Input.GetAxis("VerticalR") * -1, 0) * Time.unscaledDeltaTime * targetSpeed;
            if (Vector3.Distance(teleportTarget.transform.position, player.transform.position) > maxDistance) {
                teleportTarget.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else {
                teleportTarget.GetComponent<SpriteRenderer>().color = Color.white;
            }
            teleportTarget.transform.position = player.transform.position + new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * maxDistance;
            //teleportTarget.transform.Translate(moveDelta);
        }

        if (isActive && Input.GetButtonDown("TeleportConfirm")) {
            Vector3 newPos = teleportTarget.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            newPos.z = 0;
            player.transform.position = newPos;
            pm.EnableMovement();
            // ma.EnableMelee();
            isActive = false;
            teleportTarget.SetActive(false);
            timeController.SetOriginTime();
        }

    }
}
