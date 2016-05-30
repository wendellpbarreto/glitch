using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;


[ExecuteInEditMode]
public class Register : MonoBehaviour {

	public GUISkin skin;

	private string username = "username";
	private string email = "email";
	private string password = "password";
	// Use this for initialization

	void OnGUI(){
		GUI.skin = skin;

		email = GUI.TextField(new Rect(Screen.width/2 - 150, Screen.height/2 - 109, 300, 35), email, 25);
		username = GUI.TextField(new Rect(Screen.width/2 - 150, Screen.height/2 - 72, 300, 35), username, 25);
		password = GUI.TextField(new Rect(Screen.width/2 - 150, Screen.height/2 - 35, 300, 35), password, 25);

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
		SceneManager.LoadScene ("Login");
	}
}
