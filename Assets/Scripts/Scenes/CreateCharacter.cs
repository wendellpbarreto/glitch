using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

[ExecuteInEditMode]
public class CreateCharacter : MonoBehaviour {
	private CharacterClass characterClass;
	private string name = "CharacterName";

	public GUISkin skin;

	void OnLevelWasLoaded(){
		this.characterClass = Game.characterClasses [0];
	}

	void OnGUI(){
		GUI.skin = skin;

		name = GUI.TextField(new Rect(Screen.width/2 - 150, Screen.height/2 - 104, 300, 35), name, 25);
		GUI.Label(new Rect(Screen.width/2 - 150, Screen.height/2 - 52, 300, 35), "Class - "+characterClass.name);
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
				Player.character = Character.KiiObjToCharacter(obj);
				createCharacterSkills();
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
					createCharacterInventory();
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
				addTier1Items();

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
							if (list.IndexOf(obj) == list.Count-1)
								GoToHomeScreen();
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
				}	
			}
		});
	}

	void GoToCharacterSelection (){
		SceneManager.LoadScene("CharacterSelection");
	}

	void GoToHomeScreen(){
		SceneManager.LoadScene("Home");
	}
}
