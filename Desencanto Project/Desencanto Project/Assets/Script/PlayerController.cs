using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float moveSpeed;
    private float moveSpeedStore;
    
    public float speedMultiplier;

    public float speedIncreaseMilestone;
    private float speedIncreaseMilestoneStore;

    private float speedMilestoneCount;
    private float speedMilestoneCountStore;
	
    public float jumpForce;
    
    public float jumpTime;
	public float jumpTimeCounter;

    private bool stoppedJumping;
    private bool canDoubleJump;
	
    private Rigidbody2D myRigidbody;
    
    public bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;      
			
	//private Collider2D myCollider;
	
    public GameManager theGameManager;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //myCollider = GetComponent<Collider2D>();
        jumpTimeCounter = jumpTime;
        speedMilestoneCount = speedIncreaseMilestone;
        moveSpeedStore = moveSpeed;
        speedMilestoneCountStore = speedMilestoneCount;
        speedIncreaseMilestoneStore = speedIncreaseMilestone;

        stoppedJumping = true;


    }

    // Update is called once per frame
    void Update()
    {
    	grounded = Physics2D.OverlapCircle (groundCheck.position,groundCheckRadius, whatIsGround);

        if(transform.position.x > speedMilestoneCount){
            speedMilestoneCount += speedIncreaseMilestone;
            moveSpeed = moveSpeed*speedMultiplier;
        }
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            if(grounded){
                    myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, jumpForce);
                    stoppedJumping = false;
            }
            if(!grounded && canDoubleJump)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                jumpTimeCounter = jumpTime;
                stoppedJumping = false;
                canDoubleJump = false;
            }
        }
        if((Input.GetKey(KeyCode.Space)|| Input.GetMouseButton(0)) && !stoppedJumping){
            if(jumpTimeCounter>0){
                myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)|| Input.GetMouseButtonUp(0)){
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
        if(grounded){
            jumpTimeCounter=jumpTime;
            canDoubleJump = true;
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="killbox"){
            theGameManager.RestartGame();
            moveSpeed = moveSpeedStore;
            speedMilestoneCount = speedMilestoneCountStore;
            speedIncreaseMilestone = speedIncreaseMilestoneStore;
        }
    }
}
