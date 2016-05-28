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

	public String ToString(){
		String returnString = "";
		returnString += (this.name + "\n");
		returnString += (this.bodyPart.ToString() + "\n");
		returnString += ("HP: " + this.hp.ToString() + "\n");
		returnString += ("MP: " + this.mp.ToString() + "\n");
		returnString += ("Defense: " + this.defense.ToString() + "\n");
		returnString += ("Str: " + this.strenght.ToString() + "\n");
		returnString += ("Dex: " + this.dextrery.ToString() + "\n");
		returnString += ("Int: " + this.inteligence.ToString() + "\n");
		returnString += ("Vit: " + this.vitality.ToString() + "\n");
		return returnString;
	}
}