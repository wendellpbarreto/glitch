/*
Ultimate MMORPG Kit - GameScripts/Drop - Client's script - skeletarik@gmail.com - 2013
This script is for dropping from enemies or from rocks (ore). In the client you just need to add that script to the enemy or to the rock and set the GUISkin.
Other settings you must do in the Server.

NB! It is the client's version of the script. The Server's version of the script also exists. You can find it in the Server folders.
*/

using UnityEngine;
using System.Collections;

public class Drop : Photon.MonoBehaviour {

	int quantity;
	float[] probability;
	Component[] items;
	string[] items_data;
	bool[] result;
	string[] items_data_end;
	bool[] result_end;
	bool mine;
	int i=0;
	bool dead;
	bool list;
	bool only;
	float x;
	float y;
	bool one;
	bool ent;
	
	public GUISkin g;									//Default sking
	Vector2 scrollPosition = Vector2.zero;
	
	void Start(){
		if(PhotonNetwork.isMasterClient){				//That happens only in the Server
			items_data = new string[quantity];
			result = new bool[quantity];
			items[0].SendMessage("SendStats");
		}else{
			quantity=0;
			probability = new float[1];
			items = new Component[1];
		}
	}
	
	public void GetStats(string data){					//Get stats from items in the Server
		items_data[i]=data;
		i++;
	}
	
	void OnTriggerEnter(Collider other){				//If the Player enters in the enemy's/rock's trigger, he will be able to take dropped items
		if(other.gameObject.name == PhotonNetwork.player.name+":player"){
			ent=true;
		}
	}
	
	void OnTriggerExit(Collider other){					//If the Player exits from the enemy's/rock's, he won't be able to take dropped items
		if(other.gameObject.name == PhotonNetwork.player.name+":player"){
			ent=false;
			if(mine==true){								//If it is rock
				photonView.RPC("DestroyRock", PhotonTargets.MasterClient);
			}
		}
	}
	
	public void MineDrop(){								//Drop from rock
		mine=true;
		photonView.RPC("ServerMineDrop", PhotonTargets.MasterClient);
	}
	
	[RPC]	
	public void ServerMineDrop(){						//Server only function
		Dropping();
	}
	
	public void Dropping(){								//The process of dropping. That happens in the server
		if(PhotonNetwork.isMasterClient && !only){
			System.Random rand = new System.Random();
			int i2=0;
			foreach(float p in probability){
				float r;
				r = rand.Next(100);
				if(r<=p){
					result[i2]=true;
				}else{
					result[i2]=false;
				}
				i2++;
			}
			object[] info = new object[2];
			info[0] = items_data;
			info[1] = result;
			only=true;
			photonView.RPC("DropInfo", PhotonTargets.All, info);
		}
	}
	
	void OnGUI(){
		GUI.skin = g;
		if(dead==true && !PhotonNetwork.isMasterClient){
			if(!one){
				x = Input.mousePosition.x;						//The drop window will be appeared near the cursor
				y = Input.mousePosition.y;
				one=true;
			}
			if(list && ent){
				GUI.Window(0,new Rect(x, y, 150,150), dropWindow, "Items:","Box");	//Showing the window
			}
		}
	}
	
	[RPC]
	public void DestroyRock(){									//Destroy rock after the Player took the dropped items. That happens on the server
		PhotonNetwork.Destroy(gameObject);
	}
	
	void dropWindow(int id){
		int height = 0;
		int i=0;
		if(GUI.Button(new Rect(127,5,18,15), "X")){				//That will close the drop window
			list=false;
			if(mine==true){
				photonView.RPC("DestroyRock", PhotonTargets.MasterClient);
			}
		}
		scrollPosition = GUI.BeginScrollView(new Rect(5, 25, 140, 115), scrollPosition, new Rect(0, 0, 130, 200));
		foreach(string item in items_data_end){
			if(item==null){
				
			}
			if(result_end[i]==true){
				GUI.Box(new Rect(5, height,120,40), "");
				if(GUI.Button(new Rect(10, height+5,30,30), (Texture2D)Resources.Load("Items/"+items_data_end[i].Split('!')[7], typeof(Texture2D)), "Texture")){
					//If the Player has empty slots in his bag, he will take the items. In other way the warning will appear.
					if(Bag.ReturnI()<11){
						Bag.PlusI(items_data_end[i]);
						photonView.RPC("TakeI", PhotonTargets.All, i);
					}else{
						Debug.LogWarning("Ultimate MMORPG warning: Hero hasn't any empty slots in his bag!");
					}
				}
				if(items_data_end[i].Split('!')[1]=="bad"){					//Different colours depending on item's quality
					GUI.contentColor = new Color(0.768f,0.768f,0.768f);		//Light-grey if it is "bad"
				}
				if(items_data_end[i].Split('!')[1]=="normal"){
					GUI.contentColor = new Color(0,0.807f,0);				//Green if it is "normal"
				}
				if(items_data_end[i].Split('!')[1]=="rare"){
					GUI.contentColor = new Color(0.3137f,0.180f,0.933f);	//Blue-purple if it is "rare"
				}
				GUI.Label(new Rect(45, height+7, 100,30), items_data_end[i].Split('!')[0]);
				GUI.contentColor = Color.white;
				height+=45;
			}
			i++;
		}
		GUI.EndScrollView();
	}
	
	[RPC]
	void TakeI(int t){							//If one Player already took the item, other Player wouldn't be able to take it
		result_end[t]=false;
	}
	
	
	[RPC]
	void DropInfo(object[] info){				//Information about drop: which items were dropped.
		items_data_end = (string[])info[0];
		result_end = (bool[])info[1];
		dead=true;
		list=true;
	}
}