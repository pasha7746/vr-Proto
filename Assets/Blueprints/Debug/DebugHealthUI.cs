using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugHealthUI : MonoBehaviour
{
    private PlayerHealth myHealth;
    private Image myImage;

	// Use this for initialization
	void Start ()
	{
	    myHealth = GetComponentInParent<PlayerHealth>();
	    myImage = GetComponent<Image>();
	    myHealth.OnHealthChange += Set;
	}

    public void Set(float health)
    {
       
        myImage.fillAmount = health / myHealth.maxHealth;
    }

    
}
