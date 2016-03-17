/*
Ultimate MMORPG Kit - GameScripts/WorldItemProf - Client's script - skeletarik@gmail.com - 2013
This script is for World's workshops that Player will use in his profession.
*/

using UnityEngine;
using System.Collections;

public class WorldItemProf : MonoBehaviour {

	void OnTriggerEnter(Collider other){											//If Player is near the workshop
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			GameObject.Find(PhotonNetwork.player.name+":player/Main Camera").GetComponent("Profession").SendMessage("WorldItem", "true");
		}
	}
	
	void OnTriggerExit(Collider other){												//If Player leave the workshop
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			GameObject.Find(PhotonNetwork.player.name+":player/Main Camera").GetComponent("Profession").SendMessage("WorldItem", "false");
		}
	}
}
