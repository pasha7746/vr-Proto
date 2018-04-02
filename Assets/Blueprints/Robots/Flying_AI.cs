using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlightPathFinding))]

public class Flying_AI : MonoBehaviour
{
    private EnemyGuns myGuns;
    private FlightPathFinding myMovement;
    public Vector2 droidHoverLenghtRange;
    private Coroutine waitCoroutine;

    public bool moveLocked; //debug only

    void Awake()
    {
        myGuns = GetComponent<EnemyGuns>();
        myMovement = GetComponent<FlightPathFinding>();

    }

    // Use this for initialization
	void Start ()
	{
	    myMovement.OnRouteComplete += myMovement.MoveToRandomPointOnMap;
	    myMovement.OnGridPointHit += Wait;
        if(moveLocked) return;
		myMovement.MoveToCombatArea();

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    public void Wait()
    {
       

        if (waitCoroutine == null)
        {
            waitCoroutine = StartCoroutine(WaitCoroutine());

        }
        
    }

    public IEnumerator WaitCoroutine()
    {
        myMovement.TurnTowardsPlayer(myGuns.targetPlayers.transform.position);
        myGuns.AimGuns(myGuns.targetPlayers.transform.position);
        yield return new WaitForSeconds(Random.Range(droidHoverLenghtRange.x, droidHoverLenghtRange.y));
       
        myMovement.MoveToRandomPointOnMap();
        waitCoroutine = null;
        yield return null;
    }


}
