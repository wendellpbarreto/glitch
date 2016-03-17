/*
Ultimate MMORPG Kit - NetScripts/GameManagerVik - Client's script - skeletarik@gmail.com - 2013
This script is very important. It prepares instantiation data and answers for connecting/disconnecting from Photon Cloud.

NB! It is the client's version of the script. The Server's version of the script also exists. You can find it in the Server folders.
*/

using UnityEngine;
using System.Collections;

public class GameManagerVik : Photon.MonoBehaviour {

    public string playerPrefabName = "Charprefab";			//Viking's prefab
	
    void OnJoinedRoom()
    {
        StartGame();
    }
    
    IEnumerator OnLeftRoom()
    {
        //Wait until Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;

        Application.LoadLevel(Application.loadedLevel);
    }

    void StartGame()
	{
		//prepare instantiation data for the viking
        bool[] enabledRenderers = new bool[2];
		if(INFO.ReturnWeapon()!="None"){
        	enabledRenderers[0] = true;//Axe
		}else{
			enabledRenderers[0] = false;
		}
		if(INFO.ReturnShield()!="None"){
        	enabledRenderers[1] = true;//Axe
		}else{
			enabledRenderers[1] = false;
		}
		string[] charPref = new string[2];
        charPref[0] = INFO.ReturnInfo().Split(':')[1];
        charPref[1] = INFO.ReturnInfo().Split(':')[2];
        
        object[] objs = new object[2]; 						//Put our bool data in an object array, to send
        objs[0] = enabledRenderers;
		objs[1] = charPref;
		
        //Spawn our local player
        PhotonNetwork.Instantiate(this.playerPrefabName, transform.position, Quaternion.identity, 0,objs);
		INFO.GameStarted();			//The game has started, so the script will send the information about that to the "INFO" script
    }
	
    void OnGUI()
    {
        if (PhotonNetwork.room == null) return; 
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
    void OnFailedToConnectToPhoton()
    {
        Debug.LogWarning("OnFailedToConnectToPhoton");
    }
}
