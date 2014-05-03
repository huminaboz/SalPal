using UnityEngine;
using System.Collections;

public class PlayerCode : MonoBehaviour {

    private float horizontalInput, verticalInput;
    [HideInInspector]public Vector3 characterVelocityHoriz = new Vector3(20, 0, 0);
    [HideInInspector]public Vector3 characterVelocityVert = new Vector3(0, 20, 0);
    public float velocity;

    private bool isActive;
	private float butts2;

    private Vector3 startPosition;
    public float gameOverY = -10;

    //Jumping
    private bool touchingPlatform;
    public Vector3 jumpVelocity;

    // Use this for initialization
    void Start()
    {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        startPosition = transform.localPosition;
        isActive = false;
        //gameObject.SetActive(false);
    }

    private void GameStart()
    {
        transform.localPosition = startPosition;
        rigidbody.isKinematic = false;
        isActive = true;
        //gameObject.SetActive(true);
        enabled = true;
    }

    private void GameOver()
    {
        rigidbody.isKinematic = true;
        enabled = false;
    }

    //A slightly more old-fashioned version - doesn't use the input manager
    public virtual void UpdateMovement2()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += characterVelocityHoriz * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -characterVelocityHoriz * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += characterVelocityVert * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -characterVelocityVert * Time.deltaTime;
        }
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
                    rigidbody.velocity = jumpVelocity;
                    touchingPlatform = false;
                }
    }

	// Update is called once per frame
	void Update () 
    {
            if (isActive)
            {
                UpdateMovement3();
                //UpdateMovement2();
            }

            if (transform.localPosition.y < gameOverY)
            {
                GameEventManager.TriggerGameOver();
                //GameStart();
            }
	}

    //Detect when a platform is being touched or not
    void OnCollisionEnter()
    {
        touchingPlatform = true;
    }

    void OnCollisionExit()
    {
        touchingPlatform = false;
    }

}  //Class
