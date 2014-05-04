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
	public enum salAnim { Idle, Walk, Jump, Sword }
	public enum facingDir {left, right}
	public enum directionBtn {none, isLeft, isRight}
	//private anim salAnim;
	public facingDir facingDirection;
	private int flipX = -1;
	public directionBtn theDirectionBtn;
	private salAnim salAnimation;
	//public directionBtn theDirectionBtn;

	//Attack Stuff
	public tk2dSpriteAnimator swordAttack;
	//private float salAttackX= 3.45f;

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
		salAnimation = salAnim.Idle;
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
			salAnimation = salAnim.Walk;
			salAnimator.Play("Walk");
		}

		// keyboard input
		if(horizontalInput < 0) 
		{ 
			if(facingDirection != facingDir.left)
			{
				if(theDirectionBtn != directionBtn.isLeft)
				{
					//flipX = -1;
					FlipX();
					theDirectionBtn = directionBtn.isLeft;
					facingDirection = facingDir.left;
				}
			}
		}
		
		else if (horizontalInput > 0) 
		{ 
			if(facingDirection != facingDir.right)
			{
				if(theDirectionBtn != directionBtn.isRight)
				{
					//flipX = Mathf.Abs(flipX);
					FlipX();
					theDirectionBtn = directionBtn.isRight;
					facingDirection = facingDir.right;
				}
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
			salAnimation = salAnim.Idle;
			salAnimator.Play("Idle");
		}
		
		if (touchingPlatform == false)
		{
			salAnimation = salAnim.Jump;
			salAnimator.Play("Jump");
		}
	}

	void FlipX()
	{
		Vector3 newSpriteScale;
		newSpriteScale = salAnimator.Sprite.scale;
		newSpriteScale = new Vector3 (newSpriteScale.x * flipX, newSpriteScale.y, newSpriteScale.z);
		salAnimator.Sprite.scale = newSpriteScale;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Input.GetKeyDown (KeyCode.K)) 
		{
			//tk2dAnimatedSprite clone = (tk2dAnimatedSprite)Instantiate(swordAttack, gameObject.transform.position + new Vector3(3.1f,.29f,0), Quaternion.identity);
			//clone.Play("Attack");
			//tk2dAnimatedSprite clone = tk2dAnimatedSprite.Instantiate(swordAttack, gameObject.transform.position + new Vector3(3.1f,.29f,0), Quaternion.identity) as tk2dAnimatedSprite;
			//clone.Play("Attack");

//			tk2dAnimatedSprite tempSword;
//			tk2dSpriteCollectionData spriteCollection = Resources.Load ("Sprites/Sal/SwordSwing/SalCollection Data", typeof(tk2dSpriteCollectionData)) as tk2dSpriteCollectionData;
//			tempSword = tk2dAnimatedSprite.AddComponent(gameObject,spriteCollection, "sword1");
//			tempSword.transform.position = gameObject.transform.position + new Vector3(3.1f,.29f,0);

			GameObject swordTemp = (GameObject)Instantiate(Resources.Load ("Sword", typeof(GameObject)));


			if(swordTemp != null)
			{
				print ("asdf2");
				tk2dAnimatedSprite swordSlash;
				swordSlash = (tk2dAnimatedSprite)swordTemp.GetComponent<tk2dAnimatedSprite>();

				if(swordSlash != null)
				{
					print ("asdF");
					//swordSlash.transform.position = gameObject.transform.position + new Vector3(3.1f,.29f,0);
				}
			}


			
		}
		
//		if (GameObject.FindGameObjectWithTag("SwordAttack") != null) 
//		{
//			GameObject.FindGameObjectWithTag("SwordAttack").transform.position = gameObject.transform.position + new Vector3(3.1f,.29f,0);	
//
//
//			if (salAnimation == salAnim.Walk) 
//			{
//				GameObject.FindGameObjectWithTag("SwordAttack").transform.position = gameObject.transform.position + new Vector3(salAttackX,.29f,0);
//			}
//		}



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
