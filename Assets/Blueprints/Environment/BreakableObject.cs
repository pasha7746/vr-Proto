using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakableObject : MonoBehaviour
{
    public GameObject wholeModel;
    public GameObject brokenModel;
    public event Action OnObjectHit;
    private Rigidbody myRigidbody;
    private Collider myCollider;
    public float despawnTimer;
    public Vector2 blashForceRange;
    public Vector2 blastForceRadius;
    private List<Rigidbody> listOfShrapnelRigidbodies= new List<Rigidbody>();

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }

    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void  OnTriggerEnter(Collider other)
    {
        
        if(!other.GetComponent<Projectile>()) return;
        wholeModel.SetActive(false);
        brokenModel.SetActive(true);
        listOfShrapnelRigidbodies.AddRange(brokenModel.GetComponentsInChildren<Rigidbody>());
        listOfShrapnelRigidbodies.ForEach((a) => {a.AddExplosionForce(Random.Range(blashForceRange.x, blashForceRange.y),transform.position, Random.Range(blastForceRadius.x, blastForceRadius.y)); });
        if (myRigidbody)
        {
            myRigidbody.isKinematic = true;

        }
        if (OnObjectHit != null) OnObjectHit();
        myCollider.enabled = false;

        StartCoroutine(DespawnTimer());
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTimer);
        Destroy(gameObject);

        yield return null;
    }



}
