/*
Ultimate MMORPG Kit - MenuScripts/MainMenuVik - Client's script - skeletarik@gmail.com - 2013
This script is using in the "MainMenu" scene. 
Player can sign in and sign up there.
After the Player tried to sign in, the script receives the information about account from the "Login" script (MainMenuVik.cs <- Login.js <- check_user.php <- MySQL Database).
After the Player tried to sign up, the script sends the information to the "signUp" script (MainMenuVik.cs -> SignUp.js -> signUp.php -> MySQL Database).
For correct work set two GUISkins.
Player can use the toggle for "Remember me" function. It uses "PlayerPrefs" class.
Also Player can read the information about "World of VikingCraft" game which is the example to the Ultimate MMORPG Kit.
Read the Documentation for more details.
*/

using UnityEngine;
using System.Collections;

public class MainMenuVik : MonoBehaviour
{
	public GUISkin g; 			//Default GUISkin
	public GUISkin finalSkin;	//Good-looking GUISkin
	
	int page; 					//Variable for different pages of the Main Menu
	string email;				//for sign in
	string password;			//for sign in
	string newEmail;			//for sign up
	string newPassword;			//for sign up
	bool rem;					//for "Remember me"
	
	// For errors
	bool erGUI1;
	bool erGUI2;
	
    void Awake()
    {
		if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); 
      	page=1;
		if(PlayerPrefs.GetString("Email")!=""){
			email = PlayerPrefs.GetString("Email");			//Load email from "PlayerPrefs". In other way set email to the default value ("Your email")_
		}else{
			email = "Your email";
		}
		password = "Password";
		newEmail = "Your email";
		newPassword = "Your password";
    }
	
	public void AnswerSignUp(string ans){
		ans = ans.Remove(0,1);
		if(ans=="Done"){
			page=1;
		}else{
			StartCoroutine("ErrorWindow", "alreadyBeen");
		}
	}
	
	public void AnswerLogin(string ans){
		ans = ans.Remove(0,1);
		if(ans.Split(':')[0]=="Correct"){
			if(rem){
				PlayerPrefs.SetString("Email", email);	
			}
			if(ans.Split(':')[1]=="0"){							//If Player hasn't any characters, "CreateCharacter" scene will be loaded
				Application.LoadLevel("CreateCharacter");		
			}else{
				Application.LoadLevel("ChooseCharacter");
			}
		}if(ans.Split(':')[0]=="Wrong"){
			StartCoroutine("ErrorWindow", "wrongData");
		}
	}

   
    void OnGUI()
    {
		GUI.skin=g;
        if (!PhotonNetwork.connected)
        {
            ShowConnectingGUI();
            return;  
        }
        if (PhotonNetwork.room != null)
            return;
		GUI.skin=finalSkin;
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////MAIN MENU
		if(page==1){
			GUI.skin=finalSkin;
			GUI.Window(0, new Rect(Screen.width / 2.825f, (Screen.height - 300) / 5, 420, 430), page1W, "", "Window");
		}
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////SIGN UP
		if(page==2){
			GUI.skin=finalSkin;
			GUI.Window(0, new Rect(Screen.width / 2.825f, (Screen.height - 300) / 5, 420, 430), page2W, "", "Window");
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////ABOUT
		if(page==3){
			GUI.Window(0, new Rect(Screen.width / 2.825f, (Screen.height - 300) / 5, 420, 430), page3W, "", "Window");
		}
		//////////////////////////////////////////////////ERRORS
		GUI.skin=g;
		GUI.depth=-1;
		if(erGUI1==true){
			GUI.Box(new Rect(Screen.width/2.8f,Screen.height/2.5f,Screen.width/3,30), "Email or password is wrong");	
		}
		if(erGUI2==true){
			GUI.Box(new Rect(Screen.width/2.8f,Screen.height/2.5f,Screen.width/3,30), "This email has already been used!");	
		}
    }
	
	void page1W(int id){								//"Main Menu" window
			GUI.Label(new Rect(65,90, 400, 400), "\t\t\tWorld of Vikingcraft", "LargeTextMenu");
			GUILayout.BeginArea(new Rect(60,50, 400, 400));
			GUILayout.Space(75);
			GUI.skin=finalSkin;
			if(!erGUI1){
				email = GUILayout.TextField(email, GUILayout.Width(300));
				password = GUILayout.PasswordField(password, '*', GUILayout.Width(300));
				rem = GUILayout.Toggle(rem, "Remember my email");
			}
			GUI.skin=finalSkin;
			if(GUILayout.Button("Play", GUILayout.Width(300))){
				GameObject.Find("WEB_Login").GetComponent("Login").SendMessage("GetData", email+":"+password);
			}
			GUILayout.Space(25);
			if(GUILayout.Button("Sign up!", GUILayout.Width(300))){
				page=2;
			}
			GUILayout.Space(25);
			if(GUILayout.Button("About this game", GUILayout.Width(300))){
				page=3;
			}
			if(GUILayout.Button("Exit", GUILayout.Width(300))){
				Application.Quit();
			}
			GUILayout.EndArea();	
	}

	void page2W(int id){								//"Sign Up" window
			GUI.Label(new Rect(65,90, 400, 400), "\t\t\t\tCreate Account", "LargeTextMenu");
			
			GUILayout.BeginArea(new Rect(60,50, 400, 400));
			GUILayout.Space(75);
			if(!erGUI2){
				newEmail = GUILayout.TextField(newEmail, GUILayout.Width(300));
				newPassword = GUILayout.TextField(newPassword, GUILayout.Width(300));
			}
			if(GUILayout.Button("Sign up!", GUILayout.Width(300))){
				GameObject.Find("WEB_SignUp").GetComponent("SignUp").SendMessage("GetData", newEmail+":"+newPassword);
			}
			GUILayout.Space(25);
			if(GUILayout.Button("Back", GUILayout.Width(300))){
				page=1;
			}
			
			GUILayout.EndArea();	
	}
	
	void page3W(int id){								//"About" window
			GUI.skin=g;
			GUILayout.BeginArea(new Rect(40,50, 340, 400));
			GUI.skin=finalSkin;
			GUILayout.Space(50);
			GUILayout.TextArea("This game is an example project to the Ultimate MMORPG Kit.\nIt uses all scripts that included in this package.\nRead the documentation to know how it works.\n\nThis game contains:\n- Menu (Main, Choosing and Creating character)\n- PHP and MySQL code\n- Quests\n- Good and bad NPCs\n- Crafting and Gathering\n- Dropping things from mobs\n- And much more..\n\nHappy game-making and enjoy this awesome Kit!");
			if(GUILayout.Button("Back")){
				page=1;
			}
			GUILayout.EndArea();
	}
	
    void ShowConnectingGUI(){}
	
	IEnumerator ErrorWindow (string er){
		if(er=="wrongData"){
			erGUI1=true;
			yield return new WaitForSeconds(2);
			erGUI1=false;
		}
		if(er=="alreadyBeen"){
			erGUI2=true;
			yield return new WaitForSeconds(2);
			erGUI2=false;
		}
	}
}
