/*
Ultimate MMORPG Kit - GameScripts/ExpWindow - Client's script - skeletarik@gmail.com - 2013
This script is just show an experience bar. You can set your own bar or its filling by changing <expWindow> and <expIn> in the inspector.
*/

using UnityEngine;
using System.Collections;

public class ExpWindow : MonoBehaviour {

	public Texture2D expWindow;				//Texture for an experience bar
	public Texture2D expIn;					//Texture for bar's filling
	bool started;							//Is the game started? The script needs to display an experience bar only if its already started.
	
	public void GameStartedExp(){
		started=true;						//Receive information about the game's beginning from the "INFO" script
	}
	
	void OnGUI(){
		if(started){						//If game started
			GUI.DrawTexture(new Rect(Screen.width/3-20,Screen.height-10,(Screen.width/3)*(INFO.ReturnExp()/(float)(INFO.ReturnLevel()*100)), 20), expIn);	//Draw bar
			GUI.DrawTexture(new Rect(Screen.width/3-20,Screen.height-10,Screen.width/3, 20), expWindow);													//Draw filling
		}
	}
}
