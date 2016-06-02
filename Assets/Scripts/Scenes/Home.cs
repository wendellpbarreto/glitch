using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Home : MonoBehaviour {
	public GUISkin skin;

	public AudioSource audioSource;
	public AudioClip audioClip;

	void OnGUI(){
		GUI.skin = skin;
		string playerMainAttribute = Player.character.characterClass.mainAttributeName;
		float playerMainAttributeValue = Player.character.characterClass.MainAttributeValue() + Player.character.inventory.AttributeByName(playerMainAttribute);
		GUI.Label (new Rect (15, 15, 200, 30), "Name: " + Player.character.name);
		GUI.Label (new Rect (15, 47 , 200, 30), "Class: " + Player.character.characterClassName);
		GUI.Label (new Rect (15, 79, 200, 30), "Level: " + Player.character.Level().ToString());
		GUI.Label (new Rect (15, 111, 200, 30),  playerMainAttribute + ": "  + playerMainAttributeValue.ToString());

		if (GUI.Button (new Rect (Screen.width - 210, Screen.height - 32, 200, 30), "Inventory")) {
			audioSource.PlayOneShot (audioClip);
			SceneManager.LoadScene ("Inventory");
		}

		int row = 0;
		foreach (World world in Game.worlds)
			if (GUI.Button (new Rect (Screen.width-215, 15+row*32, 200, 30), "Enter: " + world.name)) {
				audioSource.PlayOneShot (audioClip);
				Player.currentWorld = world;
				SceneManager.LoadScene ("World");
			}
	}
}
