/*
Ultimate MMORPG Kit - SettingsScripts/Items - Client's script - skeletarik@gmail.com - 2013
This script is for items that can be taken by crafting (creating items with Player's profession).
Simply copy that:
" 
if(n=="Great Sword"){
			w_name="Great Sword";
			quality="normal";
			type="Weapon";
			cost=65;
			damage=6.0f;
			strength=3;
			stamina=6;
			pic="item16_ps";
			plHealth=0;
			plEnergy=0;
			bookText="";
}else
"
And paste after "}" and before "else" (111 line). Read the Documentation for more details.
*/

using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {
	
	static string w_name;				//Item's name
	static string quality;				//Item's quality
	static int cost;					//Item's price
	static string type;					//Item's type
	static string pic;					//The name of the item's picture in the Resources folder
	
	//For weapons and armor
	static float damage;				//Item's damage
	static int strength;				//Item's strength
	static int stamina;					//Item's stamina
	
	//For food
	static int plHealth;				//The quantity of health that will be restored by eating this item
	static int plEnergy;				//The quantity of energy that will be restored by eating this item
	
	//For books
	static string bookText;				//Item's text
	
	public static string ReturnItem(string n){
		if(n=="Copper Ore"){
			w_name="Copper Ore";
			quality="bad";
			type="Item";
			cost=2;
			damage=0.0f;
			strength=0;
			stamina=0;
			pic="item13_ps";
			plHealth=0;
			plEnergy=0;
			bookText="";
		}else
		if(n=="Copper Bar"){
			w_name="Copper Bar";
			quality="normal";
			type="Item";
			cost=10;
			damage=0.0f;
			strength=0;
			stamina=0;
			pic="item14_ps";
			plHealth=0;
			plEnergy=0;
			bookText="";
		}else
		if(n=="Good Sword"){
			w_name="Good Sword";
			quality="normal";
			type="Weapon";
			cost=30;
			damage=5.0f;
			strength=2;
			stamina=3;
			pic="item15_ps";
			plHealth=0;
			plEnergy=0;
			bookText="";
		}else
		if(n=="Great Sword"){
			w_name="Great Sword";
			quality="normal";
			type="Weapon";
			cost=65;
			damage=6.0f;
			strength=3;
			stamina=6;
			pic="item16_ps";
			plHealth=0;
			plEnergy=0;
			bookText="";
		}else
		if(n=="Real Sword"){
			w_name="Real Sword";
			quality="rare";
			type="Weapon";
			cost=100;
			damage=7.0f;
			strength=6;
			stamina=10;
			pic="item17_ps";
			plHealth=0;
			plEnergy=0;
			bookText="";
		}else
		{
			return "0";
		}
		return w_name+"!"+quality+"!"+type+"!"+cost.ToString()+"!"+damage.ToString()+"!"+strength.ToString()+"!"+stamina.ToString()+"!"+pic+"!"+plHealth.ToString()+"!"+plEnergy.ToString()+"!"+bookText;
	}
}
