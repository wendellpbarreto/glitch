using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class InventoryLoader : MonoBehaviour {

	public GUISkin skin;

	public AudioSource audioSource;
	public AudioClip audioClip;

	void OnGUI(){
		GUI.skin = skin;
		
		int row = 0;
		int col = 0;

		string itemId = Player.character.inventory.head;
		if (itemId != null) {
			Item item = Game.GetItemById (itemId);
			GUI.Button (new Rect (20, 20, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString ()));
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
		}
		itemId = Player.character.inventory.shoulder;
		if (itemId != null) {
			Item item = Game.GetItemById (itemId);
			GUI.Button (new Rect (83, 20, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString()));
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
		}
		itemId = Player.character.inventory.body;
		if (itemId != null) {
			Item item = Game.GetItemById (itemId);
			GUI.Button (new Rect (20, 88, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString()));
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
		}
		itemId = Player.character.inventory.hand;
		if (itemId != null) {
			Item item = Game.GetItemById (itemId);
			GUI.Button (new Rect (83, 88, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString()));
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
		}
		itemId = Player.character.inventory.feet;
		if (itemId != null) {
			Item item = Game.GetItemById (itemId);
			GUI.Button (new Rect (20, 155, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString()));
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
		}
		itemId = Player.character.inventory.weapon;
		if (itemId != null) {
			Item item = Game.GetItemById (itemId);
			GUI.Button (new Rect (83, 155, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString()));
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
		}
		foreach (CharacterItem characterItem in Player.character.inventory.bag){
			Item item = Game.GetItemById (characterItem.itemId);
			if (GUI.Button (new Rect (Screen.width / 4 + 45 + (col * 65), 55 + row * 67, 60, 65), new GUIContent (item.name + "\n" + item.bodyPart, item.ToString ()))) {
				audioSource.PlayOneShot (audioClip);
				EquipItem (item);
			}
			GUI.Label(new Rect(Screen.width/4 + 30, Screen.height/2+10, Screen.width*3/4-45, Screen.height/2 -25), GUI.tooltip);
			col++;
			if (col > 7) {
				col = 0;
				row++;
			}
		}
		if (GUI.Button (new Rect (Screen.width - 225, Screen.height - 75, 200, 50), "Close")) {
			audioSource.PlayOneShot (audioClip);
			SceneManager.LoadScene ("Home");
		}
	}

	void EquipItem(Item item){
		string bodyPart = item.bodyPart.ToLower ();
		string itemId = item.id;
		KiiBucket bucket = Kii.Bucket("inventories");
		KiiQuery query = new KiiQuery (KiiClause.Equals ("characterName", Player.character.name));
		bucket.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to equipItem" + e.ToString ());
			} else {
				Debug.Log (Player.character.name + " - " + list.Count.ToString () + " iventories found");
				if (list.Count > 0){
					KiiObject obj = list[0];
					obj[bodyPart] = itemId;
					obj.Save((KiiObject savedObj, Exception e2) => {
						if (e != null)
						{
							Debug.LogError("Failed to save character" + e2.ToString());
						}
						else
						{
							Debug.Log("Item equiped: "+ item.name);
							Player.character.addKiiInventory (savedObj);
						}
					});
				}
			}
		});
	}
}
