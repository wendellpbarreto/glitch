/*
Ultimate MMORPG Kit - MenuScripts/ServerConnect - Client's script - skeletarik@gmail.com - 2013
This script is using in the "ChooseServer" scene. It shows all Servers. If there is no Server, it will show the label.
*/

using UnityEngine;
using System.Collections;

public class ServerConnect : Photon.MonoBehaviour
{
	public GUISkin g; 				//Default GUISkin
	public GUISkin finalSkin;		//Good-looking GUISkin
	
	void Awake()
    {
		if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); 
        PhotonNetwork.playerName = INFO.ReturnInfo().Split(':')[0];
    }
	
    void OnGUI()
    {
		GUI.skin=finalSkin;
		
        if (!PhotonNetwork.connected)
        {
            ShowConnectingGUI();
            return;  					 //Wait for a connection
        }

        if (PhotonNetwork.room != null)
            return; 
		
		GUI.Box(new Rect((Screen.width - 400) / 2-20, -25, 440, Screen.height),"", "Window");
		if(GUI.Button(new Rect((Screen.width - 400) / 2, Screen.height-30, 400, 25), "Back")){
			Application.LoadLevel("MainMenu");	
		}
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("..no Servers available..", "LargeTextCenter");
        }
        else
        {
            //Server listing
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(game.name,GUILayout.Height(30)))
                {
					Application.LoadLevel("World");
                    PhotonNetwork.JoinRoom(game.name);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndArea();
    }

    void ShowConnectingGUI(){}
}
