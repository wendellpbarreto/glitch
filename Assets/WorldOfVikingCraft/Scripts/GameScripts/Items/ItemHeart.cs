using UnityEngine;
using System.Collections;

public class ItemHeart : MonoBehaviour {

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
		iname = "Ice Heart";
		quality = "normal";
		type = "Item";
		cost = 30;
		
		damage = 0.0f;
		strength = 0;
		stamina = 0;
		
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
}
