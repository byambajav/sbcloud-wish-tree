using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireworkManager : MonoBehaviour {

    Vector3 spawnLocation;

	// Use this for initialization
	void Start () {
        spawnLocation = new Vector3(0, 2, -500);
        InvokeRepeating("ChangePosition", 0, 2); //calls ChangePosition() every 2 secs
    }
	
    void ChangePosition()
    {
        gameObject.SetActive(false);
        transform.position = spawnLocation;
        spawnLocation = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), -500);
        gameObject.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
