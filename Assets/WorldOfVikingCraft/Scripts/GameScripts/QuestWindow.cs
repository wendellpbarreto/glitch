/*
Ultimate MMORPG Kit - GameScripts/QuestWindow - Client's script - skeletarik@gmail.com - 2013
This script is for displaying Player's quests. All you need is to set two GUISkins.
*/

using UnityEngine;
using System.Collections;

public class QuestWindow : MonoBehaviour {

	bool show;										//Is window opened?
	public GUISkin finalSkin;						//Good-looking GUISkin
	public GUISkin defaultSkin;						//Default GUISkin
	string [] quests;								//Player's quests
	bool started;									//Is the game started?
	float height;	
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.L)){			//If Player presses the <L> button, quests' window will be opened/closed
			if(show==false){
				show=true;	
			}else{
				show=false;	
			}
		}
		quests = INFO.ReturnQuests().Split('&');	//Receive Player's quests from the "INFO" script
	}
	
	void GameStartedQW(string q){					//When the game is just started
		started=true;
		quests = q.Split('&');
	}
	
	void OnGUI(){
		if(started){
			GUI.skin=finalSkin;						
			if(GUI.Button(new Rect(Screen.width-270,Screen.height-18,100,22), "Quests")){		//If Player presses the "Quests" button, the quests' window will be opened/closed
				if(show==false){
					show=true;	
				}else{
					show=false;	
				}
			}
			if(show){						
				GUI.Window(40, new Rect(0, Screen.height/6,300,350), questWindow, "");			//If window is opened
			}
		}
	}
	
	void questWindow(int id){
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		GUILayout.Label("Quest Window");
		GUILayout.Space(10);
		height=135;
		foreach(string quest in quests){
			if(quest.Length>3){
				if(quest.Split('$')[1]=="making"){
					GUI.Box(new Rect(45, height,210,40), "");
					GUI.skin = defaultSkin;
					GUI.contentColor = new Color(0.25f,0.25f,0.25f);
					GUI.Label(new Rect(45, height, 210,40), quest.Split('$')[0], "center");
					GUI.contentColor = Color.white;
					GUI.skin = finalSkin;
					height+=45;
				}
			}
		}
		GUILayout.EndVertical();
	}
}
