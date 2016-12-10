using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    [Range(1f, 100f)]
    public float rotationSpeed;
    //public Image placeholder;
    private Color c = new Color(1f, 1f, 1f, 1f);

    void Update()
    {
        //transform.Rotate(transform.position.x, rotationSpeed, transform.position.z);
		transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
        //transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.World);
        if (c.a != 0f)
        {
            c.a = c.a - 0.002f;
            //placeholder.color = new Color(1f, 1f, 1f, c.a);
        }
    }
}
