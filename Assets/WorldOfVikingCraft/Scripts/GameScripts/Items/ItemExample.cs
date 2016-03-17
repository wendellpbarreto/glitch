/*
Ultimate MMORPG Kit - GameScripts/Items/ItemExample - Client's and Server's script - skeletarik@gmail.com - 2013
This script is an example of the item's script. For each item in your MMORPG you need to make a quest like this. 
Just copy that script and change the values in the "Awake()" function.
*/

using UnityEngine;
using System.Collections;

public class ItemExample : MonoBehaviour {

	string iname;
	string quality;
	string type;
	int cost;
	public Texture2D pic;
	
	//For weapons and armor
	
	float damage;	
	int strength;
	int stamina;
	
	//For food
	
	int plHealth;	
	int plEnergy;
	
	//For books
	
	string bookText;	

	/*
	 * You can add more parameters
	 * 
	*/
	
	void Awake(){
		iname = "Item's name";
		quality = "Item's quality - bad, normal or rare";
		type = "Item's type - Item, Weapon, Food or Book";
		cost = 20;
		
		damage = 0;
		strength = 0;
		stamina = 0;
		
		plHealth = 80;
		plEnergy = 0;
		
		bookText = "";
	}
	
	public void SendStats(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Drop").SendMessage("GetStats", sData);	
	}
	
	public void SendStatsVendor(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Vendor").SendMessage("GetStats", sData);	
	}
	
	public void SendStatsQuest(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Quest").SendMessage("GetStats", sData);	
	}
	
	public void SendStatsCraft(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Profession").SendMessage("GetStats", sData);	
	}
}