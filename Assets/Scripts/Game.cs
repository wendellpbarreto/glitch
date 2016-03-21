using System;
using UnityEngine;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using System.Collections;


public static class Game {
	public static List<CharacterClass> characterClasses;
	public static List<Enemy> enemies;
	public static List<World> worlds;

	public static List<Item> items;
	public static bool gameIsReady = false;

	public static void LoadStatic(){
		Game.LoadClasses();
		Game.LoadClassesSkills();
		Game.LoadItems ();
		Game.LoadEnemies ();
		Game.LoadWorlds ();
		Game.LoadWorldsEnemies ();
		gameIsReady = true;
	}

	private static void LoadClasses(){
		characterClasses = new List<CharacterClass> ();
		
		KiiBucket bucket = Kii.Bucket("characterClasses");
		List<KiiObject> list = bucket.Query (new KiiQuery ());
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
	}

	private static void LoadClassesSkills(){
		KiiBucket bucket = Kii.Bucket("skills");
		foreach(CharacterClass characterClass in characterClasses){
			KiiQuery query = new KiiQuery (KiiClause.Equals("characterClassName", characterClass.name));
			List<KiiObject> list = bucket.Query (query);
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
				skill.isMelee = obj.GetBoolean("isMelee");
				skill.isAoe = obj.GetBoolean("isAoe");
				skill.aoe = obj.GetInt("aoe");
				skill.width = obj.GetInt("width");

				characterClass.skills.Add(skill);
			}
		}
	}

	private static void LoadItems(){
		items = new List<Item> ();

		KiiBucket bucket = Kii.Bucket("items");
		List<KiiObject> list = bucket.Query (new KiiQuery ());
		Debug.Log(list.Count.ToString() + " items Loaded");
		foreach (KiiObject obj in list)
		{
			Item item = new Item();
			item.id = obj.GetString("_id");

			item.characterClassName = obj.GetString("characterClassName");
			item.bodyPart = obj.GetString("bodyPart");
			item.itemTier = obj.GetString("itemTier");

			item.name = obj.GetString("name");
			item.description = obj.GetString("description");
			item.iconName = obj.GetString("iconName");
			item.meshName = obj.GetString("meshName");

			item.hp = (float) obj.GetDouble("hp");
			item.mp = (float) obj.GetDouble("mp");
			item.defense = (float) obj.GetDouble("defense");

			item.strenght = (float) obj.GetDouble("strenght");
			item.dextrery = (float) obj.GetDouble("dextrery");
			item.inteligence = (float) obj.GetDouble("inteligence");
			item.vitality = (float) obj.GetDouble("vitality");

			items.Add(item);
		}
	}

	private static void LoadEnemies(){
		enemies = new List<Enemy> ();

		KiiBucket bucket = Kii.Bucket("enemies");
		List<KiiObject> list = bucket.Query (new KiiQuery ());
		Debug.Log(list.Count.ToString() + " enemies loaded");
		foreach (KiiObject obj in list)
		{
			enemies.Add(
				new Enemy(
					obj.GetString("_id"),
					obj.GetString("name"),
					obj.GetString("prefabName"),
					(float) obj.GetDouble("strenght"),
					(float) obj.GetDouble("dextrery"),
					(float) obj.GetDouble("inteligence"),
					(float) obj.GetDouble("vitality")
				)
			);
		}
	}

	private static void LoadWorlds(){
		worlds = new List<World> ();

		KiiBucket bucket = Kii.Bucket("worlds");
		List<KiiObject> list = bucket.Query (new KiiQuery ());
		Debug.Log(list.Count.ToString() + " worlds loaded");
		foreach (KiiObject obj in list)
		{
			World world = new World (
				              obj.GetString ("_id"),
				              obj.GetString ("name"),
				              obj.GetInt ("xpReward"),
				              obj.GetInt ("goldReward"),
				              obj.GetString ("itemTier")
			              );
			worlds.Add(world);
		}
	}

	public static void LoadWorldsEnemies(){
		KiiBucket bucket = Kii.Bucket("worldEnemies");
		foreach (World world in worlds) {
			KiiQuery query = new KiiQuery (KiiClause.Equals("worldId", world.id));
			List<KiiObject> list = bucket.Query (query);
			Debug.Log(world.name+": "+list.Count.ToString()+" spawn found");
			foreach (KiiObject obj in list)
			{
				world.worldEnemies.Add (
					new WorldEnemy (obj.GetString ("worldId"), obj.GetString ("enemyId"))
				);
			}
		}
	}


	public static CharacterClass GetClassByName(string name){
		foreach (CharacterClass characterClass in characterClasses) {
			if (name == characterClass.name)
				return characterClass;
		}
		return null;
	}

	public static Item GetItemById(string id){
		foreach (Item item in items) {
			if (id == item.id)
				return item;
		}
		return null;
	}

	public static Enemy GetEnemyById(string id){
		foreach (Enemy enemy in enemies) {
			if (id == enemy.id)
				return enemy;
		}
		return null;
	}

	public static World GetWorldById(string id){
		foreach (World enemy in worlds) {
			if (id == enemy.id)
				return enemy;
		}
		return null;
	}
}