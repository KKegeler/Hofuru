using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public float maxSpeed;
    public float jumpPower;
    public float maxJumpCount;

    private Camera mainCamera;
    private int jumpCount;
    private float originalJumpPower;
    private float meleeCoolDown;
    private bool isGrounded;
    private bool facingRight;
    private bool doesMelee;
    private bool isRunning;
    private bool doesSlide;
    private bool cantMove;
    private float moveValue;
    private Animator animator;
    private Rigidbody2D rigiBody;
    private TimeController timeController;
    private BoxCollider2D playerCollider;

    // Use this for initialization
    void Start() {
        this.facingRight = true;
        this.doesMelee = false;
        this.cantMove = false;
        this.originalJumpPower = this.jumpPower;
        this.animator = this.GetComponent<Animator>();
        this.rigiBody = this.GetComponent<Rigidbody2D>();
        this.timeController = GameObjectBank.instance.gameController.GetComponent<TimeController>();
        this.mainCamera = GameObjectBank.instance.mainCamera.GetComponent<Camera>();
        this.playerCollider = this.GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (!this.cantMove) {
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount) {
                this.DoJump();
            }
            if (Input.GetAxis("RightTrigger") > 0.1f && !doesSlide && isRunning && isGrounded) {
                this.BeginSlide();
            }
            if (Input.GetAxis("RightTrigger") <= 0.5f) {
                this.EndSlide();
            }
        }
    }

    public bool GetIsGrounded() {
        return this.isGrounded;
    }

    void FixedUpdate() {
        //Movement Horizontal
        /*
        if (!this.cantMove && !this.doesSlide) {
            this.moveValue = Input.GetAxis("Horizontal");
            if (Mathf.Abs(moveValue) >= 0.1) {
                this.moveValue = 1f * this.moveValue / Mathf.Abs(this.moveValue);
                this.isRunning = true;
            } else {
                this.isRunning = false;
            }
        if (this.moveValue > 0 && !facingRight) {
            this.Flip();
        } else if (moveValue < 0 && facingRight) {
            this.Flip();
        }
        this.animator.SetFloat("speed", Mathf.Abs(this.moveValue * this.maxSpeed));
        Vector2 newVelocity = new Vector2(this.moveValue * this.maxSpeed, this.rigiBody.velocity.y);
        this.rigiBody.velocity = newVelocity;
        }
        */

        

        if (!this.cantMove && !this.doesSlide) {
            this.moveValue = Input.GetAxis("Horizontal");
            if (Mathf.Abs(this.moveValue) >= 0.1f) {
                this.moveValue = 1f * this.moveValue / Mathf.Abs(this.moveValue);
                this.isRunning = true;
            } else {
                this.moveValue = 0;
                this.isRunning = false;
            }
            if (this.moveValue > 0 && !facingRight) {
                this.Flip();
            } else if (moveValue < 0 && facingRight) {
                this.Flip();
            }
        }
        this.animator.SetFloat("speed", Mathf.Abs(this.moveValue * this.maxSpeed));
        Vector2 newVelocity = new Vector2(this.moveValue * this.maxSpeed, this.rigiBody.velocity.y);
        this.rigiBody.velocity = newVelocity;
    }

    /// <summary>
    /// dreht die transform-Component um
    /// </summary>
    private void Flip() {
        this.facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }

    /// <summary>
    /// Setzt den Spieler wieder auf den Boden. Zum Beenden der Fluganimation und um den DoppelJump zu resetten!
    /// </summary>
    /// <param name="value">na was wohl?</param>
    public void SetisGrounded(bool value) {
        this.isGrounded = value;
        if (value) {
            this.jumpCount = 0;
            this.jumpPower = originalJumpPower;
        }
        this.animator.SetBool("isGrounded", value);
    }

    public void DisableMovement() {
        this.cantMove = true;
    }

    public void EnableMovement() {
        this.cantMove = false;
    }

    private void EndSlide() {
        if (!doesSlide) {
            return;
        }
        this.doesSlide = false;
        this.animator.SetBool("isSliding", false);
        this.playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y * 2);
        this.playerCollider.offset = playerCollider.offset + new Vector2(0, 0.45f);
        this.maxSpeed /= 1.25f;
    }

    private void BeginSlide() {
        if (doesSlide) {
            return;
        }
        this.doesSlide = true;
        this.animator.SetBool("isSliding", true);
        this.playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y / 2);
        this.playerCollider.offset = playerCollider.offset - new Vector2(0, 0.45f);
        this.maxSpeed *= 1.25f;
    }

    private void DoJump() {
        //Reset Velocity
        this.rigiBody.velocity = Vector3.zero;
        this.jumpCount++;
        this.jumpPower *= 0.75f;
        this.rigiBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        this.EndSlide();
    }

    public bool DoesSlide() {
        return this.doesSlide;
    }


}
