using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System;

public class CharacterSelection : MonoBehaviour {


	List<Character> characters;

	// Use this for initialization
	void OnLevelWasLoaded () {
		characters = new List<Character> ();
		loadCharacters ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void selectCharacter(){
		
	}

	void OnGUI(){
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

	void loadCharacters(){
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("characters");
		KiiQuery query = new KiiQuery (KiiClause.Equals("username", user.Username));
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null)
			{
				Debug.LogError("Failed to query " + e.ToString());
			}
			else
			{
				Debug.Log(list.Count.ToString()+" characters found");
				foreach (KiiObject obj in list)
				{
					characters.Add(Character.KiiObjToCharacter(obj));
				}
				loadCharactersSkills();
			}
		});
	}
	void loadCharactersSkills(){
		KiiBucket bucket = Kii.Bucket("characterSkills");
		foreach (Character character in characters) {
			KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", character.name));
			bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) => {
				if (e != null)
				{
					Debug.LogError("Failed to query " + e.ToString());
				}
				else
				{
					Debug.Log(character.name + " - " + list.Count.ToString() + " character skills found");
					foreach (KiiObject obj in list)
					{
						character.addKiiCharacterSkills(obj);
					}
				}
			});
		}
	}
}
