using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CharacterSelection : MonoBehaviour {

	public GUISkin skin;

	public AudioSource audioSource;
	public AudioClip audioClip;

	void OnGUI(){
		GUI.skin = skin;

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), "Create Character")) {
			audioSource.PlayOneShot (audioClip);
			Debug.LogWarning("Create Character");
			SceneManager.LoadScene ("CreateCharacter");
		}

		int i = 1;
		if (Player.characters != null && Player.characters.Count > 0)
			foreach(Character character in Player.characters){
				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15 + i*35, 200, 30), character.name.ToString () + " - " + character.Level().ToString ())) {
					audioSource.PlayOneShot (audioClip);
					Debug.LogWarning ("Character selected");
					Player.character = character;
					SceneManager.LoadScene ("CharacterLoader");
				}
				i += 1;
			}
	}
}
