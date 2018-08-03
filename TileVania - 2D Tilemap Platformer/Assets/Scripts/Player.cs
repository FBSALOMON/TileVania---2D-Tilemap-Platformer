using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    //Config
    [SerializeField]
    public float runSpeed = 5f;
    [SerializeField]
    public float jumpPower = 5f;
    [SerializeField]
    public float climbSpeed = 5f;
    [SerializeField]
    public Vector2 deathKick;

    // State
    bool isAlive = true;

    //Cached component references
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private CapsuleCollider2D myBodyCollider;
    private BoxCollider2D myFeet;
    private float gravityScaleAtStart;

    //Message then methods
    void Start () {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody2D.gravityScale;
	}

	void Update ()
    {
        if (!isAlive) { return; }
        
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();


    }

    private void Run()
    {
        float controlThrown = Input.GetAxis("Horizontal") * runSpeed;
        Vector2 playerVelocity = new Vector2(controlThrown, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        bool PlayerIsBeingMoved = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", PlayerIsBeingMoved);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        bool isPlayerOnTheGround = (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")));
        if (CrossPlatformInputManager.GetButtonDown("Jump") && isPlayerOnTheGround)
        {
            Vector2 playerVelocity = new Vector2(0f, jumpPower);
            myRigidbody2D.velocity += playerVelocity;
        }
    }

    private void ClimbLadder()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myRigidbody2D.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            return; }

        float controlThrown = CrossPlatformInputManager.GetAxis("Vertical");            
        Vector2 climbVelocity = new Vector2(myRigidbody2D.velocity.x, controlThrown * climbSpeed);
        myRigidbody2D.velocity = climbVelocity;

        myRigidbody2D.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Die");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
