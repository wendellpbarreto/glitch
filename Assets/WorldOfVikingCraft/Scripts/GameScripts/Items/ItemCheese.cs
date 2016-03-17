using UnityEngine;
using System.Collections;

public class ItemCheese : MonoBehaviour {

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
		iname = "Cheese";
		quality = "normal";
		type = "Food";
		cost = 10;
		
		damage = 0;
		strength = 0;
		stamina = 0;
		
		plHealth = 35;
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
}
