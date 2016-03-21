using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class Login : MonoBehaviour {
	private string username = "admin";
	private string email = "admin@mail.com";
	private string password = "password";
	// Use this for initialization
	void Start () {
	}

	void OnGUI(){
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), "Login")) {
			DoLogin ();
		}
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 20, 200, 30), "Register")) {
			Register ();
		}
	}

	void Register(){
		KiiUser.Builder builder = KiiUser.BuilderWithName(username);
		builder.WithEmail(email);
		KiiUser usr = builder.Build();
		usr.Register(password, (KiiUser user, Exception e) =>
			{
				if (e != null)
				{
					Debug.LogError("Signup failed: " + e.ToString());
				}
				else
				{
					Debug.Log("Signup succeeded");
				}
			});
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
