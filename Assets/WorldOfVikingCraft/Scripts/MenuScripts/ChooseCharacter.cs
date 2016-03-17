/*
Ultimate MMORPG Kit - MenuScripts/ChooseCharacter - Client's script - skeletarik@gmail.com - 2013
This script is using in the "ChooseCharacter" scene. 
It receives the information about account's characters from the "Chars" script (ChooseCharacter.cs <- Chars.js <- Chars.php <- MySQL Database).
For correct work set two GUISkins. The Player can have only three characters. 
For deleting the character this script use "Delete" script (ChooseCharacter.cs <- Delete.js <- Delete.php <- MySQL Database).
Read the Documentation for more details.
*/

using UnityEngine;
using System.Collections;

public class ChooseCharacter : MonoBehaviour
{
	public GUISkin g;				//Default GUISkin
	public GUISkin finalSkin;		//Good-looking GUISkin
	
	public GameObject createVik; 	//Prefab for Vikings' race
	GameObject axe;				 	//Different variables for vikings' race. First weapons for different classes
	GameObject shield;
	GameObject axe2;
	GameObject bigAxe;
	GameObject hero;				//Hero
	string game_name;				//Current hero's name
	string game_class;				//Current hero's class
	string game_race;				//Current hero's race
	int level;						//Current hero's level
	int exp;						//Current hero's exp
	string bag;						//Hero's items
	int coins;						//Hero's money
	string weaponS;					//Hero's weapon (right hand)
	string shieldS;					//Hero's shield (or the second one-handed weapon)
	string quests;					//Hero's quests that he is making or already made
	string profession;				//Hero's profesion and it's level
	int chars;						//How many characters in the account
	string[] characters;			//All info about account's characters
	float height;					
	int i=0;
	
	// For errors
	bool erGUI1;
	bool erGUI2;
	
	void Start(){
		GameObject.Find("WEB_Chars").GetComponent("Chars").SendMessage("GetData", INFO.ReturnEmail());
		height = 40;
	}
	
	void GetInfo(string i){							//Receive the information about all characters in one account
		chars = int.Parse(i.Split('^')[1]);
		characters = i.Split('*');
		if(chars>0){
			StartCoroutine("DestroyTime", "0");	
		}
	}
	
	public void AnswerDelete(string ans){
		if(ans=="Deleted"){
			Application.LoadLevel("ChooseCharacter");
		}
	}
	
