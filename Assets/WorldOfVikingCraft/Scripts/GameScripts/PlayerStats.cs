/*
Ultimate MMORPG Kit - GameScripts/PlayerStats - Client's script - skeletarik@gmail.com - 2013
This script is for displaying Player stats for other players. For correct work set two GUISkins and Player's photo.
*/

using UnityEngine;
using System.Collections;

public class PlayerStats : Photon.MonoBehaviour {

	public GUISkin g;			//Default skin
	public GUISkin finalSkin;	//Good-looking skin

	string npc_name;			//Player's name
	float max_health;			//Player's health
	float max_energy;			//Player's energy (like mana)
	int level;					//Player's level. 
	public Texture2D photo; 	//Player's photo.
	float cur_health;			//Player's current health
	float cur_energy;			//Player's current energy
	bool active_npc;			//We don't need to show it when we are not talking with that Player
	bool mous;
	bool started;
	public bool enemy;			//If false - Player is our friend, if true - NPC is our enemy
	bool clicked;
	bool inTrig;
	bool deadAnim;
	string sendS;

	void Start(){
		cur_health = max_health;
		cur_energy = max_energy;
	}
	
	void SendQuestHealth(){
		GetComponent("QuestItem_kill").SendMessage("QuestHealth", cur_health);
	}

	void Update(){
		if(!PhotonNetwork.insideLobby && !started){		//Taking values from PhotonPlayer properties
			max_health = int.Parse(PhotonPlayer.Find(photonView.ownerId).customProperties["Health"].ToString());
			max_energy = int.Parse(PhotonPlayer.Find(photonView.ownerId).customProperties["Energy"].ToString());
			npc_name = photonView.name.Split(':')[0];
			level = int.Parse(PhotonPlayer.Find(photonView.ownerId).customProperties["Level"].ToString());			
			cur_health = max_health;
			cur_energy = max_energy;
			photonView.RPC("UpdateStatsStart", PhotonTargets.MasterClient);
			started=true;
		}
		if(cur_health>max_health){
			cur_health=max_health;	
		}
		if(cur_energy>max_energy){
			cur_energy=max_energy;	
		}	
		if (Input.GetKeyDown(KeyCode.Mouse0) && mous)	//If Player presses right-mouse button, the script will show/close other Player's stats
        {
			if(clicked==false){
				active_npc=true;
				clicked=true;
			}else{
				active_npc=false;	
				clicked=false;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Escape))			//If Player presses <Escape> button, the script will close other Player's stats
		{
			active_npc=false;
		}	
	}
	
	void LateUpdate(){
		cur_health = int.Parse(PhotonPlayer.Find(photonView.ownerId).customProperties["Health"].ToString());
		cur_energy = int.Parse(PhotonPlayer.Find(photonView.ownerId).customProperties["Energy"].ToString());
		npc_name = photonView.name.Split(':')[0];
		level = int.Parse(PhotonPlayer.Find(photonView.ownerId).customProperties["Level"].ToString());
				
	}
	
	void OnGUI(){
		GUI.skin=g;
		float width = 297/1.5f-6;
		if(active_npc==true){
			GUI.skin=finalSkin;
			GUI.Box(new Rect(307,-18,350,113),"", "Window2");
			GUI.skin=g;
			GUI.DrawTexture(new Rect(42+297,10,297/4,47), photo);
			GUI.Label(new Rect(42+297,10+47,297,30), npc_name,"Name");
			GUI.Label(new Rect(42+297,10+47,297-10,30),"Level "+level.ToString(),"Level");
			//Health
			GUI.Box(new Rect(52+297+297/4, 10,297/1.5f,57/3),"");
			GUI.Box(new Rect(52+297+297/4+3, 10+3, width/100*(cur_health/(max_health/100)),57/3-6),"");
			GUI.Label(new Rect(52+297+297/4+3, 10+3,297/1.5f-6,57/3-6), cur_health.ToString()+"/"+max_health.ToString());
			//Energy
			GUI.Box(new Rect(52+297+297/4, 15+57/3,297/1.5f,57/3),"");
			GUI.Box(new Rect(52+297+297/4+3, 15+3+57/3, width/100*(cur_energy/(max_energy/100)),57/3-6),"");
			GUI.Label(new Rect(52+297+297/4+3, 15+3+57/3,297/1.5f-6,57/3-6), cur_energy.ToString()+"/"+max_energy.ToString());
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
	
	[RPC]
	void UpdateStats(string he){
		cur_health=float.Parse(he.Split(':')[0]);
		cur_energy=float.Parse(he.Split(':')[1]);
	}
	
	[RPC]
	void UpdateStatsStart(){
		photonView.RPC("UpdateStats", PhotonTargets.All, cur_health.ToString()+":"+cur_energy.ToString());
	}
}
