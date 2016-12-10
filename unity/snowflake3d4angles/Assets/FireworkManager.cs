using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireworkManager : MonoBehaviour {

    bool isActive;

	// Use this for initialization
	void Start () {
        isActive = true;
        gameObject.SetActive(false);
        //InvokeRepeating("ChangePosition", 0, 2); //calls ChangePosition() every 2 secs
    }
	
    void ChangePosition()
    {
        gameObject.SetActive(isActive);
        isActive = !isActive;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
