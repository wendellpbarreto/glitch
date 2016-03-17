/*
Ultimate MMORPG Kit - GameScripts/HeroStats - Client's script - skeletarik@gmail.com - 2013
This script is for displaying Player's stats (picture, health, energy, level) on the screen.
Also it contains the death's function and attacks' animations. It shares the stats of this Player between other Players and enemies.
For correct work you need to set Player's picture, two GUISkins and two animations of attacking.
*/

using UnityEngine;
using System.Collections;

public class HeroStats : Photon.MonoBehaviour {

	public GUISkin g;				//Default GUISkin
	public GUISkin finalSkin;		//Good-looking GUISKin
	
	float max_health;				//Hero's health
	float max_energy;				//Hero's energy (like mana)
	int level;						//Hero's level. 
	public Texture2D photo; 		//Hero's photo.
	float cur_health;				//Hero's current health
	float cur_energy;				//Hero's current energy
	bool game_start;				//We don't need to show it when we are choosing a server
	bool show_menu;					//Fast menu
	public AnimationClip attack1;
	public AnimationClip attack2;
	GameObject hero;
	GameObject childSpell;
	bool reg;
	public static int[] spells;
	SphereCollider sc;
	public static bool[] notReady;
	int[] doubleW;
	public static bool booH;
	public static string animName;
		
