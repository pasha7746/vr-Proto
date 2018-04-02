using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class EnemyProjectileTag : MonoBehaviour
{
    public float totalPossibleLifetime;
    private Coroutine deathRoutine;
    [HideInInspector]
    public float damage;


    public void StartLifeTimer()
    {
        StartCoroutine(LifeTimer());
    }

    public IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(totalPossibleLifetime);
        SelfDestruct();

        yield return null;
    }

   

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }


}
