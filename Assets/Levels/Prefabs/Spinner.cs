using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{

  
    public Vector3 rotationVector;

    public float speedScalar;
    private Transform myTransform;


    void Awake()
    {
        myTransform = GetComponent<Transform>();
    }
    
	// Update is called once per frame
	void Update ()
    {
		myTransform.Rotate(rotationVector,speedScalar);
	}
}