	public void UpdateHealthStat(){
		max_health = level*100;				//Our formula for max health. You can edit it and create your own. 
		if(INFO.ReturnWeapon()!="None"){
			max_health+=float.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[5]);
		}
		if(INFO.ReturnShield()!="None"){
			max_health+=float.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[5]);
		}
	}
	
	void GameStarted(){
		game_start=true;
		doubleW = new int[9];
		notReady = new bool[9];
		max_health = level*100;
		if(INFO.ReturnWeapon()!="None"){
			max_health+=float.Parse(Weapons.ReturnWeapon(INFO.ReturnWeapon()).Split(':')[5]);
		}
		if(INFO.ReturnShield()!="None"){
			max_health+=float.Parse(Weapons.ReturnWeapon(INFO.ReturnShield()).Split(':')[5]);
		}
		max_energy = level*50;
		cur_health = max_health;
		cur_energy = max_energy;
		Hashtable prop = new Hashtable() 
		{ { "Game_race", INFO.ReturnInfo().Split(':')[1] }, { "Game_class", INFO.ReturnInfo().Split(':')[2] }, { "Level", INFO.ReturnInfo().Split(':')[3] }, { "Health", max_health  }, { "Energy", max_energy } };
		PhotonNetwork.player.SetCustomProperties(prop);
		spells = new int[9];
		for(int s=0; s<spells.Length;s++){
			if(PlayerPrefs.GetInt(INFO.ReturnEmail()+"_"+INFO.ReturnName()+"_Spell"+s.ToString())==0){				
				spells[s]=-1;	
			}else{
				spells[s]=PlayerPrefs.GetInt(INFO.ReturnEmail()+"_"+INFO.ReturnName()+"_Spell"+s.ToString())-1;	
			}
		}
		SpellBook.show=true;
		SpellBook.show=false;
		
		
	}
	
	void Awake(){
		if(GameObject.Find("INFO")==null){				//If there isn't any "INFO" script, loading of "MainMenu" scene begins
			Application.LoadLevel("MainMenu");
		}
		level = int.Parse(INFO.ReturnInfo().Split(':')[3]);
		
		/*There we calculate our max_health and max energy. You can invent your own formula. Or simply use your own values for each level, like this:
		if(level==1){
			max_health=130;
			max_energy=40;
		}else
		if(level==2){
			max_health=160;
			max_energy=50;
		}else...(etc.)
		*/
		
		max_health = level*100;
		max_energy = level*50;
		cur_health = max_health;
		cur_energy = max_energy;
		Hashtable prop = new Hashtable() 
		{ { "Game_race", INFO.ReturnInfo().Split(':')[1] }, { "Game_class", INFO.ReturnInfo().Split(':')[2] }, { "Level", INFO.ReturnInfo().Split(':')[3] }, { "Health", max_health  }, { "Energy", max_energy } };
		PhotonNetwork.player.SetCustomProperties(prop);
	}
	
	void Update(){
		
		if(cur_health>max_health){				//If current health is greater than max health, it will equal max health
			cur_health=max_health;	
		}
		if(cur_health<=0){						//If current health is less than 0, it will equal 0 and the Player will die
			cur_health=0;
			Die();
		}
		if(cur_energy>max_energy){				//If current energy is greater than max energy, it will equal max energy
			cur_energy=max_energy;	
		}
		if(cur_energy<0){						//If current energy is less than 0, it will equal 0
			cur_energy=0;	
		}
		if(Input.GetKeyDown(KeyCode.Escape))	//If Player presses the <Escape> button, the script will open/close the window
		{
			if(show_menu==false){
				show_menu=true;
			}else{
				show_menu=false;	
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha1) && spells[0]>=0 && !notReady[0]){
			MakeSpell(spells[0],0);	
			notReady[0]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) && spells[1]>=0 && !notReady[1]){
			MakeSpell(spells[1],1);	
			notReady[1]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3) && spells[2]>=0 && !notReady[2]){
			MakeSpell(spells[2],2);	
			notReady[2]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4) && spells[3]>=0 && !notReady[3]){
			MakeSpell(spells[3],3);	
			notReady[3]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha5) && spells[4]>=0 && !notReady[4]){
			MakeSpell(spells[4],4);	
			notReady[4]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha6) && spells[5]>=0 && !notReady[5]){
			MakeSpell(spells[5],5);	
			notReady[5]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha7) && spells[6]>=0 && !notReady[6]){
			MakeSpell(spells[6],6);	
			notReady[6]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha8) && spells[7]>=0 && !notReady[7]){
			MakeSpell(spells[7],7);	
			notReady[7]=true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha9) && spells[8]>=0 && !notReady[8]){
			MakeSpell(spells[8],8);	
			notReady[8]=true;
		}
		
	}
	
	void LateUpdate(){
		
		cur_health = float.Parse(PhotonNetwork.player.customProperties["Health"].ToString());			//The script takes values from PhotonNetwork player's properties
		cur_energy =  float.Parse(PhotonNetwork.player.customProperties["Energy"].ToString());
		if(cur_health>max_health){
			Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonNetwork.player.customProperties["Game_race"] }, 
						{ "Game_class", PhotonNetwork.player.customProperties["Game_class"] }, 
						{ "Level", PhotonNetwork.player.customProperties["Level"] }, 
						{ "Health", max_health}, 
						{ "Energy",  PhotonNetwork.player.customProperties["Energy"] } };
						PhotonNetwork.player.SetCustomProperties(prop);
		}
		if(cur_energy>max_energy){
			Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonNetwork.player.customProperties["Game_race"] }, 
						{ "Game_class", PhotonNetwork.player.customProperties["Game_class"] }, 
						{ "Level", PhotonNetwork.player.customProperties["Level"] }, 
						{ "Health", PhotonNetwork.player.customProperties["Health"]}, 
						{ "Energy", max_energy} };
						PhotonNetwork.player.SetCustomProperties(prop);
		}
	}
	
	void OnGUI(){
		GUI.skin=g;
		float width = 297/1.5f-6;
		if(game_start==true){
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////FAST MENU
			if(show_menu==true){
				GUI.skin = finalSkin;
				GUI.Box(new Rect(Screen.width/2.3f-30,Screen.height/4-50,260,320), "", "Window");
				GUI.Label(new Rect(Screen.width/2.3f+5,Screen.height/4-20,190,30), "Menu");
				if(GUI.Button(new Rect(Screen.width/2.3f+10,Screen.height/4+53,178,25),"Back to game")){
					show_menu=false;
				}
				if(GUI.Button(new Rect(Screen.width/2.3f+10,Screen.height/4+83,178,25),"Settings")){
					//You can add any settings there.
				}
				if(GUI.Button(new Rect(Screen.width/2.3f+10,Screen.height/4+113,178,25),"Exit from World")){
					PhotonNetwork.LeaveRoom();
					Application.LoadLevel("ChooseCharacter");
				}
				if(GUI.Button(new Rect(Screen.width/2.3f+10,Screen.height/4+143,178,25),"Main Menu")){
					PhotonNetwork.LeaveRoom();
					Application.LoadLevel("MainMenu");
				}
				if(GUI.Button(new Rect(Screen.width/2.3f+10,Screen.height/4+173,178,25),"Quit")){
					Application.Quit();
				}
				GUI.skin = g;
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////STATS
			GUI.skin=finalSkin;
			GUI.Box(new Rect(-13,-18,350,113),"", "Window2");
			GUI.skin=g;
			GUI.DrawTexture(new Rect(20,10,297/4,47), photo);
			GUI.Label(new Rect(20,57,297,30), INFO.ReturnInfo().Split(':')[0],"Name");
			GUI.Label(new Rect(20,57,287,30),"Level "+INFO.ReturnInfo().Split(':')[3],"Level");
			//Health
			GUI.Box(new Rect(30+297/4, 10,297/1.5f,57/3),"");
			if(cur_health<=max_health){
				GUI.Box(new Rect(30+297/4+3, 10+3, width/100*(cur_health/(max_health/100)),57/3-6),"");
			}else{
				GUI.Box(new Rect(30+297/4+3, 10+3, width,57/3-6),"");
			}
			GUI.Label(new Rect(30+297/4+3, 10+3,297/1.5f-6,57/3-6), cur_health.ToString()+"/"+max_health.ToString());
			//Energy
			GUI.Box(new Rect(30+297/4, 15+57/3,297/1.5f,57/3),"");
			GUI.Box(new Rect(30+297/4+3, 15+3+57/3, width/100*(cur_energy/(max_energy/100)),57/3-6),"");
			GUI.Label(new Rect(30+297/4+3, 15+3+57/3,297/1.5f-6,57/3-6), cur_energy.ToString()+"/"+max_energy.ToString());
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////ABILITIES
			float boxWidth=Screen.width/3;
			for(int b=0; b<9; b++){
				if(new Rect(boxWidth,Screen.height-50,35,35).Contains(new Vector2(SpellBook.dragOb.x,SpellBook.dragOb.y)) && SpellBook.show && SpellBook.dragging){
					spells[b]=SpellBook.dragS;
					SpellBook.dragging=false;
					PlayerPrefs.SetInt(INFO.ReturnEmail()+"_"+INFO.ReturnName()+"_Spell"+b.ToString(), spells[b]+1);	
					
				}
				if(spells[b]<0){
					GUI.Box(new Rect(boxWidth,Screen.height-50,35,35),"");
				}else{	
					if(SpellBook.fitLvl[spells[b]]!=null){
						if(GUI.Button(new Rect(boxWidth,Screen.height-50,35,35),(Texture2D)Resources.Load("Spells/"+SpellBook.fitLvl[spells[b]].Split(';')[6],typeof(Texture2D)),"Box") && !notReady[b]){
							MakeSpell(spells[b],b);	
							notReady[b]=true;
						}
					}else{
						GUI.Box(new Rect(boxWidth,Screen.height-50,35,35),"");
					}
				}
				GUI.Label(new Rect(boxWidth-1,Screen.height-50,35,35),(b+1).ToString(),"AbilityUp");
				if(doubleW[b]>0){
					GUI.Label(new Rect(boxWidth,Screen.height-50,35,35), doubleW[b].ToString(),"Ability");
				}
				boxWidth+=40;
			}
		}
	}
	
	void MakeSpell(int s,int b){
		
		childSpell = GameObject.Find(INFO.ReturnName()+":player/childSpell:collider");
		if(int.Parse(SpellBook.fitLvl[s].Split(';')[2])==0 || int.Parse(SpellBook.fitLvl[s].Split(';')[2])==1){
			childSpell.name = "childSpell:"+s.ToString();
		}else
		if(int.Parse(SpellBook.fitLvl[s].Split(';')[2])==2){
			cur_health+=int.Parse(SpellBook.fitLvl[s].Split(';')[1]);
			Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonNetwork.player.customProperties["Game_race"] }, 
						{ "Game_class", PhotonNetwork.player.customProperties["Game_class"] }, 
						{ "Level", PhotonNetwork.player.customProperties["Level"] }, 
						{ "Health", cur_health}, 
						{ "Energy",  PhotonNetwork.player.customProperties["Energy"] } };
						PhotonNetwork.player.SetCustomProperties(prop);
		}else
		if(int.Parse(SpellBook.fitLvl[s].Split(';')[2])==3){
			booH=true;
			StartCoroutine(DefenceSkill(SpellBook.fitLvl[s]));
		}
		sc = childSpell.GetComponent<SphereCollider>();
		sc.enabled=false;
		sc = childSpell.GetComponent<SphereCollider>();
		sc.enabled=true;
		sc.radius=float.Parse(SpellBook.fitLvl[s].Split(';')[3]);
		StartCoroutine(BeReady(float.Parse(SpellBook.fitLvl[s].Split(';')[1]),b));
		StartCoroutine(TimeReady(float.Parse(SpellBook.fitLvl[s].Split(';')[1]),b));
		
	}
	
	IEnumerator DefenceSkill(string d){
		yield return new WaitForSeconds(float.Parse(d.Split(';')[1]));
		booH=false;
	}
	
	IEnumerator TimeReady(float wait,int b){
		doubleW[b]=(int)(wait/4);
		while(doubleW[b]>0){
			yield return new WaitForSeconds(1);
			doubleW[b]--;
		}
	}
	
	IEnumerator BeReady(float wait,int b){
		yield return new WaitForSeconds(0.25f);
		childSpell.name = "childSpell:collider";
		yield return new WaitForSeconds(wait/4-0.5f);
		notReady[b]=false;
	}
	
	void Die(){	
		hero = GameObject.Find(PhotonNetwork.player.name+":player");
		GameObject[] spawn = GameObject.FindGameObjectsWithTag("Graveyard");	//Find the nearest Graveyard. Graveyard's GameObject needs to have "Graveyard" tag
		if(hero!=null){
			float dif = Mathf.Abs(hero.transform.position.x-spawn[0].transform.position.x)+Mathf.Abs(hero.transform.position.z-spawn[0].transform.position.z);
			GameObject choice = spawn[0];
			foreach(GameObject go in spawn){
				if(Mathf.Abs(hero.transform.position.x-go.transform.position.x)+Mathf.Abs(hero.transform.position.z-go.transform.position.z)<dif){
					dif = Mathf.Abs(hero.transform.position.x-go.transform.position.x)+Mathf.Abs(hero.transform.position.z-go.transform.position.z);
					choice = go;
				}
			}
			hero.transform.position = GameObject.Find(choice.name+"/SpawnPoint").transform.position;
			Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonPlayer.Find(hero.gameObject.GetPhotonView().ownerId).customProperties["Game_race"] }, 
						{ "Game_class", PhotonPlayer.Find(hero.gameObject.GetPhotonView().ownerId).customProperties["Game_class"] }, 
						{ "Level", PhotonPlayer.Find(hero.gameObject.GetPhotonView().ownerId).customProperties["Level"] }, 
						{ "Health", max_health}, 
						{ "Energy", max_energy } };		
			PhotonPlayer.Find(hero.gameObject.GetPhotonView().ownerId).SetCustomProperties(prop);		//Update PhotonNetwork player properties
		}
	}
}
