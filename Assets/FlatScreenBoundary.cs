using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatScreenBoundary : MonoBehaviour
{
   [System.Serializable]
    public struct PlayerSide
    {
        public Camera boundingCamera;
        public GameObject controller_Obj;
        public bool shouldVibrate;
        public bool isLeft;
        [HideInInspector]
        public Controller myController;
    }

    public PlayerSide leftSide;
    public PlayerSide rightSide;

    public event Action<bool> OnContOutOfBound_Inner; 




    [Header("Bounding Volume Inner")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;
    
   

	// Use this for initialization
	void Start ()
	{
	    leftSide.myController = leftSide.controller_Obj.GetComponent<Controller>();
	    rightSide.myController = rightSide.controller_Obj.GetComponent<Controller>();


    }

    // Update is called once per frame
    void Update ()
    {

        CheckBounds(leftSide);
        CheckBounds(rightSide);
       
    }

    public void CheckBounds(PlayerSide side)
    {
        if (side.controller_Obj.activeInHierarchy)
        {
            Vector3 point = side.boundingCamera.WorldToViewportPoint(side.controller_Obj.transform.position);

            if (!(point.z > zMin && point.z < zMax && point.x > xMin && point.x < xMax && point.y > yMin && point.y < yMax))
            {
                if (OnContOutOfBound_Inner != null) OnContOutOfBound_Inner(side.isLeft);
                if (side.shouldVibrate)
                {
                    side.myController.Vibrate();
                }
            }

        }
    }

}
