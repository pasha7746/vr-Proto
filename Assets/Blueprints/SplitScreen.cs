using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR;

public class SplitScreen : MonoBehaviour
{
    /// <summary>
    /// In charge of switching from VR to split screen...
    /// 
    /// </summary>
    public event Action OnSplitEnable;
    public event Action OnSplitDisable;
    private FlatScreenBoundary myScreenBoundary;

	// Use this for initialization
	void Start ()
	{
	    myScreenBoundary = GetComponentInParent<FlatScreenBoundary>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.A))
	    {
	        VRSettings.showDeviceView = false;
	        if (OnSplitEnable != null) OnSplitEnable();
	        myScreenBoundary.leftSide.shouldVibrate = true;
	        myScreenBoundary.rightSide.shouldVibrate = true;

        }
        if (Input.GetKeyDown(KeyCode.S))
	    {
	        VRSettings.showDeviceView = true;
	        if (OnSplitDisable != null) OnSplitDisable();
	        myScreenBoundary.leftSide.shouldVibrate = false;
	        myScreenBoundary.rightSide.shouldVibrate = false;
        }

	    
    }

   
}
