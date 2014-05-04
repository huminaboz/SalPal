using UnityEngine;
using System.Collections;

public class PlayerCode : MonoBehaviour {

	//Movement Stuff
    private float horizontalInput, verticalInput;
    public float velocity;
	//Jumping
	private bool touchingPlatform;
	public Vector3 jumpVelocity;
	
	//Life Stuff
    private bool isActive;
	private int blueberries;

	//Spawn Stuff
    private Vector3 startPosition;

	//Animation Stuff
	private tk2dSpriteAnimator salAnimator;
	public enum anim { Idle, Walk, Jump, Sword }
	public enum facingDir {left, right}
	public enum directionBtn {none, isLeft, isRight}
	private anim salAnim;
	public facingDir facingDirection;
	public directionBtn theDirectionBtn;
	//public directionBtn theDirectionBtn;

    // Use this for initialization
    void Start()
    {
		//Animation
		salAnimator = GetComponent<tk2dSpriteAnimator>();
		
		//Gamestates
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        startPosition = transform.localPosition;

		//Freeze the game until you start moving
        isActive = false;
        //gameObject.SetActive(false);
    }

    private void GameStart()
    {
		blueberries = 3;
		print ("Blueberries: " + blueberries);
		transform.localPosition = startPosition;
        rigidbody2D.isKinematic = false;
        isActive = true;
        enabled = true;
		salAnim = anim.Idle;
		facingDirection = facingDir.right;
		theDirectionBtn = directionBtn.none;
	}

    private void GameOver()
    {
		print ("Out of Blueberries! GameOver.");
		print ("Press A or D to Start");
        rigidbody2D.isKinematic = true;
        enabled = false;
    }

    //Testing the input manager - I likes it
    public virtual void UpdateMovement3()
    {
        //The input slowly climbs to 1 or decelerates to 0 when you press or let go of the button
        //Multiplying by this number will give a smooth start and stop
        horizontalInput = Input.GetAxis("Horizontal");


        //verticalInput = Input.GetAxis("Vertical");
		

        //if (Mathf.Abs(horizontalInput) > 0.0)
        //{
            transform.position += Vector3.right * horizontalInput * Time.deltaTime * velocity;
        //}

        //if (Mathf.Abs(verticalInput) > 0.0)
        //{
            //transform.position += Vector3.up * verticalInput * Time.deltaTime * velocity;
        //}

                if (touchingPlatform && Input.GetButtonDown("Jump"))
                {
                    //Then it may jump
                    rigidbody2D.velocity = jumpVelocity;
                    touchingPlatform = false;
                }
    }

	void UpdateFacingDirection()
	{
		if (horizontalInput < 0 || horizontalInput > 0) 
		{
			salAnim = anim.Walk;
			salAnimator.Play("Walk");
		}

		// keyboard input
		if(horizontalInput < 0) 
		{ 
			if(theDirectionBtn != directionBtn.isLeft)
			{
				theDirectionBtn = directionBtn.isLeft;
				facingDirection = facingDir.left;
			}
		}
		
		else if (horizontalInput > 0) 
		{ 
			if(theDirectionBtn != directionBtn.isRight)
			{
				theDirectionBtn = directionBtn.isRight;
				facingDirection = facingDir.right;
			}
		}

		else
		{
			theDirectionBtn = directionBtn.none;
		}

		//print ("DirectionBtn being pressed: " + theDirectionBtn);
		//print ("Facing: " + facingDirection);
	}

	void UpdateAnimation()
	{
		if(horizontalInput == 0) 
		{
			salAnim = anim.Idle;
			salAnimator.Play("Idle");
		}
		
		if (touchingPlatform == false)
		{
			salAnim = anim.Jump;
			salAnimator.Play("Jump");
		}
	}

	void FlipX()
	{
		//transform.localScale = Vector3 (1, 1, 1);
		//Vector3 currentScale;
		//currentScale = transform.localScale * Vector3 (-1, 1, 1);
	  //	transform.localScale = currentScale;
		//gameObject.transform.localScale += Vector3(0.1,0,0);
		//salAnimator.gameObject.transform.localScale *= Vector3 (-1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () 
    {

		if (Input.GetKeyDown (KeyCode.K)) {
		//Make the salamander turn around
			//FlipX();
		}

        if (isActive)
        {
            UpdateMovement3();
			UpdateFacingDirection();
			UpdateAnimation();
        }

		if (blueberries < 0) 
		{
			GameEventManager.TriggerGameOver();
			GameStart();
		}
	}
	
	//Detect when a platform is being touched or not
    void OnCollisionEnter2D(Collision2D Other)
    {
        touchingPlatform = true;
		//Stop jumping

		if (Other.gameObject.GetComponent( typeof(MushroomScript)) != null)
		{
			print ("You hit a mushroom!");
			blueberries -= 1;
			print ("Blueberries: " + blueberries);
		}
		
		else if (Other.gameObject.GetComponent( typeof(BlueberryScript)) != null)
		{
			print ("Bloop! Yum!");
			blueberries += 1;
			print ("Blueberries: " + blueberries);
			BlueberryScript BlueberryScriptInstance = (BlueberryScript) Other.gameObject.GetComponent (typeof(BlueberryScript) );
			BlueberryScriptInstance.DestroyMe();
		}
		//print ("Collision Enter");
    }

    void OnCollisionExit2D()
    {
        touchingPlatform = false;
    }

}  //Class
