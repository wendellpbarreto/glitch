using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class Register : MonoBehaviour {

	public GUISkin skin;

	private string username = "admin";
	private string email = "admin@mail.com";
	private string password = "password";
	// Use this for initialization
	void Start () {
	}

	void OnGUI(){
		GUI.skin = skin;

		email = GUI.TextField(new Rect(Screen.width/2 - 100, Screen.height/2 - 109, 200, 35), email, 25);
		username = GUI.TextField(new Rect(Screen.width/2 - 100, Screen.height/2 - 72, 200, 35), username, 25);
		password = GUI.TextField(new Rect(Screen.width/2 - 100, Screen.height/2 - 35, 200, 35), password, 25);

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 28, 200, 25), "Register")) {
			DoRegister ();
		}

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2, 200, 25), "Cancel")) {
			GoToLogin ();
		}
	}

	void DoRegister(){
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
					GoToLogin ();
				}
			});
	}

	void GoToLogin(){
		Application.LoadLevel ("Login");
	}
}
