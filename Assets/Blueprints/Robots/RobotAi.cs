using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class RobotAi : MonoBehaviour
{
    private NavMeshAgent myNav;
    private BreakableObject myObjectBreaker;
    private Players playersPlaying;
    private AudioSource mySource;
    public AudioClip deathSound;
    private RobotCenterHealth myHealth;

    void Awake()
    {
        myNav = GetComponent<NavMeshAgent>();
        myObjectBreaker = GetComponent<BreakableObject>();
        playersPlaying = FindObjectOfType<Players>();
        mySource = GetComponent<AudioSource>();
        myHealth = GetComponent<RobotCenterHealth>();

    }

    // Use this for initialization
    void Start ()
    {
        if (myObjectBreaker != null) myObjectBreaker.OnObjectHit += Death;
        if (myHealth != null) myHealth.OnDeath += NavOff;
    }

    // Update is called once per frame
    void Update ()
    {
       if(!myNav.isOnNavMesh) return;
        myNav.SetDestination(playersPlaying.transform.position);
    }

    public void Death()
    {
        myNav.isStopped = true;
        mySource.clip = deathSound;
        mySource.loop = false;
        mySource.Play();

    }

    public void NavOff()
    {
        myNav.isStopped = true;
    }
}
