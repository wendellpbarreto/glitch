using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory {

	public string characterName;

	public CharacterItem head;
	public CharacterItem body;
	public CharacterItem shoulder;
	public CharacterItem hand;
	public CharacterItem feet;
	public CharacterItem weapon;

	public List<CharacterItem> bag;

	public Inventory(){
		this.bag = new List<CharacterItem> ();
	}

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
		float value = 0f;
		if (head != null) value += Game.GetItemById (head.itemId).strenght;
		if (body != null) value += Game.GetItemById (body.itemId).strenght;
		if (shoulder != null) value += Game.GetItemById (shoulder.itemId).strenght;
		if (hand != null) value += Game.GetItemById (hand.itemId).strenght;
		if (feet != null) value += Game.GetItemById (feet.itemId).strenght;
		if (weapon != null) value += Game.GetItemById (weapon.itemId).strenght;
		return value;
	}

	private float Dextrery(){
		float value = 0f;
		if (head != null) value += Game.GetItemById (head.itemId).dextrery;
		if (body != null) value += Game.GetItemById (body.itemId).dextrery;
		if (shoulder != null) value += Game.GetItemById (shoulder.itemId).dextrery;
		if (hand != null) value += Game.GetItemById (hand.itemId).dextrery;
		if (feet != null) value += Game.GetItemById (feet.itemId).dextrery;
		if (weapon != null) value += Game.GetItemById (weapon.itemId).dextrery;
		return value;
	}

	private float Inteligence(){
		float value = 0f;
		if (head != null) value += Game.GetItemById (head.itemId).inteligence;
		if (body != null) value += Game.GetItemById (body.itemId).inteligence;
		if (shoulder != null) value += Game.GetItemById (shoulder.itemId).inteligence;
		if (hand != null) value += Game.GetItemById (hand.itemId).inteligence;
		if (feet != null) value += Game.GetItemById (feet.itemId).inteligence;
		if (weapon != null) value += Game.GetItemById (weapon.itemId).inteligence;
		return value;
	}

	private float Vitality(){
		float value = 0f;
		if (head != null) value += Game.GetItemById (head.itemId).vitality;
		if (body != null) value += Game.GetItemById (body.itemId).vitality;
		if (shoulder != null) value += Game.GetItemById (shoulder.itemId).vitality;
		if (hand != null) value += Game.GetItemById (hand.itemId).vitality;
		if (feet != null) value += Game.GetItemById (feet.itemId).vitality;
		if (weapon != null) value += Game.GetItemById (weapon.itemId).vitality;
		return value;
	}
}