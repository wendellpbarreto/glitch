/*
Ultimate MMORPG Kit - GameScripts/Dialog - Client's script - skeletarik@gmail.com - 2013
This script is for making dialogs with NPCs. In the inspector type NPC's text in the <Text> field.
You can make a dialog with a few choices. For that, type in the element of <Choice> any text you want and in the element of <WhatToDo>
an action that will happened. Indexes of elements must be equal. Look through the Documentation for more detailed instruction. 
Possible variants of action: 
1) Close (it will close the dialog); 
2) Vendor (it will open the Vendor's window. NPC must have the "Vendor" script).
3) Quest (it will open the Quest's window. NPC must have the "Quest" script).
4) CraftLearn (it will open the window of learning profession. NPC must have the "CraftLearn" script).
*/

using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {
	
	bool isActive;					//Is window opened?
	bool mous;						//Is cursor hovering over the NPC?
	public string Text;				//NPC's text
	public string[] Choice;			//Possible choices
	public string[] WhatToDo;		//For each choice you need to explain what to do if this choice is chosen
	
	public GUISkin finalSkin;		//Good-looking GUISkin
	
	void Update(){
		if(Input.GetMouseButtonDown(1) && mous){	//Will open/close the dialog if Player click on the NPC
			if(isActive){
				isActive=false;
			}else
			if(!isActive){
				isActive=true;
			}
		}
	}
	
	public void Open(){								//Open a dialog
		isActive=true;
	}
	
	public void Close(){							//Close a dialog
		isActive=false;
	}
	
	void OnGUI(){
		GUI.skin = finalSkin;
		if(isActive){								//If window is opened
			GUI.Window(10, new Rect(0,Screen.height/6,300,350), dialogWindow, "");
		}
	}
	
	void dialogWindow(int id){
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		GUILayout.Label (Text, "PlainText");	//NPC's text
		int i=0;
		foreach(string c in Choice){			//For each choice
			GUILayout.Space(5);
			if(GUILayout.Button(c, "Box")){
				if(WhatToDo[i]=="Close"){		//If it is "Close"
					isActive=false;	
				}
				if(WhatToDo[i]=="Vendor"){		//If it is "Vendor"
					isActive=false;
					GetComponent("Vendor").SendMessage("OpenVendor");
				}
				if(WhatToDo[i]=="Quest"){		//If it is "Quest"
					isActive=false;
					GetComponent("Quest").SendMessage("OpenQ");
				}
				if(WhatToDo[i]=="CraftLearn"){	//If it is "CraftLearn"
					isActive=false;
					GetComponent("CraftLearn").SendMessage("OpenLearn");
				}
			}
			i++;
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
	
	void OnTriggerExit(Collider other){			//If the Player exits from the NPC's trigger, the dialog will be ended. You can edit the radius of trigger in the inspector
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			isActive=false;
			Close();
		}
	}
}
