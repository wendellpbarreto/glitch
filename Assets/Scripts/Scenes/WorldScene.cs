using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WorldScene : Photon.PunBehaviour {
	
	private bool levelReady = false;
	private bool levelOver = false;
	private Item itemWon;

	public GUISkin skin;

	private GameObject player;
	// Use this for initialization
	void OnLevelWasLoaded () {
		PhotonNetwork.ConnectUsingSettings("0.1");
		Player.character.EnterWorld ();
	}
	
	// Update is called once per frame
	void Update() {
		if (!levelOver && levelReady && GameObject.FindGameObjectsWithTag ("Enemy").Length == 0 ) {
//			Player.currentWorld.GiveReward ();
			EndLevel();
		}

	}

	void EndLevel(){
		itemWon = Player.currentWorld.GiveReward ();
		levelOver = true;
	}

	void OnGUI()
	{
		GUI.skin = skin;

		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		if (levelOver) {
			GUI.Box (new Rect(Screen.width/2 - Screen.width/4, Screen.height/2 - Screen.height/4, Screen.width/2, Screen.height/2), "Rewards");
			//Exibir rewards aqui
			if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2, 200, 25), "Close")) {
				PhotonNetwork.Disconnect ();
				Player.currentWorld = null;
				SceneManager.LoadScene ("Home");
			}
		}
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.CreateRoom(Player.character.name+System.DateTime.Now.ToString());
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
					int angle = UnityEngine.Random.Range (1, 360);
					float x = Mathf.Cos(angle)*30;
					float z = Mathf.Sin(angle)*30;
					Vector3 spawnPoint = new Vector3 (x, 0f, z);
					GameObject enemy = PhotonNetwork.Instantiate (spawn.prefabName, spawnPoint, Quaternion.identity, 0);
					EnemyController enemyController = enemy.GetComponent<EnemyController> ();
					enemyController.Activate (spawn);
				}
					
			}
		}

		levelReady = true;
	}
}
