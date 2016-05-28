using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class World
{
	public string id;
	public string name;
	public int xpReward;
	public int goldReward;
	public string itemTier;
	public List<WorldEnemy> worldEnemies;

	public World(){
	}

	public World(string id, string name, int xpReward, int goldReward, string itemTier){
		this.id = id;
		this.name = name;
		this.xpReward = xpReward;
		this.goldReward = goldReward;
		this.itemTier = itemTier;
		this.worldEnemies = new List<WorldEnemy> ();
	}

	public Item GiveReward(){
		KiiBucket bucketCharacters = Kii.Bucket("characters");
		KiiBucket bucketCharacterItems = Kii.Bucket("characterItems");
		Item item = Game.GetRandomItemFromTier(Player.character.characterClass, this.itemTier);

		KiiQuery query = new KiiQuery (KiiClause.Equals("name", Player.character.name));
		bucketCharacters.Query (query, (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null) {
				Debug.LogError ("Failed to give reward" + e.ToString ());
			} else {
				Debug.Log (Player.character.name + " found");
				foreach (KiiObject obj in list) {
					obj["experience"] = obj.GetInt("experience") + xpReward;
					obj["gold"] = obj.GetInt("gold") + goldReward;
					obj.Save((KiiObject savedObj, Exception e2) => {
						if (e != null)
						{
							Debug.LogError("Failed to save character" + e2.ToString());
						}
						else
						{
							Debug.Log("XP and Gold Rewards added");
						}
					});
				}
			}
		});

		KiiObject characterItem = bucketCharacterItems.NewKiiObject();
		characterItem["itemId"] = item.id;
		characterItem["characterName"] = Player.character.name;
		characterItem.Save ((KiiObject obj2, Exception e2) => {
			if (e2 != null) {
				Debug.LogError ("Failed to giv items rewards" + e2.ToString ());
			} else {
				Debug.Log ("Character item reward added");

			}
		});

		return item;
	}
}

