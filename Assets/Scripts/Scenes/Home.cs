using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class Home : MonoBehaviour {
	public GUISkin skin;
	private bool ready;
	// Use this for initialization
	void OnLevelWasLoaded () {
		ready = false;
		ReloadCharacter ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (ready == true) {
			GUI.skin = skin;

			GUI.TextArea (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 30), "YOU ARE A " + Player.character.characterClassName);

			foreach (World world in Game.worlds)
				if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 30), "Enter " + world.name)) {
					Player.currentWorld = world;
					Application.LoadLevel ("World");
				}
		}
	}

	void ReloadCharacter(){
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("characters");
		KiiQuery query = new KiiQuery (KiiClause.Equals("name", Player.character.name));
		bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to save score" + e.ToString ());
			} else {
				Debug.Log (Player.character.name + " character found");
				Player.character = Character.KiiObjToCharacter (list[0]);
				loadCharacterSkills();
			}
		});
	}

	void loadCharacterSkills(){
		KiiBucket bucket = Kii.Bucket("characterSkills");
		KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", Player.character.name));
		bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to save score" + e.ToString ());
			} else {
				Debug.Log (Player.character.name + " - " + list.Count.ToString () + " character skills found");
				foreach (KiiObject obj in list) {
					Player.character.addKiiCharacterSkills (obj);
					loadCharacterIventory();
				}
			}
		});
	}

	void loadCharacterIventory(){
		KiiBucket bucket = Kii.Bucket("iventories");
		KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", Player.character.name));
		bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to save score" + e.ToString ());
			} else {
				Debug.Log (Player.character.name + " - " + list.Count.ToString () + " iventories found");
				if (list.Count > 0)
					Player.character.addKiiInventory (list [0]);
				loadCharacterItems();
			}
		});
	}

	void loadCharacterItems(){
		KiiBucket bucket = Kii.Bucket("characterItems");
		KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", Player.character.name));
		bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to save score" + e.ToString ());
			} else {
				Debug.Log (Player.character.name + " - " + list.Count.ToString () + " character items found");
				foreach (KiiObject obj in list) {
					Player.character.addKiiCharacterItem (obj);
				}
				this.ready = true;
			}
		});
	}
}