    void OnGUI()
    {
		GUI.skin=g;
		////////////////////////////////////ERRORS
		if(erGUI1==true){
			GUI.Box(new Rect(Screen.width/3,Screen.height/2.5f,Screen.width/3,30), "You can create only 3 characters!");	
		}
		if(erGUI2==true){
			GUI.Box(new Rect(Screen.width/3,Screen.height/2.5f,Screen.width/3,30), "You need to create your character!");	
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CHOOSE CHARACTER
		GUI.skin = finalSkin;
		GUILayout.BeginArea(new Rect(Screen.width / 2.5f, 50, 400, 1000));
		GUILayout.Label("\t\t\tChoose your character", "LargeTextChoose");
		GUILayout.EndArea();
		GUI.Box(new Rect(Screen.width-Screen.width/4-30,-65,Screen.width/4+40,Screen.height-65+90), "", "Window2");
		GUI.Label(new Rect(Screen.width-Screen.width/4-7, 10, Screen.width/4,30), "Characters");
		height=40;
		i=0;
		if(characters!=null){
			GUI.skin = g;
			foreach(string c in characters){			//For each character draw a window
				if(c[0]=='^'){
					break;
				}
				GUI.Window(i,new Rect(Screen.width-Screen.width/4, height, Screen.width/4-20,(Screen.height-65)/4), charW, c.Split(':')[0]);
				height+=10+(Screen.height-65)/4;
				i++;
			}
			GUI.skin = finalSkin;
		}
		GUI.Label(new Rect(Screen.width / 2.3f, Screen.height-35, 200,35), game_name);
		if(GUI.Button(new Rect(Screen.width-Screen.width/4, Screen.height-60, Screen.width/4, 25), "Create new one!")){
			if(chars<3){
				Application.LoadLevel("CreateCharacter");		//To the menu of the creating a character
			}else{
				StartCoroutine("ErrorWindow", "manyChars");		//Player can create only 3 characters in one account
			}
		}
		if(GUI.Button(new Rect(Screen.width-Screen.width/4, Screen.height-30, Screen.width/4, 25), "Play!")){	//Load the World
			if(chars>0){
				Destroy(hero);
				GameObject INFO = GameObject.Find("INFO");
				INFO.GetComponent("INFO").SendMessage("GetInfo", game_name+":"+game_race+":"+game_class+":"+level.ToString()+":"+exp.ToString()+":"+bag+":"+coins.ToString()+":"+weaponS+":"+shieldS+":"+quests+":"+profession);
				Application.LoadLevel("ChooseServer");
			}else{
				StartCoroutine("ErrorWindow", "noChars");
			}
		}
		if(GUI.Button(new Rect(0, Screen.height-30, Screen.width/4, 25), "Back to Main Menu")){
			Application.LoadLevel("MainMenu");
		}
    }

	void charW(int id){
		GUI.Label(new Rect(5,20,150,100), characters[id].Split(':')[2], "charPrefs");
		GUI.Label(new Rect(200,20,150,100), characters[id].Split(':')[1], "charPrefs");
		GUI.Label(new Rect(5,45,150,100), "Level "+characters[id].Split(':')[3], "charPrefs");
		if(GUI.Button(new Rect(5,70,100,20), "Choose")){
			if(characters[id].Split(':')[1]=="Viking"){
				StartCoroutine("DestroyTime", id.ToString());
				game_race="Viking";
			}
			
		}
		if(GUI.Button(new Rect(120,70,100,20), "Delete")){
			Destroy(GameObject.Find("Player(Clone)"));
			
			GameObject.Find("INFO").GetComponent("INFO").SendMessage("DeleteChar", characters[id].Split(':')[0]);
		}
	}
	
	IEnumerator ErrorWindow (string er){
		GUI.skin = g;
		if(er=="manyChars"){
			erGUI1=true;
			yield return new WaitForSeconds(2);
			erGUI1=false;
		}
		if(er=="noChars"){
			erGUI2=true;
			yield return new WaitForSeconds(2);
			erGUI2=false;
		}
	}
	
	IEnumerator DestroyTime (string ch){
		if(GameObject.Find("Player(Clone)")!=null){
			Destroy(GameObject.Find("Player(Clone)"));
		}
		yield return new WaitForSeconds(0.25f);
		hero = GameObject.Instantiate(createVik, new Vector3(70.80774f,4.55112f, 65.6056f), Quaternion.identity) as GameObject;
		hero.transform.Rotate(1.253711f,228.0281f,1.257576f);
		if(characters[int.Parse(ch)].Split(':')[7]!="None"){
			axe = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigLArmCollarbone/CATRigLArm1/CATRigLArm2/CATRigLArmPalm/"+characters[int.Parse(ch)].Split(':')[7]) as GameObject;
			bigAxe = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigLArmCollarbone/CATRigLArm1/CATRigLArm2/CATRigLArmPalm/"+characters[int.Parse(ch)].Split(':')[7]) as GameObject;
		}
		if(characters[int.Parse(ch)].Split(':')[8]!="None"){
			axe2 = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigRArmCollarbone/CATRigRArm1/CATRigRArm2/CATRigRArmPalm/"+characters[int.Parse(ch)].Split(':')[8]) as GameObject;
			shield = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigRArmCollarbone/CATRigRArm1/CATRigRArm2/CATRigRArmPalm/"+characters[int.Parse(ch)].Split(':')[8]) as GameObject;
		}
		GameObject[] w = GameObject.FindGameObjectsWithTag("Weapon");
		GameObject[] s = GameObject.FindGameObjectsWithTag("Shield");
		if(characters[int.Parse(ch)].Split(':')[2]=="Warrior"){				//Receive different values depending on the character's class
			foreach(GameObject th in w){
				if(characters[int.Parse(ch)].Split(':')[7]!="None"){		//Warrior
					if(th.name!=bigAxe.name){
						th.SetActive(false);
					}	
				}else{
					th.SetActive(false);	
				}
			}
			foreach(GameObject th in s){
				th.SetActive(false);
			}
			game_class="Warrior";
		}else
		if(characters[int.Parse(ch)].Split(':')[2]=="Defender"){			//Defender
			foreach(GameObject th in w){
				if(characters[int.Parse(ch)].Split(':')[7]!="None"){
					if(th.name!=axe.name){
						th.SetActive(false);
					}
				}else{
					th.SetActive(false);
				}
			}
			foreach(GameObject th in s){
				if(characters[int.Parse(ch)].Split(':')[8]!="None"){
					if(th.name!=shield.name){
						th.SetActive(false);
					}
				}else{
					th.SetActive(false);
				}
			}
			game_class="Defender";
		}else
		if(characters[int.Parse(ch)].Split(':')[2]=="Robber"){				//Robber
			foreach(GameObject th in w){
				if(characters[int.Parse(ch)].Split(':')[7]!="None"){
					if(th.name!=axe.name){
						th.SetActive(false);
					}
				}else{
					th.SetActive(false);
				}
			}
			foreach(GameObject th in s){
				if(characters[int.Parse(ch)].Split(':')[8]!="None"){
					if(th.name!=axe2.name){
						th.SetActive(false);
					}
				}else{
					th.SetActive(false);
				}
			}
			game_class="Robber";
		}
		level = int.Parse(characters[int.Parse(ch)].Split(':')[3]);
		game_name = characters[int.Parse(ch)].Split(':')[0];
		game_race = characters[int.Parse(ch)].Split(':')[1];
		exp = int.Parse(characters[int.Parse(ch)].Split(':')[4]);
		bag = characters[int.Parse(ch)].Split(':')[6];
		coins = int.Parse(characters[int.Parse(ch)].Split(':')[5]);
		weaponS = characters[int.Parse(ch)].Split(':')[7];
		shieldS = characters[int.Parse(ch)].Split(':')[8];
		quests = characters[int.Parse(ch)].Split(':')[9];
		profession = characters[int.Parse(ch)].Split(':')[10];
	}
}
