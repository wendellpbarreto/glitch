using System;

public enum BodyPart{
	Head,
	Body,
	Shoulder,
	Hand,
	Feet,
	Weapon
}

public enum ItemTier {
	Tier1,
	Tier2,
	Tier3
}

public class Item {
	
	public string id;

	public string characterClassName;
	public string bodyPart;
	public string itemTier;

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