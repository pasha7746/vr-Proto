using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectHolding : MonoBehaviour
{
    private bool isHoldingObject;
    private GameObject gripedObject;
    private bool isLeft;
    public List<GameObject> listOfObjectsInRange= new List<GameObject>();
    [Header("Debug")] public Material red, green;
    public float detectionRadius;
    public bool canPickupTriggerObjects;
    private Rigidbody myRigidbody;
    private Controller myController;
    public float throwPowerMultiplier;
    [Range(0.00001f,10000)]
    public float rotationVelocityScalar;
    public event Action<GameObject> OnItemPickup;
    public event Action<GameObject> OnItemDrop;
    private bool itemNeedsDoubleTapToDrop;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myController = GetComponentInParent<Controller>();
    }

    // Use this for initialization
	void Start ()
	{
	    if (myController.isLeft)
	    {
	        myController.OnTriggerButtonLeft += AttemptToPickupObject;
	        myController.OnSideTapLeft += SingleTapCheck;
	        myController.OnSideDoubeTapLeft += DoubleTapCheck;
	    }
	    else
	    {
	        myController.OnTriggerButtonRight += AttemptToPickupObject;
	        myController.OnSideTapRight += SingleTapCheck;
	        myController.OnSideDoubleTapRight += DoubleTapCheck;
	    }


	}
	
	
    public void DoubleTapCheck()
    {
        if(!gripedObject) return;
        if (itemNeedsDoubleTapToDrop)
        {
            AttemptToDropObject();
        }
        
    }

    public void SingleTapCheck()
    {
        if (!gripedObject) return;
        if (!itemNeedsDoubleTapToDrop)
        {
            AttemptToDropObject();
        }
    }

    public void AttemptToDropObject()
    {
        if (!gripedObject) return;
        gripedObject.transform.parent = null;
        Rigidbody objectRgBody = gripedObject.GetComponent<Rigidbody>();
        if (objectRgBody)
        {
            objectRgBody.isKinematic = false;
            objectRgBody.velocity = (myController.velocity *throwPowerMultiplier)/objectRgBody.mass;
            objectRgBody.angularVelocity = myController.rotVelocity/rotationVelocityScalar;
        }
        isHoldingObject = false;
        gripedObject = null;
    }

    public void AttemptToPickupObject()
    {
        if(isHoldingObject) return;

        List<Collider> listOfOverlapingColliders = Physics.OverlapSphere(transform.position, detectionRadius).ToList();
        listOfOverlapingColliders.RemoveAll((a) => !a.GetComponent<Interactable>());
        if (listOfOverlapingColliders.Count == 0) return;

        if (!canPickupTriggerObjects)
        {
            listOfOverlapingColliders.RemoveAll((a) => a.isTrigger);
        }
        SelectsObject(listOfOverlapingColliders.ConvertAll((a)=>a.gameObject));


    }

    public void SelectsObject(List<GameObject> interactablesList)
    {
        Rigidbody objectRgBody = interactablesList[0].GetComponent<Rigidbody>();
        if (objectRgBody)
        {
            objectRgBody.isKinematic = true;
        }
        interactablesList[0].transform.parent = transform;
        gripedObject = interactablesList[0];
        isHoldingObject = true;
        itemNeedsDoubleTapToDrop= gripedObject.GetComponent<Interactable>().itemNeedsDoubleTapToDrop;
        gripedObject.GetComponent<Interactable>().CallPickUp();
    }

    

}
