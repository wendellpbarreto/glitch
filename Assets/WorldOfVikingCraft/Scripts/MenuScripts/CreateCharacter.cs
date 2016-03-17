/*
Ultimate MMORPG Kit - MenuScripts/CreateCharacter - Client's script - skeletarik@gmail.com - 2013
This script is using in the "CreatCharacter" scene. 
After creation it sends the information about new character to the "Create" script (CreateCharacter.cs -> Create.js -> Create.php -> MySQL Database).
For correct work set two GUISkins. 
Read the Documentation for more details.
*/

using UnityEngine;
using System.Collections;

public class CreateCharacter : MonoBehaviour
{

	public GUISkin g; 				//Default GUISkin
	public GUISkin finalSkin;		//Good-looking GUISkin
	
	int page; 						//Variable for different pages of Main Menu
	public GameObject createVik; 	//Prefab for Vikings' race
	GameObject axe;					//Different variables for vikings' race. First weapons for different classes and description to the whole race
	GameObject shield;
	GameObject axe2;
	GameObject bigAxe;
	string vik_desc = "These guys are brave, strong and bold. Vikings have a great history. And now they must save the world!";
	
	/* There you can add your own prefabs and race's variables for different races
	public GameObject createYourRace;
	...
	*/
	
	bool oCreate; 			//It is for Instantiate. We need it only one time.
	GameObject hero;		//Hero
	string game_name;		//Hero's name      
	string game_class;		//Hero's class
	string game_race;		//Hero's race
	string class_desc;		//Desription for hero's class
	string race_desc;		//Desription for hero's race
	string warrior_desc = "Warriors always have a powerful two-handled weapon and great strength. Warrior is a good choice for assured players.";
	string defender_desc = "Defenders think a little bit before start a new battle. They have shield and one-handled weapon. Defender is an excellent class for clever and logical players.";
	string robber_desc = "Robbers are fast, quiet and invisible. They use two one-handled weapons. Robber is a great choice for wise men.";
	
	// For errors
	bool erGUI1;
	bool erGUI2;
	
    void Awake()
    {
		game_name="Name";		//Default name is "Name"
    }
	
	public void AnswerCreate(string ans){
		if(ans=="Created"){
			Application.LoadLevel("ChooseCharacter");			//Creation will be successful only if character's name is unique and Player has already two characters or less
		}else{
			if(ans == "Been"){
				StartCoroutine("ErrorWindow", "alreadyBeen");	//Error if this name has already been
			}
			if(ans == "TooMuch"){
				StartCoroutine("ErrorWindow", "tooMuch");		//Error if Player has three characters in one account
			}
		}
	}
 
