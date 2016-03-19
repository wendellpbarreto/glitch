using System;
using UnityEngine;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System.Collections;


public static class Game {
	public static List<CharacterClass> characterClasses;
	public static List<Item> items;
	public static bool gameIsReady = false;

	public static void LoadStatic(){
		Game.LoadClasses();
	}

	private static void LoadClasses(){
		characterClasses = new List<CharacterClass> ();
		
		KiiBucket bucket = Kii.Bucket("characterClasses");
		bucket.Query(new KiiQuery(), (KiiQueryResult<KiiObject> list, Exception e) => {
			if (e != null)
			{
				Debug.LogError("Failed to query " + e.ToString());
			}
			else
			{
				Debug.Log(list.Count.ToString() + " classes Loaded");
				foreach (KiiObject obj in list)
				{
					CharacterClass characterClass = new CharacterClass();
					characterClass.name = obj.GetString("name");

					characterClass.prefabName = obj.GetString("prefabName");
					characterClass.deathAnimation = obj.GetString("deathAnimation");
					characterClass.idleanimation = obj.GetString("idleanimation");
					characterClass.runAnimation = obj.GetString("runAnimation");

					//Atributes Stuff
					characterClass.strenght = (float) obj.GetDouble("strenght");
					characterClass.dextrery = (float) obj.GetDouble("dextrery");
					characterClass.inteligence = (float) obj.GetDouble("inteligence");
					characterClass.vitality = (float) obj.GetDouble("vitality");

					characterClass.strenghtPerLevel = (float) obj.GetDouble("strenghtPerLevel");
					characterClass.dextreryPerLevel = (float) obj.GetDouble("dextreryPerLevel");
					characterClass.inteligencePerLevel = (float) obj.GetDouble("inteligencePerLevel");
					characterClass.vitalityPerLevel = (float) obj.GetDouble("vitalityPerLevel");

					characterClass.mainAttributeName = obj.GetString("mainAttributeName");

					characterClasses.Add(characterClass);
				}
				Game.LoadClassesSkills();
			}

		});
	}

	private static void LoadClassesSkills(){
		KiiBucket bucket = Kii.Bucket("skills");

		foreach(CharacterClass characterClass in characterClasses){
			KiiQuery query = new KiiQuery (KiiClause.Equals("characterClassName", characterClass.name));
			bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) => {
				if (e != null)
				{
					Debug.LogError("Failed to query " + e.ToString());
				}
				else
				{
					Debug.Log(characterClass.name+": "+list.Count.ToString()+" skills found");
					foreach (KiiObject obj in list)
					{
						Skill skill = new Skill();

						skill.id = obj.GetString("_id");
						skill.animationName = obj.GetString("animationName");
						skill.name = obj.GetString("name"); 
						skill.description = obj.GetString("description");
						skill.damage = (float) obj.GetDouble("damage");
						skill.damagePerLevel = (float) obj.GetDouble("damagePerLevel");
						skill.cooldown = (float) obj.GetDouble("cooldown");
						skill.range = (float) obj.GetDouble("range");

						characterClass.skills.Add(skill);
					}
					if (characterClass == characterClasses[characterClasses.Count-1])
						Game.gameIsReady = true;
				}
			});
		}
	}

	private static void LoadClassesItems(){

	}

	private static void LoadItems(){
	
	}

	public static CharacterClass GetClassByName(string name){
		foreach (CharacterClass characterClass in characterClasses) {
			if (name == characterClass.name)
				return characterClass;
		}
		return null;
	}

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
}

