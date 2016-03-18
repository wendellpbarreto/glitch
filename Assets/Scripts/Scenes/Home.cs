using UnityEngine;
using System.Collections;

public class Home : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.TextArea (new Rect (Screen.width/2 - 100, Screen.height/2 - 50, 200, 30), "YOU ARE A "+ DataManager.GetPlayerAttributes ().GetType().ToString());

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), "Enter World")) {
			Application.LoadLevel ("World");
		}
	}
}
