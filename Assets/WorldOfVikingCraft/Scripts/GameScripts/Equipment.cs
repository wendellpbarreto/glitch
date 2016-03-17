/*
Ultimate MMORPG Kit - GameScripts/Equipment - Client's script - skeletarik@gmail.com - 2013
This script is for equipment's window. It shows Player's weapon and shield. Also it contains Player's name, level, max health, strength and others.
For correct work set 2 GUISkins and Player's picture.
*/

using UnityEngine;
using System.Collections;

public class Equipment : Photon.MonoBehaviour {
	
	public GUISkin finalSkin;			//Good-looking skin
	public GUISkin defaultSkin;			//Default skin
	bool show;							//Is window opened?
	string weapon;						//Player's weapon
	string shield;						//Player's shield
	string race;						//Player's race
	int level;							//Player's level
	string game_class;					//Player's class
	bool started;						//Is game started?
	public Texture2D pic;				//Player's picture
	
	float leafOffset;
	float frameOffset;
	float skullOffset;
	float WSwaxOffsetX;
	float WSwaxOffsetY;
	float WSribbonOffsetX;
	float WSribbonOffsetY;
	
	void GameStartedEq(string s){				//Receive information about Player from the "INFO" script
		started=true;
		game_class=s.Split(':')[0];
		level=int.Parse(s.Split(':')[1]);
		weapon=s.Split(':')[2];
		shield=s.Split(':')[3];
		race=s.Split(':')[4];							
	}
	
	[RPC]
	public void ChangeCharEq(string c){			//If Player change himself (e.g. equip weapon), it will be seen by all other players
		if(c.Split(':')[1]=="weapon"){
			GameObject.Find(c.Split(':')[0]+":player/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigLArmCollarbone/CATRigLArm1/CATRigLArm2/CATRigLArmPalm/"+c.Split(':')[2]).GetComponent<Renderer>().enabled = false;
		}
		if(c.Split(':')[1]=="shield"){
			GameObject.Find(c.Split(':')[0]+":player/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigRArmCollarbone/CATRigRArm1/CATRigRArm2/CATRigRArmPalm/"+c.Split(':')[2]).GetComponent<Renderer>().enabled = false;
		}
	}
	
