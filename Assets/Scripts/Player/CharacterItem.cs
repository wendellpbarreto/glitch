using System;

public class CharacterItem {
	public string id;
	public string itemId;
	public string characterName;

	public CharacterItem(){
		
	}

	public CharacterItem(string id, string itemId, string characterName){
		this.id = id;
		this.itemId = itemId;
		this.characterName = characterName;
	}
}