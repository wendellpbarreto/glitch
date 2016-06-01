using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Home : MonoBehaviour {
	public GUISkin skin;
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = skin;

		GUI.TextArea (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 30), "YOU ARE A " + Player.character.characterClassName);

		if (GUI.Button (new Rect (Screen.width - 210, Screen.height - 32, 200, 30), "Inventory")) {
			SceneManager.LoadScene ("Inventory");
		}

		foreach (World world in Game.worlds)
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 30), "Enter " + world.name)) {
				Player.currentWorld = world;
				SceneManager.LoadScene ("World");
			}
	}
}
