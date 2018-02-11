using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.Translate(Vector3.up* 10f);
            Vector3 scale = transform.localScale;
            scale *= 2;
            transform.localScale = scale;
        }
    }
}
