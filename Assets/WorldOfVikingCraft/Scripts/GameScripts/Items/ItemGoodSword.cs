using UnityEngine;
using System.Collections;

public class ItemGoodSword : MonoBehaviour {

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
		iname = "Good Sword";
		quality = "normal";
		type = "Weapon";
		cost = 30;
		
		damage = 5.0f;
		strength = 2;
		stamina = 3;
		
		plHealth = 0;
		plEnergy = 0;
		
		bookText = "";
	}
	
	public void SendStats(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Drop").SendMessage("GetStats", sData);	
	}
	
	public void SendStatsQuest(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Quest").SendMessage("GetStats", sData);	
	}
	
	public void SendStatsCraft(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Profession").SendMessage("GetStats", sData);	
	}
	
	public void SendStatsVendor(){
		string sData = iname+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic.name+"!"+plHealth+"!"+plEnergy+"!"+bookText;
		gameObject.GetComponent("Vendor").SendMessage("GetStats", sData);	
	}
}
