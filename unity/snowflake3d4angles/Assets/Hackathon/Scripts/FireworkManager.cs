using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FireworkManager : MonoBehaviour {

    public Transform firework1;
    public Transform firework2;

    // Use this for initialization
    void Start()
    {
        firework1.gameObject.SetActive(false);
        firework2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Called");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Play());
        }
		if (Input.GetButtonDown("Fire1"))
		{
			SceneManager.LoadScene ("SensorReader");
		}

    }

    IEnumerator Play()
    {
        firework1.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        firework2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        firework1.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        firework2.gameObject.SetActive(false);
    }
}
