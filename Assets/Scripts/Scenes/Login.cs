using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;


[ExecuteInEditMode]
public class Login : MonoBehaviour {

	public GUISkin skin;

	private string username = "admin";
	private string password = "password";

	public AudioSource audioSource;
	public AudioClip audioClip;
	// Use this for initialization

	void OnGUI(){
		GUI.skin = skin;

		username = GUI.TextField(new Rect(Screen.width/2 - 150, Screen.height/2 - 72, 300, 35), username, 25);
		password = GUI.TextField(new Rect(Screen.width/2 - 150, Screen.height/2 - 35, 300, 35), password, 25);
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2, 200, 25), "Login")) {
			audioSource.PlayOneShot (audioClip);
			DoLogin ();
		}
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 28, 200, 25), "Register")) {
			audioSource.PlayOneShot (audioClip);
			SceneManager.LoadScene ("Register");
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
					SceneManager.LoadScene ("GameLoader");
				}
			});
	}
}
