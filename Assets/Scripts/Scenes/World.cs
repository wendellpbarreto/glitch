using UnityEngine;
using System.Collections;

public class World : Photon.PunBehaviour {

	public GameObject playerPrefab;

	// Use this for initialization
	void OnLevelWasLoaded () {
		PhotonNetwork.ConnectUsingSettings("0.1");
		Player.character.EnterWorld ();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	void OnJoinedRoom()
	{
		Debug.Log ("Joined - "+Player.character.characterClassName);
		GameObject player = PhotonNetwork.Instantiate(Player.character.characterClass.prefabName, Vector3.zero, Quaternion.identity, 0);
		Debug.Log (player.ToString());
		if (player != null) {
			PlayerController playerController = player.GetComponent<PlayerController>();
			playerController.enabled = true;
			playerController.isControllable = true;

			PlayerCamera playerCamera = player.GetComponent<PlayerCamera>();
			playerCamera.enabled = true;
			playerCamera.target = player.transform;

		}
	}
}
