using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class EnemyProjectileTag : MonoBehaviour
{
    public float totalPossibleLifetime;
    private Coroutine deathRoutine;


    void Awake()
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
