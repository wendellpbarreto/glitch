/*
Ultimate MMORPG Kit - GameScripts/MiniMap - Client's script - skeletarik@gmail.com - 2013
This script is for the second camera that shows to the Player the MiniMap. The camera will simply follow the Player (x and z values will be equal), 
so he will be always in center of the MiniMap.
*/

using UnityEngine;
using System.Collections;

public class MiniMap : Photon.MonoBehaviour {
	void LateUpdate(){
		GameObject pl = GameObject.Find(PhotonNetwork.player.name+":player");		//Find the Player
		if(pl!=null){																//X and Z miniMap-camera's values will be equal to the Player's values
			transform.position = new Vector3(pl.transform.position.x, transform.position.y, pl.transform.position.z);
		}
	}
}
