using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static PhotonView ScenePhotonView;

	void Start()
	{
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void OnJoinedRoom()
	{
	}

	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}
		

	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerDisconnected: " + player);
	}

	void Update () {
	
	}
}
