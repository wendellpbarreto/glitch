/*
Ultimate MMORPG Kit - GameScripts/RockScript - Client's script - skeletarik@gmail.com - 2013
This script is for respawning Rocks that contains the ore. Simply add this to the Rock's prefab in the "Resources" folder.

NB! It is the client's version of the script. The Server's version of the script also exists. You can find it in the Server folders.
*/

using UnityEngine;
using System.Collections;

public class RockScript : Photon.MonoBehaviour {

	bool ended;										//Is the ore taken?
	
	void Update(){
		if(ended){									//If the ore is taken, the Rock will respawn in another place (it happens in the Server)
			ended=false;
			ServerSend();
		}
	}
	
	void Start(){
		gameObject.name = gameObject.name+":WorldItem";	
	}
	
	public void ServerSend(){
		photonView.RPC("ServerRespawn",PhotonTargets.MasterClient);
	}
	
	[RPC]
	public void ServerRespawn(){
		gameObject.transform.parent.GetComponent("MiningAI").SendMessage("ReSpawnStart");
	}
}
