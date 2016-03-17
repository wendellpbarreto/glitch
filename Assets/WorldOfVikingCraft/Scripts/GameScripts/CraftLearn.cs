/*
Ultimate MMORPG Kit - GameScripts/CraftLearn - Client's script - skeletarik@gmail.com - 2013
This script is for learning professions. Simply drag-and-drop this script to the NPC that will teach that. NPC must have "Dialog" script.
For correct work set two GUISkins and a profession name. For example, Blacksmithing.
*/

using UnityEngine;
using System.Collections;

public class CraftLearn : Photon.MonoBehaviour {

	bool isActive;						//Is window opened?				
	public GUISkin finalSkin;			//Good-looking skin
	public GUISkin defaultSkin;			//Default skin
	public string professionName;		//A profession name that will be learnt
	int page=0;							//Pages in the window
	
	public void OpenLearn(){			//Open the window
		isActive=true;	
	}
	public void CloseLearn(){			//Close the window
		isActive=false;	
	}
	
	void OnGUI(){
		GUI.skin = finalSkin;
		if(isActive){					//If window is opened
			GUI.Window(10, new Rect(0,Screen.height/6,300,350), learnWindow, "");	
		}
	}
	
	void learnWindow(int id){
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		if(page==0){		//First page
			GUILayout.Label("Do you really want to learn '"+professionName+"'?", "PlainText");		//Player need to confirm his choice
			GUILayout.Space(20);
			if(GUILayout.Button("Yes", "Box")){
				page=1;
				GameObject.Find("WEB_Prof").GetComponent("ProfWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+professionName+"%0");
				GameObject.Find(PhotonNetwork.player.name+":player/Main Camera").GetComponent("Profession").SendMessage("UpdateProf", professionName+"%0");	
			}
			if(GUILayout.Button("No", "Box")){
				CloseLearn();	
			}
		}
		if(page==1){		//Second page
			GUILayout.Label("You have learnt "+professionName+" successfully!","PlainText");		//Player successfully learnt the profession
			GUILayout.Space(20);
			if(GUILayout.Button("Thanks!", "Box")){
				CloseLearn();
			}
		}
		GUILayout.EndVertical();
	}
}