	void Equip(string what){					//If Player equip new thing, script will update information about him
		if(what.Split(':')[0]=="right"){
			weapon=what.Split(':')[1];
			INFO.SetShield(weapon);
			GetComponent("HeroStats").SendMessage("UpdateHealthStat","");
		}
		if(what.Split(':')[0]=="left"){
			shield=what.Split(':')[1];
			INFO.SetWeapon(shield);
			GetComponent("HeroStats").SendMessage("UpdateHealthStat", "");
		}
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.C)){		//Equipment window will appear if Player presses the <C> button
			if(show==false){
				show=true;	
			}else{
				show=false;	
			}
		}
	}
	
	void OnGUI(){
		GUI.skin=finalSkin;
		if(started==true){
			if(GUI.Button(new Rect(Screen.width-180,Screen.height-18,120,22), "Character")){		//Equipment window will also appear if Player presses the "Character" button
				if(show==false){
					show=true;	
				}else{
					show=false;	
				}
			}
			if(show){			//If window is opened
				GUI.Window(30, new Rect(0, Screen.height/6,300,350), equipWindow, "");	
			}
		}
	}
	
	void equipWindow(int id){
		FancyTop(300);
		GUI.skin=defaultSkin;
		GUI.Box(new Rect(50,90,63,63), pic,"Texture");								//Draw Player's picture
		GUI.skin=finalSkin;
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		GUILayout.BeginHorizontal();
			GUILayout.Space(80);
			GUILayout.Label(PhotonNetwork.player.name, "OutlineText");				//Display Player's name
			GUILayout.Space(20);
			GUILayout.Label("Level: "+level.ToString(),"PlainText");				//Display Player's level
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
			GUILayout.Space(80);
			GUILayout.Label(game_class, "PlainText");								//Display Player's class
			GUILayout.Space(20);
			GUILayout.Label(race,"PlainText");										//Display Player's race
		GUILayout.EndHorizontal();
		GUILayout.Space(25);
		GUILayout.BeginHorizontal();
			GUILayout.Label("Health: "+(level*100).ToString(), "PlainText");		//Display Player's health
			GUILayout.Space(25);
			GUILayout.Label("\tEnergy: "+(level*50).ToString(), "PlainText");		//Display Player's energy
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
			int strength=0;
			if(INFO.ReturnWeapon()!="None"){
				strength = strength + int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[4]);	
			}
			if(INFO.ReturnShield()!="None"){
				strength = strength + int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[4]);
			}
			GUILayout.Label("Strength: "+strength.ToString(), "PlainText");			//Display Player's strength
			GUILayout.Space(25);
			int stamina=0;
			if(INFO.ReturnWeapon()!="None"){
				stamina = stamina + int.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[5]);
			}
			if(INFO.ReturnShield()!="None"){
				stamina = stamina + int.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[5]);
			}
			GUILayout.Label("Stamina: "+stamina.ToString(), "PlainText");			//Display Player's stamina
		GUILayout.EndHorizontal();
		GUILayout.Space(25);
		GUILayout.BeginHorizontal();
			GUILayout.Space(20);
			GUI.skin=defaultSkin;
			if(weapon=="None"){
				GUILayout.Box("Left\nHand", GUILayout.Width(40), GUILayout.Height(40));		//Box for left hand
			}else{
				if(GUILayout.Button((Texture2D)Resources.Load("Items/"+Weapons.ReturnWeapon(weapon).Split(':')[6], typeof(Texture2D)), "Texture", GUILayout.Width(40), GUILayout.Height(40))){
					if(Bag.ReturnI()<11){
						Bag.PlusI(Weapons.ReturnWeapon(weapon).Split(':')[0]+"!"+Weapons.ReturnWeapon(weapon).Split(':')[1]+"!Weapon!"+Weapons.ReturnWeapon(weapon).Split(':')[2]+"!"+Weapons.ReturnWeapon(weapon).Split(':')[3]+"!"+Weapons.ReturnWeapon(weapon).Split(':')[4]+"!"+Weapons.ReturnWeapon(weapon).Split(':')[5]+"!"+Weapons.ReturnWeapon(weapon).Split(':')[6]);	
						photonView.RPC("ChangeCharEq", PhotonTargets.All, PhotonNetwork.player.name+":weapon:"+weapon);
						weapon="None";
						INFO.SetWeapon("None");
						GetComponent("HeroStats").SendMessage("UpdateHealthStat", "");
						GameObject.Find("WEB_Equip").GetComponent("EquipWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^right^None");
					}else{
						Debug.LogWarning("Ultimate MMORPG warning: Hero hasn't any empty slots in his bag!");
					}
				}
			}
			GUILayout.Space(20);
			if(shield=="None"){
				GUILayout.Box("Right\nHand", GUILayout.Width(40), GUILayout.Height(40));	//Box for right hand
			}else{
				if(GUILayout.Button((Texture2D)Resources.Load("Items/"+Weapons.ReturnWeapon(shield).Split(':')[6], typeof(Texture2D)), "Texture", GUILayout.Width(40), GUILayout.Height(40))){
					if(Bag.ReturnI()<11){
						Bag.PlusI(Weapons.ReturnWeapon(shield).Split(':')[0]+"!"+Weapons.ReturnWeapon(shield).Split(':')[1]+"!Weapon!"+Weapons.ReturnWeapon(shield).Split(':')[2]+"!"+Weapons.ReturnWeapon(shield).Split(':')[3]+"!"+Weapons.ReturnWeapon(shield).Split(':')[4]+"!"+Weapons.ReturnWeapon(shield).Split(':')[5]+"!"+Weapons.ReturnWeapon(shield).Split(':')[6]);	
						photonView.RPC("ChangeCharEq", PhotonTargets.All, PhotonNetwork.player.name+":shield:"+shield);
						shield="None";
						INFO.SetShield("None");
						GetComponent("HeroStats").SendMessage("UpdateHealthStat", "");
						GameObject.Find("WEB_Equip").GetComponent("EquipWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^left^None");
					}else{
						Debug.LogWarning("Ultimate MMORPG warning: Hero hasn't any empty slots in his bag!");
					}
				}
			}
			GUI.skin=finalSkin;
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		WaxSeal(300 , 350);
	}	
	
	//GUISkin functions
	void FancyTop(float topX)
	{
		leafOffset = (topX/2)-64;
		frameOffset = (topX/2)-27;
		skullOffset = (topX/2)-20;
		GUI.Label(new Rect(leafOffset, 18, 0, 0), "", "GoldLeaf");
		GUI.Label(new Rect(frameOffset, 3, 0, 0), "", "IconFrame");	
		GUI.Label(new Rect(skullOffset, 12, 0, 0), "", "Skull");
	}
	
	void WaxSeal(float x, float y)
	{
		WSwaxOffsetX = x - 120;
		WSwaxOffsetY = y - 115;
		WSribbonOffsetX = x - 114;
		WSribbonOffsetY = y - 83;
		GUI.Label(new Rect(WSribbonOffsetX, WSribbonOffsetY, 0, 0), "", "RibbonBlue");
		GUI.Label(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), "", "WaxSeal");
	}
	
}
