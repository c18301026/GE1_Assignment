using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	// Variables that control the player's movement
	public float speed = 10;
	public float rotationSpeed = 100;
	public float horizontalInput, verticalInput;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		// Player input
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");

		// The camera view can rotate horizontally or move back & forth
		transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
		transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);

		// Respawn if you fall into the void
		if(transform.position.y < -50)
		{
			transform.position = new Vector3(0, 30, -9);
		}
	}
}
