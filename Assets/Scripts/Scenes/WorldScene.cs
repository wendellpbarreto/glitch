using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WorldScene : Photon.PunBehaviour {
	
	private bool levelReady = false;
	private GameObject player;
	// Use this for initialization
	void OnLevelWasLoaded () {
		PhotonNetwork.ConnectUsingSettings("0.1");
		Player.character.EnterWorld ();
	}
	
	// Update is called once per frame
	void Update() {
		if (levelReady && GameObject.FindGameObjectsWithTag ("Enemy").Length == 0) {
//			Player.currentWorld.GiveReward ();
			PhotonNetwork.Disconnect();
			Player.currentWorld = null;
			Application.LoadLevel ("Home");
		}

	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.CreateRoom(null);
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
	}

	void OnJoinedRoom()
	{
		Debug.Log ("Joined - "+Player.character.characterClassName);
		player = PhotonNetwork.Instantiate(Player.character.characterClass.prefabName, Vector3.zero, Quaternion.identity, 0);
		if (player != null) {
			PlayerController playerController = player.GetComponent<PlayerController>();
			playerController.enabled = true;
			playerController.isControllable = true;

			PlayerCamera playerCamera = player.GetComponent<PlayerCamera>();
			playerCamera.enabled = true;
			playerCamera.target = player.transform;
		}

		Debug.Log (player.ToString());

		if (PhotonNetwork.isMasterClient) {
			Debug.Log ("Is master client");
			Debug.Log (Player.currentWorld.name+": "+Player.currentWorld.worldEnemies.Count+" spawns");
			foreach (WorldEnemy worldEnemy in Player.currentWorld.worldEnemies) {
				Enemy spawn = Game.GetEnemyById (worldEnemy.enemyId);
				if (spawn != null) {
					Debug.Log (
						"Spawning: name "+spawn.name+", "+
						"prefabName: "+spawn.prefabName
					);	
					GameObject enemy = PhotonNetwork.Instantiate (spawn.prefabName, Vector3.zero, Quaternion.identity, 0);
					EnemyController enemyController = enemy.GetComponent<EnemyController> ();
					enemyController.Activate (spawn);
				}
					
			}
		}

		levelReady = true;
	}
}
