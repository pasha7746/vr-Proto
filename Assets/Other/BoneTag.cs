using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneTag : MonoBehaviour
{
    public Transform myController;
    private Rigidbody myRigidbody;
    private Vector3 lastPos=Vector3.zero;
    public float speedScale;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        
    }


    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	   myRigidbody.AddForce((lastPos-transform.position)*speedScale);
	    lastPos = myController.transform.position;
	}
}
