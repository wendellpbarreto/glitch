using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class CreateCharacter : MonoBehaviour {
	private CharacterClass characterClass;
	private string name = "ADMIN";

	public GUISkin skin;

	// Use this for initialization
	void OnLevelWasLoaded () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = skin;

		name = GUI.TextField(new Rect(Screen.width/2 - 100, Screen.height/2 - 52, 200, 35), name, 25);

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), "Create")) {
			VerifyName ();
		}

		int i = 1;
		if (Game.characterClasses != null && Game.characterClasses.Count > 0)
			foreach(CharacterClass charClass in Game.characterClasses){
				if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15 + i*35, 200, 30), charClass.name)) {
					this.characterClass = charClass;
				}
				i += 1;
			}

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 17 + Game.characterClasses.Count*32, 200, 30), "Cancel")) {
			GoToCharacterSelection();
		}
		
	}

	public void createCharacter()
	{
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("characters");
		KiiObject kiiObj = bucket.NewKiiObject();
		kiiObj["username"] = user.Username;
		kiiObj["name"] = this.name;
		kiiObj["level"] = 0;
		kiiObj["experience"] = 0;
		kiiObj["title"] = "noob";
		kiiObj["gold"] = 0;
		kiiObj["gem"] = 0;
		kiiObj["characterClassName"] = this.characterClass.name;
		kiiObj.Save((KiiObject obj, Exception e) => {
			if (e != null) {
				Debug.LogError("Failed to save score" + e.ToString());
			} else {
				Debug.Log("Character created");
			}
		});
	}

	void createCharacterSkills(){
		KiiBucket bucket = Kii.Bucket("characterSkills");
		foreach (Skill skill in characterClass.skills) {
			KiiObject kiiObj = bucket.NewKiiObject();
			kiiObj["characterName"] = this.name;
			kiiObj["skillId"] = skill.id;
			kiiObj["skillLevel"] = 1;
			kiiObj.Save((KiiObject obj, Exception e) => {
				if (e != null) {
					Debug.LogError("Failed to save score" + e.ToString());
				} else {
					Debug.Log("Character skill created");
				}
			});
		}
	}

	void createCharacterInventory(){
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("inventories");
		KiiObject inventory = bucket.NewKiiObject();
		inventory["characterName"] = this.name;

		inventory["head"] = "";
		inventory["body"] = "";
		inventory["shoulder"] = "";
		inventory["hand"] = "";
		inventory["feet"] = "";
		inventory["weapon"] = "";

		inventory.Save((KiiObject obj, Exception e) => {
			if (e != null) {
				Debug.LogError("Failed to save score" + e.ToString());
			} else {
				Debug.Log("Character inventory created");
			}
		});
	}

	void addTier1Items(){
		KiiBucket bucket = Kii.Bucket("items");
		KiiQuery query = new KiiQuery (
			KiiClause.And(
				KiiClause.Equals("itemTier", ItemTier.Tier1.ToString()),
				KiiClause.Equals("characterClassName", characterClass.name)
			)
		);
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null){
				Debug.LogError("Failed to query " + e.ToString());
			} else {
				Debug.Log(list.Count.ToString()+" tier 1 itens found");
				KiiBucket characterItemsBucket = Kii.Bucket("characterItems");
				foreach(KiiObject obj in list){
					KiiObject characterItem = characterItemsBucket.NewKiiObject();
					characterItem["itemId"] = obj.GetString("_id");
					characterItem["characterName"] = this.name;
					characterItem.Save((KiiObject obj2, Exception e2) => {
						if (e != null) {
							Debug.LogError("Failed to save score" + e.ToString());
						} else {
							Debug.Log("Character item created");
						}
					});
				}
			}
		});
	}

	void VerifyName(){
		KiiBucket bucket = Kii.Bucket("characters");
		KiiQuery query = new KiiQuery (KiiClause.Equals("name", this.name));
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null){
				Debug.LogError("Failed to query " + e.ToString());
			} else {
				if (list.Count > 0){
					Debug.Log("Name is unavaliable");
				} else {
					Debug.Log("Name is avaliable");
					createCharacter();
					createCharacterSkills();
					createCharacterInventory();
					addTier1Items();
					GoToCharacterSelection();
				}	
			}
		});
	}

	void GoToCharacterSelection (){
		Game.gameStatus = GameStatus.LoadedWoroldsEnemies;
		Application.LoadLevel("CharacterSelection");
	}
}
