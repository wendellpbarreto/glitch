using UnityEngine;
using System.Collections;
using System;
using KiiCorp.Cloud.Storage;

public static class GoblinClassInitializer {

	private static void InitializeGoblinClass(){

		KiiBucket characterClassBucket = Kii.Bucket("characterClasses");
		KiiObject kiiObj = characterClassBucket.NewKiiObject();

		kiiObj["name"] = "goblin";

		kiiObj["prefabName"] = "Goblin";
		kiiObj["deathAnimation"] = "";
		kiiObj["idleanimation"] = "idle";
		kiiObj["runAnimation"] = "run";

		//Atributes Stuff
		kiiObj["strenght"] = 10d;
		kiiObj["dextrery"] = 10d;
		kiiObj["inteligence"] = 10d;
		kiiObj["vitality"] = 10d;

		kiiObj["strenghtPerLevel"] = 1d;
		kiiObj["dextreryPerLevel"] = 1d;
		kiiObj["inteligencePerLevel"] = 1d;
		kiiObj["vitalityPerLevel"] = 1d;

		kiiObj["mainAttributeName"] = "Dextrery";

		kiiObj.Save((KiiObject obj, Exception e) => {
			if (e != null)
			{
				Debug.LogError("Failed to save score" + e.ToString());
			}
			else
			{
				Debug.Log("Goblin Created");
			}
		});

		KiiBucket skillBucket = Kii.Bucket("skills");
		KiiObject skill = skillBucket.NewKiiObject();

		skill["characterClassName"] = "goblin";
		skill["animationName"] = "attack3";
		skill["name"] = "RegularAttack";
		skill["description"] = "Regular Goblin Attack";
		skill["damage"] = 100d;
		skill["damagePerLevel"] = 20d;
		skill["cooldown"] = 1d;
		skill["range"] = 5d;

		skill.Save((KiiObject obj, Exception e) => {
			if (e != null)
			{
				Debug.LogError("Failed to save score" + e.ToString());
			}
			else
			{
				Debug.Log("Goblin regular attack added");
			}
		});
	}

	public static void InitializeGoblinItems(){
		KiiBucket itemBucket = Kii.Bucket("items");
		foreach (BodyPart bodyPart in Enum.GetValues(typeof(BodyPart))) {
			KiiObject item = itemBucket.NewKiiObject();
			item ["characterClassName"] = "goblin";
			item ["bodyPart"] = bodyPart;
			item ["itemTier"] = ItemTier.Tier1;

			item["name"] = "Tier 1";
			item ["description"] = "Tier 1";
			item ["iconName"] = "";
			item ["meshName"] = "";

			item ["hp"] = 100;
			item ["mp"] = 100;
			item ["defense"] = 10;

			item ["strenght"] = 5;
			item ["dextrery"] = 5;
			item ["inteligence"] = 5;
			item ["vitality"] = 5;	

			item.Save((KiiObject obj, Exception e) => {
				if (e != null)
				{
					Debug.LogError("Failed to save score" + e.ToString());
				}
				else
				{
					Debug.Log("Saved item");
				}
			});
		}
	}
}
