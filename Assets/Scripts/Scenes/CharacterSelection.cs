using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System;

public class CharacterSelection : MonoBehaviour {

	List<Character> characters;

	// Use this for initialization
	void OnLevelWasLoaded () {
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
		if (Game.gameStatus == GameStatus.LoadedCharacters) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			loadCharactersSkills ();
		}
		if (Game.gameStatus == GameStatus.LoadedCharactersSkills){
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			loadCharactersIventory ();
		}
		if (Game.gameStatus == GameStatus.LoadedCharacterIventory){
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			loadCharactersItems ();
		}
	}

	void selectCharacter(){
		
	}

	void OnGUI(){
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
				Game.gameStatus = GameStatus.LoadedCharacters;
			}
		});
	}

	void loadCharactersSkills(){
		KiiBucket bucket = Kii.Bucket("characterSkills");
		foreach (Character character in characters) {
			KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", character.name));
			bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
				if (e != null) {
					Debug.LogError ("Failed to save score" + e.ToString ());
				} else {
					Debug.Log (character.name + " - " + list.Count.ToString () + " character skills found");
					foreach (KiiObject obj in list) {
						character.addKiiCharacterSkills (obj);
					}
					if (character == characters[characters.Count - 1])
						Game.gameStatus = GameStatus.LoadedCharactersSkills;
				}
			});
		}
	}

	void loadCharactersIventory(){
		KiiBucket bucket = Kii.Bucket("iventories");
		foreach (Character character in characters) {
			KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", character.name));
			bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
				if (e != null) {
					Debug.LogError ("Failed to save score" + e.ToString ());
				} else {
					Debug.Log (character.name + " - " + list.Count.ToString () + " iventories found");
					if (list.Count > 0)
						character.addKiiInventory (list [0]);
					Game.gameStatus = GameStatus.LoadedCharacterIventory;
				}
			});
		}
	}

	void loadCharactersItems(){
		KiiBucket bucket = Kii.Bucket("characterItems");
		foreach (Character character in characters) {
			KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", character.name));
			bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
				if (e != null) {
					Debug.LogError ("Failed to save score" + e.ToString ());
				} else {
					Debug.Log (character.name + " - " + list.Count.ToString () + " character items found");
					foreach (KiiObject obj in list) {
						character.addKiiCharacterItem (obj);
					}
					Game.gameStatus = GameStatus.Ready;
				}
			});
		}
	}
}
