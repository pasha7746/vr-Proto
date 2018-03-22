using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCountUI : MonoBehaviour
{
    private Canvas myCanvas;

    void Awake()
    {
        myCanvas = GetComponentInChildren<Canvas>();

    }

    // Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    void LateUpdate()
    {
        myCanvas.transform.position = transform.position;
    }
}
