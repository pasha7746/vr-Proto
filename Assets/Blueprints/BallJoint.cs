using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallJoint : MonoBehaviour
{
    private SplitScreen mySplitScreen;


	// Use this for initialization
    void Awake()
    {
        mySplitScreen = FindObjectOfType<SplitScreen>();

    }

    void Start ()
    {
        mySplitScreen.OnSplitEnable += OnsplitScreen;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnsplitScreen()
    {

    }
}
