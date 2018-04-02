using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotBreakableCenter : MonoBehaviour
{
    private List<BaseRobotPiece> breakablePieces;
    public float robotPieceDespawnTime;
    private bool isBreakingUp;
    public float finalBreakupDellay;

    void Awake()
    {
        breakablePieces = GetComponentsInChildren<BaseRobotPiece>().ToList();
    }


    // Use this for initialization
	void Start ()
    {
		breakablePieces.ForEach((A) => { A.GetComponent<RobotPieceBreak>().OnPieceBreak += ObjectBrokeOf; });
        GetComponent<RobotCenterHealth>().OnDeath += BreakWholeBody;
       
    }
	
	// Update is called once per frame
	void Update ()
    {
       
    }

    public void ObjectBrokeOf(BaseRobotPiece pieceOwner)
    {
        GameObject brokenPiece = pieceOwner.piece;
        StartCoroutine(DespawnPiece(brokenPiece));
        if(!isBreakingUp)
        {
            breakablePieces.Remove(pieceOwner);

        }
        Destroy(pieceOwner);
    }

    public IEnumerator DespawnPiece(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(robotPieceDespawnTime);
        Destroy(objectToDestroy);
        yield return null;

    }

    public void BreakWholeBody()
    {
        isBreakingUp = true;
        breakablePieces.RemoveAll((A) => A == null);
        breakablePieces.Sort((a, b) => a.breakPriority.CompareTo(b.breakPriority));
        for (int i = 0; i < breakablePieces.Count; i++)
        {
            breakablePieces[i].GetComponent<RobotPieceBreak>().BreakPice();
        }
        StartCoroutine(CountdownFinalDespawn());
        //breakablePieces.ForEach((a)=>a.GetComponent<RobotPieceBreak>().BreakPice());

    }

    public IEnumerator CountdownFinalDespawn()
    {
        yield return new WaitForSeconds(finalBreakupDellay);
        Destroy(gameObject);
        yield return null;
    }
}
