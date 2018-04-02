using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public bool healthRegenerates;
    public float regenerationRate;
    
    public Coroutine regenRoutine;
    

    public event Action<float> OnHealthChange;
    public event Action OnHealthZero;

    private PlayerTrigger myPlayerTrigger;


    void Awake()
    {
        OnHealthZero += () =>
        {
            if (regenRoutine != null)
            {
                StopCoroutine(regenRoutine);
            }
        };

        if (healthRegenerates)
        {
            StartRegenRoutine();
        }

        myPlayerTrigger = GetComponentInChildren<PlayerTrigger>();
    }

    void Start()
    {
        myPlayerTrigger.OnDamage += ChangeHealth;
        if (OnHealthChange != null) OnHealthChange(health);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeHealth(10);
        }
    }

    public void ChangeHealth(float change)
    {
        if (health > 0)
        {
            health += change;
          
        }
        if(health <= 0)
        {
            if (OnHealthZero != null) OnHealthZero();
        }
        else if(health>maxHealth)
        {
            health = maxHealth;
        }

        if (OnHealthChange != null) OnHealthChange(health);
    }

    public void HealEffect(float rate, float healthAdded)
    {
        StartCoroutine(HealEffectTimer(rate, healthAdded));
    }

    public IEnumerator HealEffectTimer(float rate, float healthAdded)
    {
        float duration = healthAdded / rate;
        regenerationRate += rate;
        while (true)
        {
            duration -= Time.deltaTime;

            if (duration <= 0)
            {
                break;
            }
            //yield return new WaitForEndOfFrame();
            yield return null;
        }

        regenerationRate -= rate;

        yield return null;
    }

    public void StartRegenRoutine()
    {
        regenRoutine = StartCoroutine(HealRoutine());
    }

    public IEnumerator HealRoutine()
    {
        float healthPiece = 0;
        while (true)
        {
            healthPiece = Time.deltaTime * regenerationRate;
            
            ChangeHealth(healthPiece);
            
            yield return null;
        }

        yield return null;
    }

}
