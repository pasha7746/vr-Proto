using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRobotPiece : MonoBehaviour
{
    public GameObject piece;
    protected MeshRenderer render;
    protected Collider myCollider;
    protected Rigidbody myRigidbody;
    protected List<MeshRenderer> childRendrerList;
    public bool shouldRenderChildren;
    public event Action<float> OnPieceHit;
    [Range(0,100)]
    public float pieceDeffence;
    [Tooltip("Lower breaks first")]
    public int breakPriority;

   
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Projectile>()) return;
        CalculateDamageDealt(other.gameObject.GetComponent<Projectile>().baseDamage);
    }

    public void CalculateDamageDealt(float baseDamage)
    {
        float outcome = baseDamage * (pieceDeffence/100);
        if (OnPieceHit != null) OnPieceHit(outcome);

    }
}
