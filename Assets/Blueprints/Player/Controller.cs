using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool isLeft;

    //movement
    [HideInInspector]
    public Vector3 velocity;
    private Vector3 posA;
    private Vector3 posB;
    private Vector3 rotA;
    private Vector3 rotB;
    [HideInInspector]
    public Vector3 rotVelocity;

    //side bumpers
    public float doubleTapDelay;
    private Coroutine doubleTapToDropRoutine;
    private int sideButtonState;
    private bool doubleTaped;
    private bool isTapedIn;
    public event Action OnTriggerButtonLeft, OnTriggerButtonRight;
    public event Action OnSideTapLeft, OnSideTapRight;
    public event Action OnSideDoubeTapLeft, OnSideDoubleTapRight;

    [HideInInspector] public Vector2 leftTrackPad, rightTrackPad;
    public event Action<Vector2> OnPadLeft;
    public event Action<Vector2> OnPadRight;


    private Coroutine vibrateRoutine;
    private int controllerIndex;
    private SteamVR_TrackedObject myTracker;


    // Use this for initialization
    void Start ()
	{
	    myTracker = GetComponent<SteamVR_TrackedObject>();

        controllerIndex = myTracker.index.GetHashCode();

        posA = transform.position;
	    rotA = transform.rotation.eulerAngles;
	}

    // Update is called once per frame
    void Update ()
    {
        CalculateRotationVelocity();
        CalculateMovementVelocity();

        if (isLeft)
        {
            LeftInput();

        }
        else
        {
            RightInput();
        }

       

    }

    public void LeftInput()
    {
        if (Input.GetButtonDown("TriggerButLeft"))
        {
            if (OnTriggerButtonLeft != null) OnTriggerButtonLeft();
        }
        sideButtonState = (int)Input.GetAxis("SideButtonLeft");
        if (sideButtonState == 1)
        {
            if (!isTapedIn)
            {
                if (doubleTapToDropRoutine == null)
                {
                    doubleTapToDropRoutine = StartCoroutine(DoubleTapToDropCounter());
                }
                else if (doubleTaped)
                {
                    doubleTaped = false;
                    StopCoroutine(doubleTapToDropRoutine);
                    doubleTapToDropRoutine = null;
                    if (OnSideDoubeTapLeft != null) OnSideDoubeTapLeft();
                }
                
                    if (OnSideTapLeft != null) OnSideTapLeft();

                
            }
            isTapedIn = true;
        }
        else
        {
            if (isTapedIn)
            {
                if (doubleTapToDropRoutine != null)
                {
                    doubleTaped = true;
                }
                else
                {
                    doubleTaped = false;
                }
            }
            isTapedIn = false;

        }

        leftTrackPad.x = Input.GetAxis("TPHL"); //track pad horizontal left
        leftTrackPad.y = Input.GetAxis("TPVL");

        if (leftTrackPad != Vector2.zero)
        {
            if (OnPadLeft != null) OnPadLeft(leftTrackPad);
        }


    }

    public void RightInput()
    {

        if (Input.GetButtonDown("TriggerButRight"))
        {
            if (OnTriggerButtonRight != null) OnTriggerButtonRight();
        }
        sideButtonState = (int)Input.GetAxis("SideButtonRight");

        if (sideButtonState == 1)
        {
            if (!isTapedIn)
            {
                if (doubleTapToDropRoutine == null)
                {
                    doubleTapToDropRoutine = StartCoroutine(DoubleTapToDropCounter());
                }
                else if (doubleTaped)
                {
                    doubleTaped = false;
                    StopCoroutine(doubleTapToDropRoutine);
                    doubleTapToDropRoutine = null;
                    if (OnSideDoubleTapRight != null) OnSideDoubleTapRight();
                }
               
                if (OnSideTapRight != null) OnSideTapRight();
                
            }
            isTapedIn = true;
        }
        else
        {
            if (isTapedIn)
            {
                if (doubleTapToDropRoutine != null)
                {
                    doubleTaped = true;
                }
                else
                {
                    doubleTaped = false;
                }
            }
            isTapedIn = false;

        }
        rightTrackPad.x = Input.GetAxis("TPHR"); //track pad horizontal left
        rightTrackPad.y = Input.GetAxis("TPVR");

        if (rightTrackPad != Vector2.zero)
        {
            if (OnPadRight != null) OnPadRight(rightTrackPad);
        }
    }



    public void CalculateMovementVelocity()
    {
        posB = transform.position;
        velocity = -((posA - posB) / Time.deltaTime);
        posA = posB;
    }

    public void CalculateRotationVelocity()
    {
        rotB = transform.rotation.eulerAngles;
        rotVelocity = -((rotA - rotB) / Time.deltaTime);
        


        rotA = rotB;
    }
    public IEnumerator DoubleTapToDropCounter()
    {

        yield return new WaitForSeconds(doubleTapDelay);
        doubleTapToDropRoutine = null;
        yield return null;
    }

    public void Vibrate()
    {
        
            if (vibrateRoutine != null)
            {
                StopCoroutine(vibrateRoutine);

            }
            vibrateRoutine = StartCoroutine(VibrateController(2000, 0.1f));
        
    }
    public IEnumerator VibrateController(ushort stenght, float duration)
    {
        float time = 0;
        while (true)
        {
            if (time >= duration) break;
            time += Time.deltaTime;
            SteamVR_Controller.Input(controllerIndex).TriggerHapticPulse(stenght);
            yield return null;
        }
        vibrateRoutine = null;
        yield return null;
    }
}
