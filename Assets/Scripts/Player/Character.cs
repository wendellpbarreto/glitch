using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;

public class Character {
	private string username;
	public string name;
	public int level;
	public int experience;
	public string title;
	public int gold;
	public int gem;

	public string characterClassName;
	public CharacterClass characterClass;
	public List<CharacterSkill> skills;
	public Inventory inventory;


	public float currentHp;
	public float currentMp;

	public Character(){
		skills = new List<CharacterSkill> ();
	}

	public void EnterWorld(){
		this.currentHp = this.MaxHP ();
		this.currentHp = this.MaxMP ();
	}

	public float MaxHP() {
		return characterClass.vitality * 100;
	}

	public float MaxMP() {
		return characterClass.inteligence * 100;
	}

	public bool IsAlive(){
		return this.currentHp > 0;
	}

	public void TakeDamage(float damage){
		this.currentHp -= damage;
	}

	public int SkillLevel(string skillId){
		foreach(CharacterSkill skill in skills){
			if (skill.skillId == skillId)
				return skill.skillLevel;
		}
		return 0;
	}

	public float MainAttributeValue(){
		if (this.inventory != null)
			return this.characterClass.MainAttributeValue() + this.inventory.AttributeByName(this.characterClass.mainAttributeName);
		return this.characterClass.MainAttributeValue ();
	}

	public void addKiiCharacterSkills(KiiObject obj){
		CharacterSkill characterSkill = new CharacterSkill ();
		characterSkill.characterName = obj.GetString ("characterName");
		characterSkill.skillId = obj.GetString ("skillId");
		characterSkill.skillLevel = obj.GetInt ("skillLevel");
		skills.Add (characterSkill);
	}

	public static Character KiiObjToCharacter(KiiObject obj){
		Character character = new Character ();
		character.username = obj.GetString ("username");
		character.name = obj.GetString ("name");
		character.title = obj.GetString ("title");
		character.level = obj.GetInt ("level");
		character.experience = obj.GetInt ("experience");
		character.gold = obj.GetInt ("gold");
		character.gem = obj.GetInt ("gem");
		character.characterClassName = obj.GetString ("characterClassName");
		character.characterClass = Game.GetClassByName (character.characterClassName);
		return character;
	}
}
