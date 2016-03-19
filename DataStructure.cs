public static class Player () {
  //Loaded upon character selection
  //Created upon character creation
  public Character character;
}

public class Character(){
  public string name;
  public int level;
  public float experience;
  public string title;
  public int gold;
  public int gem;

  public CharacterClass characterClass;
  public CharacterSkill[] skills;
  public Inventory inventory;

  public int SkillLevel(string skillId){
    foreach(CharacterSkill skill in skills){
      if (skill.skillId == skillId)
        return skill.skillLevel;
    }
    return 0f;
  }

  public float MainAttributeValue(){
    this.characterClass.MainAttributeValue + this.inventory.AttributeByName(this.characterClass.mainAttributeName);
  }
}

public class CharacterItem(){
  public string itemId;
  public string characterId;
}


public class Inventory() {
  public CharacterItem head;
  public CharacterItem body;
  public CharacterItem shoulder;
  public CharacterItem hand;
  public CharacterItem feet;
  public CharacterItem weapon;
  public CharacterItem[] bag;

  public float AttributeByName(string name){
      switch(name){
        case "Strenght": return Strenght(); break;
        case "Dextrery": return Dextrery(); break;
        case "Inteligence": return Inteligence(); break;
        case "Vitality": return Vitality(); break;
        else: return 0f; break;
      }
  }

  private float Strenght(){
    return 0f;
  }
  private float Dextrery(){
    return 0f;
  }
  private float Inteligence(){
    return 0f;
  }
  private float Vitality(){
    return 0f;
  }
}

//Relation
public class CharacterSkill {
  public string skillId;
  public string characterId;
  public int skillLevel;
}

//LOADED GAME INITIATION
public static class CharacterClass(){

  public string name;

  //Animation Stuff
  public string prefabName;
  public string attackAnimation;
  public string deathAnimation;
  public string idleanimation;
  public string runAnimation;

  //Atributes Stuff
  public float strenght;
  public float dextrery;
  public float inteligence;
  public float vitality;

  public float strenghtPerLevel;
  public float dextreryPerLevel;
  public float inteligencePerLevel;
  public float vitalityPerLevel;

  public string mainAttributeName;

  public Skill[] skills;
  public Item[] items;

  public float MainAttributeValue(){
    switch(mainAttributeName){
      case "Strenght": return this.strenght; break;
      case "Dextrery": return this.dextrery; break;
      case "Inteligence": return this.inteligence; break;
      case "Vitality": return this.vitality; break;
      else: return 0f; break;
    }
  }
}

public class Skill {
  public string id;
  public string animationName;
  public string name;
  public string description;
  public float damage;
  public float damagePerLevel;

  public float Damage(){
    return Player.character.MainAttributeValue * (damage+(damagePerLevel*Player.character.SkillLevel(this.id)));
  }
}

public class Item(){
  public string id;

  public string characterClass;
  public string bodyPart;

  public string name;
  public string description;
  public string iconName;
  public string meshName;

  public float hp;
  public float mp;
  public float defense;

  public float strenght;
  public float dextrery;
  public float inteligence;
  public float vitality;
}







//Definir defaults
//Animation order
//