using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehaviour : MonoBehaviour
{
	public float speed = 15;

	void Start() {}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(-(speed * Time.deltaTime), 0, 0);

		// Get rid of the note when it reaches the other side
		if(transform.position.x < -26)
		{
			Destroy(gameObject);
		}
	}
}
