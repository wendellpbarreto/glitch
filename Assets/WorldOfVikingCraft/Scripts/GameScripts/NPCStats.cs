/*
Ultimate MMORPG Kit - GameScripts/NPCStats - Client's script - skeletarik@gmail.com - 2013
This script is for displaying and storing NPC's stats. You can set them in the inspector.
Read the Documentation for more details. 

NB! It is the client's version of the script. The Server's version of the script also exists. You can find it in the Server folders.
*/

using UnityEngine;
using System.Collections;

public class NPCStats : Photon.MonoBehaviour {

	public GUISkin g;				//Default GUISKin
	public GUISkin finalSkin;		//Good-looking GUISkin
	GameObject greenP;				//Green particles (for friendly NPCs)
	GameObject redP;				//Red particles (for "bad" NPCs)

	string npc_name;			//NPC's name
	float max_health;		//NPC's health
	float max_energy;		//NPC's energy (like mana)
	int level;				//NPC's level. 
	public Texture2D photo; 		//NPC's photo.
	float cur_health;		//NPC's current health
	float cur_energy;		//NPC's current energy
	public AnimationClip die_anim;	//Animation for die
	public int secondsToDestroy;	//After NPC's death it will be destroyed in ... seconds.
	GameObject playerGo;			//Auto-detecting NPC's target
	bool active_npc;				//We don't need to show it when we are not talking with that NPC
	bool mous;
	bool started;					//Is game started?
	bool enemy;				//If false - NPC is our friend, if true - NPC is our enemy
	bool createdP;
	bool clicked;
	bool inTrig;
	bool dead;
	bool deadAnim;
	string sendS;

	void Start(){
		createdP=false;
		//cur_health = max_health;
		//cur_energy = max_energy;
		redP = GameObject.Find(gameObject.name+"/RedNPCParticles");			//If NPC is an enemy, red particles will appear
		if(redP!=null){
			redP.SetActive(false);
		}
		greenP = GameObject.Find(gameObject.name+"/GreenNPCParticles");		//If NPC is a friend, green particles will appear
		if(greenP!=null){
			greenP.SetActive(false);
		}
		if(gameObject.tag=="NPC"){
			gameObject.name = gameObject.name+":NPC";
		}
		photonView.RPC("UpdateNow", PhotonTargets.MasterClient);
	}
	
