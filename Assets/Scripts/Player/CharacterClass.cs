using System;
using System.Collections.Generic;

//LOADED GAME INITIATION
public class CharacterClass : BaseAttributes {
	public string name;

	//Animation Stuff
	public string prefabName;
	public string deathAnimation;
	public string idleanimation;
	public string runAnimation;

	public float strenghtPerLevel;
	public float dextreryPerLevel;
	public float inteligencePerLevel;
	public float vitalityPerLevel;

	public string mainAttributeName;

	public List<Skill> skills;

	public CharacterClass(){
		skills = new List<Skill>();
	}

	public float MainAttributeValue(){
		switch(mainAttributeName){
		case "Strenght": return this.strenght; break;
		case "Dextrery": return this.dextrery; break;
		case "Inteligence": return this.inteligence; break;
		case "Vitality": return this.vitality; break;
		default: return 0f; break;
		}
	}

}