using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotCenterHealth : MonoBehaviour
{
    public float robotHealth;
    public event Action OnDeath;

	// Use this for initialization
	void Start ()
    {
		GetComponentsInChildren<BaseRobotPiece>().ToList().ForEach((a) => { a.OnPieceHit += EventPieceIsHit; });
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventPieceIsHit(100);
        }
    }

    public void EventPieceIsHit(float damage)
    {
        robotHealth -= damage;
        if (robotHealth <= 0)
        {
            if (OnDeath != null) OnDeath();
        }
    }
}