	void SendQuestHealth(){
		GetComponent("QuestItem_kill").SendMessage("QuestHealth", cur_health);
	}

	
	void Update(){
		if(!PhotonNetwork.insideLobby && !started){
			cur_health = max_health;
			cur_energy = max_energy;
			//photonView.RPC("UpdateStatsStart", PhotonTargets.MasterClient);
			
		}
		if(cur_health>max_health){
			cur_health=max_health;	
		}
		if(cur_health<=0 && started){
			dead=true;
			cur_health=0;
			if(deadAnim==false){
				Die();
			}
		}
		if(cur_energy>max_energy){
			cur_energy=max_energy;	
		}	
		if (Input.GetKeyDown(KeyCode.Mouse0) && mous)		//The script will display NPC's stats window, if Player presses left-mouse button.
        {
			if(!dead){
				if(clicked==false){
					active_npc=true;
					if(createdP==false){
						if(enemy==false){
							greenP.SetActive(true);
						}else{
							redP.SetActive(true);
						}
					}
					clicked=true;
				}else{
					active_npc=false;
					greenP.SetActive(false);
					redP.SetActive(false);	
					clicked=false;
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Escape))				//If Player presses <Escape>, all NPCs' stats window will disappeared
		{
			active_npc=false;
			if(enemy==false){
				greenP.SetActive(false);
			}else{
				redP.SetActive(false);
			}
			
		}	
		
		//If we use abilities
		if(active_npc && enemy && inTrig && !dead){
			/*if(Input.GetKeyDown(KeyCode.Alpha1)){			//First ability
				if(INFO.ReturnValue("ability1")==0){
					if(float.Parse(PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Energy"].ToString())>=5){
						if(INFO.ReturnWeapon()!="None"){
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[3]);
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[4]);
							if(INFO.ReturnShield()!="None"){
								cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[4]);
							}
						}else
						if(INFO.ReturnShield()!="None"){
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[3]);
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[4]);
							if(INFO.ReturnWeapon()!="None"){
								cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[4]);
							}
						}else{
							cur_health-=0;	
						}
						Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Game_race"] }, 
						{ "Game_class", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Game_class"] }, 
						{ "Level", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Level"] }, 
						{ "Health", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Health"]}, 
						{ "Energy", float.Parse(PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Energy"].ToString())- 0 } };
						PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).SetCustomProperties(prop);
						photonView.RPC("UpdateStats", PhotonTargets.All, cur_health.ToString()+":"+cur_energy.ToString());
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("Cooldown", "ability1");
					}
				}
			}*/
			/*if(Input.GetKeyDown(KeyCode.Alpha2)){				//Second ability
				if(INFO.ReturnValue("ability2")==0){
					if(float.Parse(PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Energy"].ToString())>=10){
					
						if(INFO.ReturnWeapon()!="None"){
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[3]);
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[4]);
							if(INFO.ReturnShield()!="None"){
								cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[4]);
							}
						}
						if(INFO.ReturnShield()!="None"){
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[3]);
							cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[4]);
							if(INFO.ReturnWeapon()!="None"){
								cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[4]);
							}
						}else{
							cur_health-=0;	
						}
						
						Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Game_race"] }, 
						{ "Game_class", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Game_class"] }, 
						{ "Level", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Level"] }, 
						{ "Health", PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Health"]}, 
						{ "Energy", float.Parse(PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).customProperties["Energy"].ToString())- 0} };
						PhotonPlayer.Find(playerGo.gameObject.GetPhotonView().ownerId).SetCustomProperties(prop);
						photonView.RPC("UpdateStats", PhotonTargets.All, cur_health.ToString()+":"+cur_energy.ToString());
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("Cooldown", "ability2");
					}
				}
			}*/
			//You can add your own abilities
		}
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
	
	void OnTriggerEnter(Collider other)										
    {
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			inTrig=true;
		}
		if(other.gameObject.name.Split(':')[1]=="player"){
			playerGo = other.gameObject;
		}
		if(other.gameObject.name.Split(':')[0]=="childSpell" && enemy && other.gameObject.name.Split(':')[1]!="collider"){
			cur_health-=float.Parse(SpellBook.fitLvl[int.Parse(other.gameObject.name.Split(':')[1])].Split(';')[1])/2;
			if(INFO.ReturnWeapon()!="None"){
				cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[3]);
				cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[4]);
			}
			if(INFO.ReturnShield()!="None"){
				cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[3]);
				cur_health-=int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[4]);
			}
			photonView.RPC("UpdateStats", PhotonTargets.All, cur_health.ToString()+":"+cur_energy.ToString());
			//GameObject.Find("INFO").GetComponent("INFO").SendMessage("Cooldown", "ability1");
		}
	}
	
	void OnTriggerExit(Collider other)												//If Player exits from NPC's trigger, the NPC's stats window will be closed
    {
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			inTrig=false;
        	active_npc=false;	
			if(enemy==false){	
				greenP.SetActive(false);
			}else{
				redP.SetActive(false);
			}
		}
		if(playerGo!=null){
			if(other.gameObject.name == playerGo.name){
				playerGo = null;
			}
		}
    }
	
	void Die(){
		gameObject.GetComponent<Animation>().clip = die_anim;
		gameObject.GetComponent<Animation>().Play();
		deadAnim=true;
		active_npc=false;	
		clicked=true;
		if(enemy==false){
			greenP.SetActive(false);
		}else{
			redP.SetActive(false);
		}
		GetComponent<Collider>().enabled=false;
		StartCoroutine("destroyIt", secondsToDestroy);
		photonView.RPC("DropInServer", PhotonTargets.MasterClient);		
	}
	
	IEnumerator destroyIt(int sec){											//Destroy NPC after <sec> seconds
		Destroy(gameObject.GetComponent<Rigidbody>());
		yield return new WaitForSeconds(sec);
		photonView.RPC("DestroyInServer", PhotonTargets.MasterClient);
						
	}
	
	[RPC]
	void DestroyInServer(){
		PhotonNetwork.Destroy(gameObject);
	}
	
	[RPC]
	void DropInServer(){
		GetComponent("Drop").SendMessage("Dropping");
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
	
	[RPC]
	void UpdateNow(){
	
	}
	
	[RPC]
	void UpdateAllStats(string he){
		cur_health=float.Parse(he.Split(':')[0]);
		cur_energy=float.Parse(he.Split(':')[1]);
		max_health=float.Parse(he.Split(':')[2]);
		max_energy=float.Parse(he.Split(':')[3]);
		level=int.Parse(he.Split(':')[4]);
	}
	
	[RPC]
	void SyncAllStats(string ali){
		string perInfo = ali.Split('*')[0];
		cur_health = float.Parse(ali.Split('*')[1]);
		cur_energy = float.Parse(ali.Split('*')[2]);
		max_health=int.Parse(perInfo.Split(';')[1]);
		max_energy=int.Parse(perInfo.Split(';')[2]);
		level=int.Parse(perInfo.Split(';')[3]);
		enemy=IntToBool(int.Parse(perInfo.Split(';')[4]));
		npc_name=perInfo.Split('|')[0].ToString().Split(';')[7];
		started=true;
	}
	
	int BoolToInt(bool b){
		if(b==true){
			return 1;	
		}else{
			return 0;	
		}
	}
	
	bool IntToBool(int b){
		if(b==0){
			return false;	
		}else{
			return true;	
		}
	}
}
