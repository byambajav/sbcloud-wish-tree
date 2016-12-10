using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HumanDetector : MonoBehaviour
{
	public SerialHandler serialHandler;
//
//	private List<Vector3> angleCache = new List<Vector3>();
//	public int angleCacheNum = 10;
//	public Vector3 angle {
//		private set {
//			angleCache.Add(value);
//			if (angleCache.Count > angleCacheNum) {
//				angleCache.RemoveAt(0);
//			}
//		}
//		get {
//			if (angleCache.Count > 0) {
//				var sum = Vector3.zero;
//				angleCache.ForEach(angle => { sum += angle; });
//				return sum / angleCache.Count;
//			} else {
//				return Vector3.zero;
//			}
//		}
//	}

	void Start()
	{
		serialHandler.OnDataReceived += OnDataReceived;
	}

	void Update()
	{
//		transform.rotation = Quaternion.Euler(angle);
	}

	void OnDataReceived(string message)
	{
//		var data = message.Split(
//			new string[]{"\t"}, System.StringSplitOptions.None);
//		if (data.Length < 2) return;
//
//		try {
//			var angleX = float.Parse(data[0]);
//			var angleY = float.Parse(data[1]);
//			angle = new Vector3(angleX, 0, angleY);
//		} catch (System.Exception e) {
//			Debug.LogWarning(e.Message);
//		}
		Debug.Log(message);
		message = message.Trim ();
		if (message.Contains ("HUMAN")) {
			
//			SceneManager.UnloadSceneAsync ("SensorReader");
			SceneManager.LoadScene ("SelfMade");
			Debug.Log ("snowing");
//			Application.LoadLevel("SelfMade");
		}

	}
}