using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System;

public class GameLoader : MonoBehaviour {

	void OnLevelWasLoaded () {
		if (Game.gameStatus == GameStatus.Loading)
			Game.LoadLevelsXp();
	}

	// Update is called once per frame
	void Update () {
		if (Game.gameStatus == GameStatus.LoadedLevelsXp) {
			Debug.Log (Game.gameStatus);
			Game.gameStatus = GameStatus.Loading;
			Game.LoadClasses();
		}
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
			Player.characters = new List<Character> ();
			loadCharacters ();
		}
		if (Game.gameStatus == GameStatus.Ready)
			Application.LoadLevel ("CharacterSelection");
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
					Player.characters.Add (Character.KiiObjToCharacter (obj));
				}
				Game.gameStatus = GameStatus.Ready;
			}
		});
	}
}
