/*
Ultimate MMORPG Kit - GameScripts/Vendor - Client's script - skeletarik@gmail.com - 2013
This script is for NPC-vendor. The Player can sell his items or buy Vendor's goods. 
If you want to set Vendor's goods, add items' scripts to the NPC's GameObject and 
drag-and-drop them to the <items>. Also you must set <quantity> to the number of the items.
For correct work you need to set two GUISkins too.
*/

using UnityEngine;
using System.Collections;

public class Vendor : Photon.MonoBehaviour {

	bool isActive;								//Is window opened?
	public GUISkin g;							//Good-looking GUISkin
	public GUISkin main;						//Default GUISkin
	int page=0;
	public Component[] items;					//Vendor's goods
	string[] items_data_end;
	public int quantity;						//The number of Vendor's goods
	int height=100;
	int iD=0;
	private float spikeCount;
	int i2;
	bool plus;
	int coins;
	float coinsHeight;
	float alpha;
	
	
	void Start(){
		items_data_end = new string[quantity];	
		items[0].SendMessage("SendStatsVendor");
	}
	
	public void GetStats(string data){			//Get items' information
		items_data_end[iD]=data;
		iD++;
	}
	
	public void OpenVendor(){					//Open Vendor's window
		isActive=true;	
	}
	public void CloseVendor(){					//Close Vendor's window
		isActive=false;		
	}
	
	public void Sell(string item){				//Sell to the Vendor
		coins = int.Parse(item.Split('!')[3]);
		StartCoroutine("PlusCoins", coins);	
	}
	
	void OnGUI(){
		GUI.skin = g;
		if(isActive){							//If window is opened
			GUI.Window(10, new Rect(0,Screen.height/6,300,350), vendorWindow, "");	
		}
	}
	
	void vendorWindow(int id){
		int i = 0;
		height=100;
		AddSpikes(300);
		///////////////////////////////////////////////////////Page for buying
		if(page==0){
			if(GUI.Button(new Rect(40,260,100,30), "Bye!")){			//Close the window
				isActive=false;
				page=0;
			}
			foreach(string item in items_data_end){
				if(item==null){
				}
				GUI.Box(new Rect(45, height,120,40), "");
				GUI.skin = main;
				if(GUI.Button(new Rect(50, height+5,30,30), (Texture2D)Resources.Load("Items/"+items_data_end[i].Split('!')[7], typeof(Texture2D)), "Texture")){
					if(Bag.ReturnI()<11 && int.Parse(INFO.ReturnCoins())>=int.Parse(items_data_end[i].Split('!')[3])){
						Bag.PlusIVendor(items_data_end[i]);	
					}else{
						Debug.LogWarning("Ultimate MMORPG warning: Hero hasn't any empty slots in his bag or enough money!");
					}
				}	
				GUI.contentColor = new Color(0.25f,0.25f,0.25f);
				GUI.Label(new Rect(85, height+4, 100,30), items_data_end[i].Split('!')[0]);
				GUI.Label(new Rect(85, height+5, 100,30), items_data_end[i].Split('!')[3]+" coins", "Ability");
				GUI.contentColor = Color.white;
				GUI.skin = g;
				height+=45;
				i++;
			}
			if(GUI.Button(new Rect(160,260,100,30), "Selling")){		//To the page of selling	
				page=1;
				GameObject.Find("Main Camera").GetComponent("Bag").SendMessage("VendorBag", "true:"+gameObject.name);
			}
		}
		/////////////////////////////////////////////////////////////////////////Page for selling
		if(page==1){
			GUILayout.BeginVertical();
			GUILayout.Label ("Choose something from your bag and sell it to me!", "PlainText");
			GUILayout.EndVertical();
			if(GUI.Button(new Rect(40,260,100,30), "Bye!")){			//Close the window
				GameObject.Find("Main Camera").GetComponent("Bag").SendMessage("VendorBag", "false:"+gameObject.name);
				isActive=false;
				page=0;
			}
			if(GUI.Button(new Rect(160,260,100,30), "Buying")){			//To the page of buying
				page=0;
				GameObject.Find("Main Camera").GetComponent("Bag").SendMessage("VendorBag", "false:"+gameObject.name);
			}
			if(plus==true){
				GUI.skin = main;
				GUI.contentColor = new Color(0.25f,0.25f,0.25f, alpha);
				GUI.Label(new Rect(100,coinsHeight, 100,30), "+"+coins.ToString()+" coins");	
				GUI.skin = g;
			}
		}
	}
	
	void AddSpikes(int winX)
	{
		spikeCount = Mathf.Floor(winX - 152)/22;
		GUILayout.BeginHorizontal();
		GUILayout.Label ("", "SpikeLeft");//-------------------------------- custom
		for (i2 = 0; i2 < spikeCount; i2++)
	    {
			GUILayout.Label ("", "SpikeMid");//-------------------------------- custom
        }
		GUILayout.Label ("", "SpikeRight");//-------------------------------- custom
		GUILayout.EndHorizontal();
	}
	
	IEnumerator PlusCoins(){
		plus=true;
		alpha=1;
		coinsHeight=175;
		for(int d=0; d<100; d++){
			coinsHeight-=0.25f;
			alpha-=0.01f;
			yield return new WaitForSeconds(0.001f);
		}
		plus=false;
	}
	
	void OnTriggerExit(Collider other){													//If Player exits from Vendor's trigger, the window will be closed
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			isActive=false;
			GetComponent("Dialog").SendMessage("Close");
			GameObject.Find("Main Camera").GetComponent("Bag").SendMessage("VendorBag", "false:"+gameObject.name);
		}
	}
}
