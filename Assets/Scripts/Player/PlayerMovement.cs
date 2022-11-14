/*
Purpose: Control player movement
Author:  Ilyes Benachi
Date:    Unknown
Notes:   This script takes in keyboard input and applies a force to the player game object depending on what is 
	     inputted.
Changes: 13/07/2021 Added this comment header as it was previously ommited - Rhys Myring
		 13/07/2021 Converted the colliders to 3D so they can interact with the 3D level. - Rhys Myring
		 08/08/2021 Commented out jump code as it doesn't work correctly and is now implemented elsewhere
					(left commented so it can be viewed while marking) - Rhys Myring
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rb;

	//X,Y axis Movement Variables
	Vector2 move;
	public int speed = 200;

	//Jump Variables
	public float jumpVelocity = 10f;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2;
	public float normalJumpMultiplier = 1;
	public float groundedSkin;
	public LayerMask groundMask;

	public bool jumpRequest;
	public bool grounded;

	Vector2 playerSize;
	Vector2 boxSize;

	private void Awake()
	{
		playerSize = GetComponent<BoxCollider>().size;
		playerSize.Scale(gameObject.transform.localScale);
		boxSize = new Vector2(playerSize.x, groundedSkin);
		boxSize.Scale(gameObject.transform.localScale);
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		//Movement X,Y axis Input
		move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//Jump Input
		//if (Input.GetButtonDown("Jump") && grounded)
		//{
			//jumpRequest = true;
		//}
	}

	void FixedUpdate()
	{
		//Jump
		//if (jumpRequest)
		//{
		//	rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
		//	jumpRequest = false;
		//	grounded = false;
		//}
		//else
		//{
		//	Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
		//	grounded = (Physics.OverlapBox(boxCenter, boxSize, Quaternion.identity, groundMask) != null);
		//}

		//Jumping Higher
		//if (rb.velocity.y < 0)
		//{
			//rb.gravityScale = fallMultiplier;
		//}
		//else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
		//{
			//rb.gravityScale = lowJumpMultiplier;
		//}
		//else
		//{
			//rb.gravityScale = normalJumpMultiplier;
		//}

		//Movement X,Y axis
		rb.velocity = new Vector2(move.x * speed * Time.deltaTime, rb.velocity.y);
	}
}
