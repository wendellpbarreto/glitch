/*
Ultimate MMORPG Kit - NetScripts/ThirdPersonNetworkVik - Client's script - skeletarik@gmail.com - 2013
This script is for special properties of correct displaying the Player to the other Players.
*/

using UnityEngine;
using System.Collections;

public class ThirdPersonNetworkVik : Photon.MonoBehaviour
{
    ThirdPersonCameraNET cameraScript;
    ThirdPersonControllerNET controllerScript;
	GameObject axe;				 
	GameObject shield;
	GameObject axe2;
	GameObject bigAxe;

    void Awake()
    {
        cameraScript = GetComponent<ThirdPersonCameraNET>();
        controllerScript = GetComponent<ThirdPersonControllerNET>();

    }
	
    void Start()
    {
        if (photonView.isMine)
        {
          	cameraScript.enabled = true;
            controllerScript.enabled = true;
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = new Vector3(0, 2, -10);
            Camera.main.transform.localEulerAngles = new Vector3(10, 0, 0);
			GetComponent<Collider>().enabled=false;
        }
        else
        {           
            cameraScript.enabled = false;
            controllerScript.enabled = true;
        }
        controllerScript.SetIsRemotePlayer(!photonView.isMine);
        gameObject.name = photonView.owner.name + ":player";
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
          	//stream.SendNext((int)controllerScript._characterState);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(GetComponent<Rigidbody>().velocity); 

        }
        else
        {
            //Network player, receive data
            //controllerScript._characterState = (CharacterState)(int)stream.ReceiveNext();
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            GetComponent<Rigidbody>().velocity = (Vector3)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    void Update()
    {
        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
		//We know there should be instantiation data..get our bools from this PhotonView!
        object[] objs = photonView.instantiationData; //The instantiate data..
        string[] myPrefs = (string[])objs[1];
		
		 //Disable name
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
		if(photonView.isMine){
       		rens[0].enabled = false; //Name
		}else{
			rens[0].enabled = true; //Name
		}
		
		if(myPrefs[1]=="Warrior"){
	        foreach(MeshRenderer n in rens){
				n.enabled = false;				//Disable all weapons. We will enable a few of them later	
			}
		}else
		if(myPrefs[1]=="Defender"){
			foreach(MeshRenderer n in rens){
				n.enabled = false;	
			}
		}else
		if(myPrefs[1]=="Robber"){
			foreach(MeshRenderer n in rens){
				n.enabled = false;	
			}
		}else{
			Debug.LogError("Ultimate MMORPG Kit error: unknown game class! There is: "+myPrefs[1]+" Length="+myPrefs[1].Length);
		}
		
		TextMesh[] tm = GetComponentsInChildren<TextMesh>();
		tm[0].text = photonView.owner.name;
		tm[0].GetComponent<Renderer>().material.color=Color.green;
    }
}