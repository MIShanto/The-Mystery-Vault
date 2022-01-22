// This script controls the player's movement and physics within the game

using UnityEngine;
using System.Collections;

public class NormalPlayerMovement : MonoBehaviour
{
	public bool drawDebugRaycasts = true;	//Should the environment checks be visualized

	[Header("Movement Properties")]
	public float speed = 8f;				//Player speed
	public float coyoteDuration = .05f;		//How long the player can jump after falling
	public float maxFallSpeed = -25f;		//Max speed player can fall

	[Header("Jump Properties")]
	public float jumpForce = 6.3f;			//Initial force of jump
	public float jumpHoldForce = 1.9f;		//Incremental force when jump is held
	public float jumpHoldDuration = .1f;	//How long the jump key can be held

	[Header("Environment Check Properties")]
	public float footOffset = .4f;			//X Offset of feet raycast
	public float groundDistance = .2f;		//Distance player is considered to be on the ground
	public LayerMask groundLayer;			//Layer of the ground

	[Header ("Status Flags")]
	public bool isOnGround;					//Is the player on the ground?
	public bool isJumping;					//Is player jumping?

	PlayerInput input;						//The current inputs for the player
	BoxCollider2D bodyCollider;				//The collider component
	Rigidbody2D rigidBody;                  //The rigidbody component
	Animator playerAnimator;
	
	float jumpTime;							//Variable to hold jump duration
	float coyoteTime;						//Variable to hold coyote duration
	float playerHeight;						//Height of the player

	float originalXScale;					//Original scale on X axis
	int direction = 1;						//Direction player is facing

	Vector2 colliderStandSize;				//Size of the standing collider
	Vector2 colliderStandOffset;			//Offset of the standing collider

	const float smallAmount = .05f;			//A small amount used for hanging position


	void Start ()
	{
		//Get a reference to the required components
		input = GetComponent<PlayerInput>();
		rigidBody = GetComponent<Rigidbody2D>();
		bodyCollider = GetComponent<BoxCollider2D>();
		playerAnimator = GetComponent<Animator>();

		//Record the original x scale of the player
		originalXScale = transform.localScale.x;

		//Record the player's height from the collider
		playerHeight = bodyCollider.size.y;

		//Record initial collider size and offset
		colliderStandSize = bodyCollider.size;
		colliderStandOffset = bodyCollider.offset;
	}

	void FixedUpdate()
	{
		//Check the environment to determine status
		PhysicsCheck();

		//Process ground and air movements
		GroundMovement();		
		MidAirMovement();
	}

	void PhysicsCheck()
	{
		//Start by assuming the player isn't on the ground and the head isn't blocked
		isOnGround = false;

		//Cast rays for the left and right foot
		RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

		//If either ray hit the ground, the player is on the ground
		if (leftCheck || rightCheck)
			isOnGround = true;



		//Determine the direction of the wall grab attempt
		Vector2 grabDir = new Vector2(direction, 0f);
	}

	void GroundMovement()
	{

		if(input.horizontal !=0)
		{
			if(isOnGround)
				playerAnimator.Play("walk");
			else
				playerAnimator.Play("none");
		}
		else
		{
			if(isOnGround)
				playerAnimator.Play("idle");
			else
				playerAnimator.Play("none");
		}

		//Calculate the desired velocity based on inputs
		float xVelocity = speed * input.horizontal;

		//If the sign of the velocity and direction don't match, flip the character
		if (xVelocity * direction < 0f)
			FlipCharacterDirection();

		//Apply the desired velocity 
		rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

		//If the player is on the ground, extend the coyote time window
		if (isOnGround)
			coyoteTime = Time.time + coyoteDuration;
	}

	void MidAirMovement()
	{

		//If the jump key is pressed AND the player isn't already jumping AND EITHER
		//the player is on the ground or within the coyote time window...
		if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
		{
			audio_Manager.instance.Play("jump");

			StartCoroutine(JumpSqueeze(1.15f * direction, 0.85f, 0.1f));

			//...The player is no longer on the groud and is jumping...
			isOnGround = false;
			isJumping = true;

			//...record the time the player will stop being able to boost their jump...
			jumpTime = Time.time + jumpHoldDuration;

			//...add the jump force to the rigidbody...
			rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

			//...and tell the Audio Manager to play the jump audio
			//AudioManager.PlayJumpAudio();
		}
		//Otherwise, if currently within the jump time window...
		else if (isJumping)
		{
			//...and the jump button is held, apply an incremental force to the rigidbody...
			if (input.jumpHeld)
				rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

			//...and if jump time is past, set isJumping to false
			if (jumpTime <= Time.time)
				isJumping = false;
		}

		//If player is falling to fast, reduce the Y velocity to the max
		if (rigidBody.velocity.y < maxFallSpeed)
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
	}

	IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
	{
		Vector3 originalSize = transform.localScale;
		Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
		float t = 0f;
		while (t <= 1.0)
		{
			t += Time.deltaTime / seconds;
			transform.localScale = Vector3.Lerp(originalSize, newSize, t);
			yield return null;
		}
		t = 0f;
		while (t <= 1.0)
		{
			t += Time.deltaTime / seconds;
			transform.localScale = Vector3.Lerp(newSize, originalSize, t);
			yield return null;
		}

	}

	void FlipCharacterDirection()
	{
		//Turn the character by flipping the direction
		direction *= -1;

		//Record the current scale
		Vector3 scale = transform.localScale;

		//Set the X scale to be the original times the direction
		scale.x = originalXScale * direction;

		//Apply the new scale
		transform.localScale = scale;
	}


	//These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
	//functionality
	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
	{
		//Call the overloaded Raycast() method using the ground layermask and return 
		//the results
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
	{
		//Record the player's position
		Vector2 pos = transform.position;

		//Send out the desired raycasr and record the result
		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

		//If we want to show debug raycasts in the scene...
		if (drawDebugRaycasts)
		{
			//...determine the color based on if the raycast hit...
			Color color = hit ? Color.red : Color.green;
			//...and draw the ray in the scene view
			Debug.DrawRay(pos + offset, rayDirection * length, color);
		}

		//Return the results of the raycast
		return hit;
	}
}