    void OnGUI()
    {
		GUI.skin=finalSkin;
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CREATE CHARACTER
			GUILayout.BeginArea(new Rect(Screen.width / 2.5f, 50, 400, 1000));
			GUILayout.Label("\t\t\tCreating character", "LargeTextChoose");
			if(oCreate==false){
				hero = GameObject.Instantiate(createVik, new Vector3(70.80774f,4.55112f, 65.6056f), Quaternion.identity) as GameObject;
				hero.transform.Rotate(1.253711f,228.0281f,1.257576f);
				game_class="Warrior";
				game_race="Viking";
				race_desc=vik_desc;
				class_desc=warrior_desc;
				MeshRenderer[] rens = GameObject.Find("Player(Clone)").GetComponentsInChildren<MeshRenderer>();
				if(game_race=="Viking"){
					axe = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigLArmCollarbone/CATRigLArm1/CATRigLArm2/CATRigLArmPalm/Axe") as GameObject;
					bigAxe = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigLArmCollarbone/CATRigLArm1/CATRigLArm2/CATRigLArmPalm/Big Axe") as GameObject;
					axe2 = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigRArmCollarbone/CATRigRArm1/CATRigRArm2/CATRigRArmPalm/Axe") as GameObject;
					shield = GameObject.Find("Player(Clone)/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigRArmCollarbone/CATRigRArm1/CATRigRArm2/CATRigRArmPalm/Shield") as GameObject;
					axe.SetActive(false);
					shield.SetActive(false);
					axe2.SetActive(false);
					foreach(MeshRenderer m in rens){
						if(m.gameObject.name!=axe.name && m.gameObject.name!=axe2.name && m.gameObject.name!=bigAxe.name && m.gameObject.name!=shield.name){
							m.gameObject.SetActive(false);
						}
					}
				}
				
				/* There you can add properties to your own race
				if(game_race=="your_race"){
					
				}*/
				
				oCreate=true;
			}
			GUILayout.EndArea();
			GUI.Box(new Rect(Screen.width / 2.5f-30, Screen.height-55, 340,60), "", "Window2");
			if(GUI.RepeatButton(new Rect(Screen.width / 2.5f-10, Screen.height-40, 50,40), "<")){
				hero.transform.Rotate(0,1,0);
			}
			game_name = GUI.TextField(new Rect(Screen.width / 2.5f+40, Screen.height-35, 200,30), game_name);
			if(GUI.RepeatButton(new Rect(Screen.width / 2.5f+40+200, Screen.height-40, 50,40), ">")){
				hero.transform.Rotate(0,-1,0);	
			}
			GUI.Box(new Rect(-10,-25,Screen.width/4+40,Screen.height/2+50), "", "Window");
			GUI.Label(new Rect(30,5,Screen.width/4-40,30), "Classes:");
			if(GUI.Button(new Rect(20,60,Screen.width/4-20,(Screen.height/2)/5), "Warrior")){
				game_class="Warrior";
				shield.SetActive(false);
				axe.SetActive(false);
				bigAxe.SetActive(true);
				axe2.SetActive(false);
				class_desc=warrior_desc;
			}
			if(GUI.Button(new Rect(20,60+(Screen.height/2)/5,Screen.width/4-20,(Screen.height/2)/5), "Defender")){
				game_class="Defender";
				shield.SetActive(true);
				axe.SetActive(true);
				bigAxe.SetActive(false);
				axe2.SetActive(false);
				class_desc=defender_desc;
			}
			if(GUI.Button(new Rect(20,60+((Screen.height/2)/5)*2,Screen.width/4-20,(Screen.height/2)/5), "Robber")){
				game_class="Robber";
				shield.SetActive(false);
				axe.SetActive(true);
				bigAxe.SetActive(false);
				axe2.SetActive(true);
				class_desc=robber_desc;
			}
			GUI.Box(new Rect(-10,Screen.height/2-24,Screen.width/4+40,Screen.height/2+50), "", "Window");
			GUI.Label(new Rect(30,Screen.height/2+6,Screen.width/4-40,30), "Description:");
			GUI.TextArea(new Rect(20,Screen.height/2+70, Screen.width/4-20,(Screen.height/2)-100),class_desc);
			GUI.Box(new Rect(Screen.width-Screen.width/4-30,-25,Screen.width/4+40,Screen.height/2+50), "", "Window");
			GUI.Label(new Rect(Screen.width-Screen.width/4+10,5,Screen.width/4-40,30), "Race:");
			if(GUI.Button(new Rect(Screen.width-Screen.width/4,60,Screen.width/4-20,(Screen.height/2)/5), "Viking")){
				game_race="Viking";
			}
			GUI.Box(new Rect(Screen.width-Screen.width/4-30,Screen.height/2-24,Screen.width/4+40,Screen.height/2+10), "", "Window");
			GUI.Label(new Rect(Screen.width-Screen.width/4+10,Screen.height/2+6,Screen.width/4-40,30), "Description:");
			GUI.TextArea(new Rect(Screen.width-Screen.width/4,Screen.height/2+71, Screen.width/4-20,(Screen.height/2-40)-100 ),race_desc);
			if(GUI.Button(new Rect(Screen.width-Screen.width/4, Screen.height-30, Screen.width/4, 25), "Create!")){
				GameObject.Find("INFO").GetComponent("INFO").SendMessage("GetInfo", game_name+":"+game_race+":"+game_class+":1:0:Nothing:0:Nothing:Nothing:Nothing:Nothing");
				//PlayerPrefs.SetInt(INFO.ReturnEmail()+"_"+INFO.ReturnName()+"_Spell0", 1);	
				//PlayerPrefs.SetInt(INFO.ReturnEmail()+"_"+INFO.ReturnName()+"_Spell1", 2);	
			}	
		////////////////////////////////////ERRORS
		GUI.skin = g;
		if(erGUI1==true){
			GUI.Box(new Rect(Screen.width/3,Screen.height/2.5f,Screen.width/3,30), "This name is already taken!");	
		}
		if(erGUI2==true){
			GUI.Box(new Rect(Screen.width/3,Screen.height/2.5f,Screen.width/3,30), "You can create only 3 characters!");	
		}
    }
	
	IEnumerator ErrorWindow (string er){
		if(er=="alreadyBeen"){
			erGUI1=true;
			yield return new WaitForSeconds(2);
			erGUI1=false;
		}
		if(er=="tooMuch"){
			erGUI2=true;
			yield return new WaitForSeconds(2);
			erGUI2=false;
		}
	}
}
