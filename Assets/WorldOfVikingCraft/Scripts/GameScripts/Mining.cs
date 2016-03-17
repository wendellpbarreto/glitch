/*
Ultimate MMORPG Kit - GameScripts/Mining - Client's script - skeletarik@gmail.com - 2013
This script is for gathering ore (or any other thing). Player needs to have the "Pick Axe" in the left or the right hand.
In other way nothing will happen. 
*/

using UnityEngine;
using System.Collections;

public class Mining : MonoBehaviour {
	
	bool entered;						//Does Player enter the Rock's trigger?
	bool mous;							//Is cursor hovering over the Rock?
	bool oneTime;						//Player can gather the ore from one Rock only one time.
	public AnimationClip miningAnim;	//Mining animation
	GameObject hero;					//Player
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			if(INFO.ReturnShield()=="Pick Axe" || INFO.ReturnWeapon()=="Pick Axe"){
				entered=true;
				hero=other.gameObject;
			}
		}
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(1) && mous && entered && !oneTime){			//Only if the Player click on the right-mouse button, the cursor is hovering over the Rock, 
			oneTime=true;														//the Player enters the Rock's trigger and he tries	the first time, he will be able to gather the ore.
			GameObject.Find(hero.name+"/Viking").GetComponent<Animation>().clip=miningAnim;
			GameObject.Find(hero.name+"/Viking").GetComponent<Animation>().Play();
			GetComponent("RockScript").SendMessage("ServerSend");
			GetComponent("Drop").SendMessage("MineDrop");
		}
	}
	
	
	void OnMouseEnter()
    {
        mous = true;
    }
    void OnMouseExit()
    {
        mous = false;
    }
}
