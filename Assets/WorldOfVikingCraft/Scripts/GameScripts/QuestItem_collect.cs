/*
Ultimate MMORPG Kit - GameScripts/QuestItem_collect - Client's script - skeletarik@gmail.com - 2013
This script is practically a part of the "Quest" script. If you want to make a collecting quest,
add this script for each GameObject in the Scene that needs to be collected. The GameObject must have Collider.
For correct work fill <QuestNPCName> field. You need to write the name of the NPC that gives the collecting quest.
For more detailed instruction read the Documentation.
*/

using UnityEngine;
using System.Collections;

public class QuestItem_collect : MonoBehaviour {

	public string QuestNPCName;						//NPC's name who gives necessary quest (e.g. "NPC_TomWest")
	bool taken;										//Is the item taken?
	bool mous;										//Is the cursor hovering over the item?
	
	void Update(){
		if(!taken){
			if(Input.GetMouseButtonDown(1) && mous){	//If Player presses the right-mouse button, the item will vanish (will be taken)
				taken=true;
				GameObject.Find(QuestNPCName+"(Clone):NPC").GetComponent("Quest").SendMessage("PlusNum");
				GetComponent<Renderer>().enabled = false;
			}
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
