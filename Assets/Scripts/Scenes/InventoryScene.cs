using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class InventoryScene : MonoBehaviour {
	public GUISkin skin;
	
	// Update is called once per frame
	void OnGUI () {
		GUI.skin = skin;

		GUI.Box (new Rect(15, 15, Screen.width/4, Screen.height -30), "Equiped");

		GUI.Box (new Rect(Screen.width/4 + 30, 15, Screen.width*3/4-45, Screen.height /2 -15 ), "Bag");

		GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
	}
}
