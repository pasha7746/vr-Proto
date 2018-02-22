using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Button : MonoBehaviour
{
    private CapsuleCollider myButtonCollider;
    private Image myTimerBar;
    private bool isBeingTriggered;
    public float holdLenght;
    public float timerDecayRate;
    private float timerValue;
    public event Action OnHoldComplete;
    public event Action OnDecayComplete;
    private Coroutine timerCoroutine;

    private void Awake()
    {
        myButtonCollider = GetComponentInChildren<CapsuleCollider>();
        myTimerBar = GetComponentInChildren<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        OnHoldComplete += EventHoldComplete;
        OnDecayComplete += EventDecayComplete;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print("bhrvb");
        if(!other.GetComponent<Controller>()) return;
        isBeingTriggered = true;
        timerCoroutine = StartCoroutine(Timer());


    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Controller>()) return;
        isBeingTriggered = false;
    }

    public IEnumerator Timer()
    {
        
        while (true)
        {
            if (isBeingTriggered)
            {
                timerValue += Time.deltaTime;
                if (timerValue >= holdLenght)
                {
                    if (OnHoldComplete != null) OnHoldComplete();
                }
                else
                {
                    myTimerBar.fillAmount = timerValue / holdLenght;
                }
            }
            else
            {
                timerValue -= Time.deltaTime * timerDecayRate;
                if (timerValue < 0)
                {
                    timerValue = 0;
                    myTimerBar.fillAmount = timerValue / holdLenght;
                    break;
                }
                myTimerBar.fillAmount = timerValue / holdLenght;
            }

            yield return null;
        }

        yield return null;
    }

    public void EventHoldComplete()
    {
        print("Comp");
    }

    public void EventDecayComplete()
    {
        print("Dec");
    }

}
