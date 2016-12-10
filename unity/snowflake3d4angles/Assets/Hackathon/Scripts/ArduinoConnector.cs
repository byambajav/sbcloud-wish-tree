using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;

using System.IO.Ports;



public class ArduinoConnector : MonoBehaviour {

	public Vector3[] newVertices;
	public Vector2[] newUV;
	public int[] newTriangles;

	private SerialPort stream;

	void Start()
	{
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;




	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		//Serial
		if (stream == null) {
			stream = new SerialPort ("/dev/tty.usbmodem1421", 9600);
			stream.ReadTimeout = 50;
			stream.Open ();
		}
		WriteToArduino("PING");
		StartCoroutine
		(
			AsynchronousReadFromArduino
			(   (string s) => Debug.Log(s),     // Callback
				() => Debug.LogError("Error!"), // Error callback
				10f                             // Timeout (seconds)
			)
		);
	}

	public void WriteToArduino(string message) {
		stream.WriteLine(message);
		stream.BaseStream.Flush();
	}

	public string ReadFromArduino (int timeout = 0) {
		stream.ReadTimeout = timeout;        
		try {
			return stream.ReadLine();
		}
		catch (TimeoutException) {
			return null;
		}
	}

	public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity) {
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);

		string dataString = null;

		do {
			try {
				dataString = stream.ReadLine();
			}
			catch (TimeoutException) {
				dataString = null;
			}

			if (dataString != null)
			{
				callback(dataString);
				yield return null;
			} else
				yield return new WaitForSeconds(0.05f);

			nowTime = DateTime.Now;
			diff = nowTime - initialTime;

		} while (diff.Milliseconds < timeout);

		if (fail != null)
			fail();
		yield return null;
	}
}
