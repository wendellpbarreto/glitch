using System;

public class Inventory {
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
			default: return 0f; break;
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