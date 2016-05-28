using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System;

public class CharacterSelection : MonoBehaviour {

	List<Character> characters;
	public GUISkin skin;

	// Use this for initialization
	void OnLevelWasLoaded () {
		if (Game.gameStatus == GameStatus.Loading)
			Game.LoadClasses();
	}
	
	// Update is called once per frame
	void Update () {
		if (Game.gameStatus == GameStatus.LoadedClasses) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			Game.LoadClassesSkills();
		}
		if (Game.gameStatus == GameStatus.LoadedClassesSkills) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			Game.LoadItems ();	
		}
		if (Game.gameStatus == GameStatus.LoadedItems) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			Game.LoadEnemies ();
		}
		if (Game.gameStatus == GameStatus.LoadedEnemies) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			Game.LoadWorlds ();
		}
		if (Game.gameStatus == GameStatus.LoadedWorlds) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			Game.LoadWorldsEnemies ();
		}
		if (Game.gameStatus == GameStatus.LoadedWoroldsEnemies) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			characters = new List<Character> ();
			loadCharacters ();
		}
	}

	void selectCharacter(){
		
	}

	void OnGUI(){
		GUI.skin = skin;

		if (Game.gameStatus == GameStatus.Ready) {
			if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), "Create Character")) {
				Debug.LogWarning("Create Character");
				Application.LoadLevel ("CreateCharacter");
			}
			int i = 1;
			if (characters != null && characters.Count > 0)
				foreach(Character character in characters){
					if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15 + i*35, 200, 30), character.characterClass.name.ToString () + " - " + character.level.ToString ())) {
						Debug.LogWarning ("Character selected");
						Player.character = character;
						Application.LoadLevel ("Home");
					}
					i += 1;
				}
		}
	}

	void loadCharacters(){
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("characters");
		KiiQuery query = new KiiQuery (KiiClause.Equals("username", user.Username));
		bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to save score" + e.ToString ());
			} else {
				Debug.Log (list.Count.ToString () + " characters found");
				foreach (KiiObject obj in list) {
					characters.Add (Character.KiiObjToCharacter (obj));
				}
				Game.gameStatus = GameStatus.Ready;
			}
		});
	}
}
