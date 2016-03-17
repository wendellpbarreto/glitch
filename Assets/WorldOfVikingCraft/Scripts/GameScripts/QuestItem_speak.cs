/*
Ultimate MMORPG Kit - GameScripts/QuestItem_speak - Client's script - skeletarik@gmail.com - 2013
This script is practically a part of the "Quest" script. If you want to make a speaking quest,
add this script for each GameObject in the Scene that needs to be spoken. The GameObject must have Collider.
For correct work fill <QuestNPCName> field. You need to write the name of the NPC that gives the speaking quest.
For more detailed instruction read the Documentation.
*/

using UnityEngine;
using System.Collections;

public class QuestItem_speak : MonoBehaviour {

	bool spoken;										//Has Player spoken with this creature?
	public string QuestNPCName;							//NPC's name who gives necessary quest (e.g. "NPC_MaxFree")
	public string Text;									//Creature's text
	bool isActive;										//Is window opened?
	bool mous;											//Is the cursor hovering the creature?
	public GUISkin g;									//Good-looking GUISkin
	
	void Update(){
		if(!spoken){
			if(Input.GetMouseButtonDown(1) && mous){	//If Player presses the right-mouse button, the dialog's window will be opened/closed
				if(isActive){
					isActive=false;
				}else
				if(!isActive){
					isActive=true;
				}
			}
		}
	}
	
	public void Open(){									//Open the window
		isActive=true;
	}
	
	public void Close(){								//Close the window
		isActive=false;
	}
	
	void OnGUI(){
		GUI.skin = g;
		if(isActive){									//If window is opened
			GUI.Window(100, new Rect(0,Screen.height/6,300,350), dialogWindow, "");
		}
	}
	
	void dialogWindow(int id){
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		GUILayout.Label (Text, "PlainText");			//Creature's text (<Text>)
		if(GUILayout.Button("Ok, bye", "Box")){
			spoken=true;
			Close();
			GameObject.Find(QuestNPCName+"(Clone):NPC").GetComponent("Quest").SendMessage("PlusNum");
		}
		GUILayout.EndVertical();
	}
	
	void OnMouseEnter()
    {
        mous = true;
    }
    void OnMouseExit()
    {
        mous = false;
    }
	
	void OnTriggerExit(Collider other){					//If Player exits from the creature's trigger, the window will be closed
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			isActive=false;
			Close();
		}
	}
}
