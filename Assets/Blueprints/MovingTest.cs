using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTest : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		transform.Translate(transform.forward* Input.GetAxisRaw("Vertical") * Time.deltaTime);
        transform.Translate(Vector3.left * Input.GetAxisRaw("Horizontal") * Time.deltaTime);

    }
}
