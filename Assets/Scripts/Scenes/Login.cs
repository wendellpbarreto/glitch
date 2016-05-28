using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class Login : MonoBehaviour {

	public GUISkin skin;

	private string username = "admin";
	private string password = "password";
	// Use this for initialization
	void Start () {
	}

	void OnGUI(){
		GUI.skin = skin;

		username = GUI.TextField(new Rect(Screen.width/2 - 100, Screen.height/2 - 72, 200, 35), username, 25);
		password = GUI.TextField(new Rect(Screen.width/2 - 100, Screen.height/2 - 35, 200, 35), password, 25);
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2, 200, 25), "Login")) {
			DoLogin ();
		}
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 28, 200, 25), "Register")) {
			Application.LoadLevel ("Register");
		}
	}



	void DoLogin(){
		KiiUser.LogIn(username,password, (KiiUser user, Exception e) =>
			{
				if (e != null)
				{
					Debug.LogError("Login failed: " + e.ToString());
					// process error
				}
				else
				{
					Debug.Log("Login succeeded");
					Application.LoadLevel("CharacterSelection");
				}
			});
	}
}
