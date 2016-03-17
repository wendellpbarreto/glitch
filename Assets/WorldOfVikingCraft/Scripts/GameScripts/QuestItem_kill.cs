/*
Ultimate MMORPG Kit - GameScripts/QuestItem_kill - Client's script - skeletarik@gmail.com - 2013
This script is practically a part of the "Quest" script. If you want to make a killing quest,
add this script for each GameObject in the Scene that needs to be killed. The GameObject must have Collider.
For correct work fill <QuestNPCName> field. You need to write the name of the NPC that gives the killing quest.
For more detailed instruction read the Documentation.
*/

using UnityEngine;
using System.Collections;

public class QuestItem_kill : MonoBehaviour {

	bool killed;								//Is the creature killed?
	public string QuestNPCName;					//NPC's name who gives necessary quest (e.g. "NPC_HarryBrain")
	float q_health;								//Creature's health
	
	void Update(){
		if(!killed){
			GetComponent("NPCStats").SendMessage("SendQuestHealth");	
			if(q_health<=0){
				killed=true;
				GameObject.Find(QuestNPCName+"(Clone):NPC").GetComponent("Quest").SendMessage("PlusNum");
			}
		}
	}
	
	public void QuestHealth(float c){
		q_health=c;
	}
}
