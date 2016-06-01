using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory {

	public string characterName;

	public string head;
	public string body;
	public string shoulder;
	public string hand;
	public string feet;
	public string weapon;

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
		if (head != null) value += Game.GetItemById (head).strenght;
		if (body != null) value += Game.GetItemById (body).strenght;
		if (shoulder != null) value += Game.GetItemById (shoulder).strenght;
		if (hand != null) value += Game.GetItemById (hand).strenght;
		if (feet != null) value += Game.GetItemById (feet).strenght;
		if (weapon != null) value += Game.GetItemById (weapon).strenght;
		return value;
	}

	private float Dextrery(){
		float value = 0f;
		if (head != null) value += Game.GetItemById (head).dextrery;
		if (body != null) value += Game.GetItemById (body).dextrery;
		if (shoulder != null) value += Game.GetItemById (shoulder).dextrery;
		if (hand != null) value += Game.GetItemById (hand).dextrery;
		if (feet != null) value += Game.GetItemById (feet).dextrery;
		if (weapon != null) value += Game.GetItemById (weapon).dextrery;
		return value;
	}

	private float Inteligence(){
		float value = 0f;
		if (head != null) value += Game.GetItemById (head).inteligence;
		if (body != null) value += Game.GetItemById (body).inteligence;
		if (shoulder != null) value += Game.GetItemById (shoulder).inteligence;
		if (hand != null) value += Game.GetItemById (hand).inteligence;
		if (feet != null) value += Game.GetItemById (feet).inteligence;
		if (weapon != null) value += Game.GetItemById (weapon).inteligence;
		return value;
	}

	private float Vitality(){
		float value = 0f;
		if (head != null) value += Game.GetItemById (head).vitality;
		if (body != null) value += Game.GetItemById (body).vitality;
		if (shoulder != null) value += Game.GetItemById (shoulder).vitality;
		if (hand != null) value += Game.GetItemById (hand).vitality;
		if (feet != null) value += Game.GetItemById (feet).vitality;
		if (weapon != null) value += Game.GetItemById (weapon).vitality;
		return value;
	}
}