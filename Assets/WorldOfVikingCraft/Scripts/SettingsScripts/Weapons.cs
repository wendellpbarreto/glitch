/*
Ultimate MMORPG Kit - SettingsScripts/Weapons - Client's script - skeletarik@gmail.com - 2013
This script is for weapons.
Simply copy that:
" 
if(n=="Axe"){
			w_name="Axe";
			quality="normal";
			cost=15;
			damage=2;
			strength=0;
			stamina=0;
			pic="item9_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
}
"
And paste after "}" and before "else" (113 line). Read the Documentation for more details.
*/

using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour {
	
	static string w_name;				//Weapon's name
	static string quality;				//Weapon's quality
	static int cost;					//Weapon's price
	static int damage;					//Weapon's damage
	static int strength;				//Weapon's strength
	static int stamina;					//Weapon's stamina
	static string pic;					//The name of the weapon's picture in the Resources folder
	
	public static string ReturnWeapon(string n){
		if(n=="Big Axe"){
			w_name="Big Axe";
			quality="normal";
			cost=30;
			damage=4;
			strength=1;
			stamina=0;
			pic="item8_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Axe"){
			w_name="Axe";
			quality="normal";
			cost=15;
			damage=2;
			strength=0;
			stamina=0;
			pic="item9_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Shield"){
			w_name="Shield";
			quality="normal";
			cost=20;
			damage=0;
			strength=0;
			stamina=50;
			pic="item10_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Rare Axe"){
			w_name="Rare Axe";
			quality="rare";
			cost=100;
			damage=7;
			strength=2;
			stamina=15;
			pic="item5_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Pick Axe"){
			w_name="Pick Axe";
			quality="normal";
			cost=100;
			damage=1;
			strength=2;
			stamina=2;
			pic="item12_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Good Sword"){
			w_name="Good Sword";
			quality="normal";
			cost=30;
			damage=5;
			strength=2;
			stamina=3;
			pic="item15_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Great Sword"){
			w_name="Great Sword";
			quality="normal";
			cost=65;
			damage=6;
			strength=3;
			stamina=6;
			pic="item16_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		if(n=="Real Sword"){
			w_name="Real Sword";
			quality="rare";
			cost=100;
			damage=7;
			strength=6;
			stamina=10;
			pic="item17_ps";
			return w_name+":"+quality+":"+cost.ToString()+":"+damage.ToString()+":"+strength.ToString()+":"+stamina.ToString()+":"+pic;
		}else
		{
			return "0";
		}
	}
}
