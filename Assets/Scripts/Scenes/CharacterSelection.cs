using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System;

public class CharacterSelection : MonoBehaviour {


	List<PlayerAttributes> characters;

	// Use this for initialization
	void OnLevelWasLoaded () {
		characters = new List<PlayerAttributes> ();
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
			foreach(PlayerAttributes character in characters){
				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15 + i*35, 200, 30), character.GetType ().ToString () + " - " + character.MaxHP ().ToString ())) {
					Debug.LogWarning ("Character selected");
					DataManager.SetPlayerAttributes (character);
					Application.LoadLevel ("World");
				}
				i += 1;
			}	
	}

	void loadCharacters(){
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("characters");
		KiiQuery query = new KiiQuery(KiiClause.Equals("username", 	user.Username));
		query.Limit = 5;
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>
			{
				if (e != null)
				{
					Debug.LogError("Failed to query " + e.ToString());
				}
				else
				{
					Debug.Log("Query succeeded");
					foreach (KiiObject obj in list)
					{
						var type = Type.GetType(obj.GetString("characterClass"));
						var playerAttributes = (PlayerAttributes)Activator.CreateInstance(type);
						characters.Add(playerAttributes);
					}
				}
			});
	}
}
