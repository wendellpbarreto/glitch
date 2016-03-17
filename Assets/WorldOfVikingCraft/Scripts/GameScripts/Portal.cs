/*
Ultimate MMORPG Kit - GameScripts/Portal - Client's script - skeletarik@gmail.com - 2013
This script is for making portals. All you need for creating a portal is to set a place, where it will transfer (just drag-and-drop the GameObject from Hierarchy to inspector).
Also you can edit the position of the place where it will transfer (<plusPositionX>, <plusPositionY>, <plusPositionZ>). 
It will be useful if you want to set two portals for "there and back again system".
And you can set your own sound of teleportation. 
*/

using UnityEngine;
using System.Collections;

public class Portal : Photon.MonoBehaviour {

	public Transform place;			//Place where it will transfer
	public float plusPositionX;		//Addition to the X value of the place. Default is 0.
	public float plusPositionY;		//Addition to the Y value of the place. Default is 0.
	public float plusPositionZ;		//Addition to the Z value of the place. Default is 0.
	public AudioClip teleport;		//Sound of teleportation
		
	void OnTriggerEnter(Collider other){									//If Player enters into the portal
		if(other.gameObject.name.Split(':')[0]==PhotonNetwork.player.name){
			GetComponent<AudioSource>().clip = teleport;												//Play the sound of teleportation
			GetComponent<AudioSource>().Play();
			other.gameObject.transform.position = new Vector3(place.position.x+plusPositionX, place.position.y+plusPositionY, place.position.z+plusPositionZ);
		}
	}
}
