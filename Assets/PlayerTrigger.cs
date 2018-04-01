using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public event Action<float> OnDamage;
    private SteamVR_Camera myCenter;

	// Use this for initialization
	void Start ()
	{
	    myCenter = FindObjectOfType<SteamVR_Camera>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    void LateUpdate()
    {
        transform.position = myCenter.transform.position;
    }

    public void SendHitEvent(float damage)
    {
        if (OnDamage != null) OnDamage(damage);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!other.GetComponent<EnemyProjectileTag>()) return;

        SendHitEvent(-other.GetComponent<EnemyProjectileTag>().damage);


    }
}
