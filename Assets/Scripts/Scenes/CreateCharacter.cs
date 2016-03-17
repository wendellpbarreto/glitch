using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class CreateCharacter : MonoBehaviour {

	// Use this for initialization
	void OnLevelWasLoaded () {
		createCharacter ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createCharacter()
	{
		KiiUser user = KiiUser.CurrentUser;
		KiiBucket bucket = Kii.Bucket("characters");
		KiiObject kiiObj = bucket.NewKiiObject();
		kiiObj["characterClass"] = "Goblin";
		kiiObj["username"] =  user.Username;
		kiiObj.Save((KiiObject obj, Exception e) =>
			{
				if (e != null)
				{
					Debug.LogError("Failed to save score" + e.ToString());
				}
				else
				{
					Debug.Log("Character created");
					Application.LoadLevel("CharacterSelection");
				}
			});
	}
}
