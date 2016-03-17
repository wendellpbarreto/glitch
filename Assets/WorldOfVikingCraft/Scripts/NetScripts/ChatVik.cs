/*
Ultimate MMORPG Kit - NetScripts/ChatVik - Client's script - skeletarik@gmail.com - 2013
This script is for chat. It uses [RPC] function for synchronize all messages.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatVik : Photon.MonoBehaviour
{
	public GUISkin finalSkin;								//Good-looking GUISkin
	public static ChatVik SP;	
    public List<string> messages = new List<string>();
    private int chatHeight = (int)140;
    private Vector2 scrollPos = Vector2.zero;
    private string chatInput = "";
    private float lastUnfocusTime = 0;

    void Awake()
    {
        SP = this;
    }

    void OnGUI()
    {        
		GUI.skin=finalSkin;
		GUI.Box(new Rect(-15, Screen.height - chatHeight-40, 240, chatHeight+50), "", "Window2");
        GUI.SetNextControlName("");
        GUILayout.BeginArea(new Rect(5, Screen.height - chatHeight-10, 200, chatHeight));
        
		//Show scroll list of chat messages
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUI.color = Color.red;
        for (int i = 0; i <= messages.Count - 1; i++)						//Display all messages
        {
            GUILayout.Label(messages[i], "PlainText");
        }
        GUILayout.EndScrollView();
        GUI.color = Color.white;
		
        //Chat input
        GUILayout.BeginHorizontal(); 
        GUI.SetNextControlName("ChatField");
   		chatInput = GUILayout.TextField(chatInput, GUILayout.Width(200));	//Field for input
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n'){
            if (GUI.GetNameOfFocusedControl() == "ChatField")
            {                
                SendChat(PhotonTargets.All);
                lastUnfocusTime = Time.time;
                GUI.FocusControl("");
                GUI.UnfocusWindow();
            }
            else
            {
                if (lastUnfocusTime < Time.time - 0.1f)
                {
                    GUI.FocusControl("ChatField");
                }
            }
		}
      	GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public static void AddMessage(string text)
    {
        SP.messages.Add(text);
        if (SP.messages.Count > 15)
            SP.messages.RemoveAt(0);
    }

    [RPC]
    void SendChatMessage(string text, PhotonMessageInfo info)
    {
        AddMessage("[" + info.sender + "] " + text);
		scrollPos.y = chatHeight*3;								//Scroll chat's slide to the latest message
    }

    void SendChat(PhotonTargets target)
    {
        if (chatInput != "")
        {
            photonView.RPC("SendChatMessage", target, chatInput);
            chatInput = "";
        }
    }

    void SendChat(PhotonPlayer target)
    {
        if (chatInput != "")
        {
            chatInput = "[PM] " + chatInput;
            photonView.RPC("SendChatMessage", target, chatInput);
            chatInput = "";
        }
    }

    void OnLeftRoom()
    {
        this.enabled = false;
    }

    void OnJoinedRoom()
    {
        this.enabled = true;
    }
    void OnCreatedRoom()
    {
        this.enabled = true;
    }
}
