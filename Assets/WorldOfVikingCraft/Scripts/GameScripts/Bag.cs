/*
Ultimate MMORPG Kit - GameScripts/Bag - Client's script - skeletarik@gmail.com - 2013
This script is for Player's bag. In the example it is 12-slot bag. You can set in the inspector only GUISKins, but this script is useful and very important.
Check that in scene you have only one GameObject that contains it.

NB! It is the client's version of the script. The Server's version of the script also exists. You can find it in the Server folders.
*/

using UnityEngine;
using System.Collections;

public class Bag : Photon.MonoBehaviour {

	bool open;						//Is bag open?
	bool started;					//Is the game started?
	public GUISkin g;				//Default skin
	public GUISkin finalSkin;		//Good-looking Skin
	public static string[] items;	//Massive of Player's items
	static int i=-1;				//Quantity of Player's items
	int curI;						
	bool vBag;						//Is Player talking with Vendor?
	string vendor;
	bool use;
	bool book;
	bool weapon;
	string weaponName;
	int weaponSlot;
	string bookText;
	string bookName;
	bool inst;
	static bool exist;
	static int coins;				//Player's money
	
	void Awake(){
		items = new string[12];		//Player has 12-slot bag
	}
	
	public static int ReturnI(){	//Use this function from any script if you want to know how many things Player has
		return i;
	}
	
	public static bool itemExist(string it){			//This function checks if the item exists in the Player's bag. After that this item will be deleted.
		exist=false;
		for(int iE = 0;iE<=i;iE++)
		{
			if(it==items[iE].Split('!')[0]){
				int iR;
				for(iR=iE+1; iR<items.Length-1; iR++){
					items[iR-1]=items[iR];
				}
				i--;
				
				string final = "";
				for(int i2 = 0;i2<=i;i2++)
				{
					final=final+items[i2]+"@";
				}
				GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
				exist=true;
				break;
			}
		}
		if(exist==false){
			return false;	
		}else{
			return true;	
		}
	}
	
	public static void PlusI(string item){		//Use this function if you want to add an item to the bag. Before that check how many items Player has already by using ReturnI()
		i++;
		items[i] = item;
		string final = "";
		for(int i2 = 0;i2<=i;i2++)
		{
			final=final+items[i2]+"@";
		}
		GameObject.Find("WEB_Bag").GetComponent("BagWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final);
	}
	
	public static void PlusIVendor(string item){ 	//Use this function if you want to add an item to the bag from the Vendor.
		i++;										//Before that check how many items Player has already by using ReturnI()
		items[i] = item;
		string final = "";
		for(int i2 = 0;i2<=i;i2++)
		{
			final=final+items[i2]+"@";
		}
		GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+item.Split('!')[3]+"^true");
		coins-=int.Parse(item.Split('!')[3]);
		GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());				
	}
	
	public void VendorBag(string r){				//Is Player begin to talk with Vendor? We need to know that
		if(r.Split(':')[0]=="true"){
			vBag=true;	
		}
		if(r.Split(':')[0]=="false"){
			vBag=false;	
		}
		vendor=r.Split(':')[1];
	}
	
	public void QuestCoins(string c){				//This function adds reward's coins for the made quest
		coins+=int.Parse(c);
		GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());		
		string final = "";
		for(int i2 = 0;i2<=i;i2++)
		{
			final=final+items[i2]+"@";
		}
		GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+c+"^false");			
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.B)){		//Bag is opened/closed if Player press the <B> button on the keyboard
			if(open==false){
				open=true;	
			}else{
				open=false;	
			}
		}
		if(GameObject.Find(PhotonNetwork.player.name+":player")!=null && !inst){
			if(INFO.ReturnWeapon()!="None"){
				photonView.RPC("ChangeChar", PhotonTargets.All, PhotonNetwork.player.name+":weapon:"+INFO.ReturnWeapon());
			}
			if(INFO.ReturnShield()!="None"){
				photonView.RPC("ChangeChar", PhotonTargets.All, PhotonNetwork.player.name+":shield:"+INFO.ReturnShield());
			}
			inst=true;
		}
	}
	
	void GameStartedBag(string b){					//Game is started. Now script needs to display "Bag" button.
		started=true;	
		coins = int.Parse(INFO.ReturnCoins());
		string[] lastItems = b.Split('@');
		int i3;
		for(i3 = 0; i3<lastItems.Length-1; i3++){
			items[i3]=lastItems[i3];
		}
		i=i3-1;
	}
	
	void InfoWindow(int id){						//If the cursor is hovering over the item's image in the bag and the <Left Shift> button is pressed, the script will display
		GUI.Button(new Rect(10,20, 35, 35), (Texture2D)Resources.Load("Items/"+items[curI].Split('!')[7], typeof(Texture2D)), "Texture");		//all information about that item
		if(items[curI].Split('!')[1]=="bad"){
			GUI.contentColor = new Color(0.768f,0.768f,0.768f);
		}
		if(items[curI].Split('!')[1]=="normal"){
			GUI.contentColor = new Color(0,0.807f,0);
		}
		if(items[curI].Split('!')[1]=="rare"){
			GUI.contentColor = new Color(0.3137f,0.180f,0.933f);
		}
		GUI.Label(new Rect(55,25, 100,30), items[curI].Split('!')[0]);
		GUI.contentColor = Color.white;
		if(items[curI].Split('!')[2]=="Weapon" || items[curI].Split('!')[2]=="Armor"){
			if(items[curI].Split('!')[2]=="Weapon"){
				GUI.Label(new Rect(10,60,150,30), "Damage: +"+items[curI].Split('!')[4]);
			}
			GUI.Label(new Rect(10,75,150,30), "Strength: +"+items[curI].Split('!')[5]);
			GUI.Label(new Rect(10,90,150,30), "Stamina: +"+items[curI].Split('!')[6]);
		}
		GUI.Label(new Rect(10,115, 150,30), "Cost: "+items[curI].Split('!')[3]+" coins");
	}
	
	void readBook(int id){							//Window for reading book's text
		GUILayout.BeginVertical();
		GUILayout.Space(40);
		GUILayout.Label(bookName);
		GUILayout.Label(bookText, "PlainText");
		GUILayout.EndVertical();
		GUILayout.Space(10);
		if(GUILayout.Button("Close")){
			book=false;	
		}
	}
	
	void equipW(int id){							//If Player wants to equip a weapon or a shield, he needs to choose on which hand it will be equipped
		GUILayout.BeginVertical();
		GUILayout.Space(40);
		GUILayout.Label(weaponName);
		GUILayout.Label("\t\tOn which hand do you want to equip it?", "PlainText");
		GUILayout.Space(10);
		GUILayout.BeginHorizontal();	
		if(GUILayout.Button("Left")){
			GetComponent("Equipment").SendMessage("Equip", "right:"+weaponName);
			StartCoroutine("NotBuy");
			int iR;
			for(iR=weaponSlot+1; iR<items.Length-1; iR++){
				items[iR-1]=items[iR];
			}
			i--;
			string final = "";
			for(int i2 = 0;i2<=i;i2++)
			{
				final=final+items[i2]+"@";
			}
			GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
			GameObject.Find("WEB_Equip").GetComponent("EquipWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^right^"+weaponName);
			photonView.RPC("ChangeChar", PhotonTargets.All, PhotonNetwork.player.name+":weapon:"+weaponName);
			weapon=false;
		}
		GUILayout.Space(10);
		if(GUILayout.Button("Right")){
			GetComponent("Equipment").SendMessage("Equip", "left:"+weaponName);
			StartCoroutine("NotBuy");
			int iR;
			for(iR=weaponSlot+1; iR<items.Length-1; iR++){
				items[iR-1]=items[iR];
			}
			i--;
			string final = "";
			for(int i2 = 0;i2<=i;i2++)
			{
				final=final+items[i2]+"@";
			}
			GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
			GameObject.Find("WEB_Equip").GetComponent("EquipWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^left^"+weaponName);
			photonView.RPC("ChangeChar", PhotonTargets.All, PhotonNetwork.player.name+":shield:"+weaponName);
			weapon=false;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}
	
	void OnGUI(){
		GUI.skin = g;
		if(started==true){
			if(book){					//If Player click on the image of the book, the script will show the book's text
				GUI.skin=finalSkin;
				GUI.Window(20, new Rect(Screen.width/2.5f, Screen.height/8, Screen.width/4, Screen.height/1.5f), readBook, "");
				GUI.skin=g;
			}
			if(weapon){					//If Player click on the image of the weapon or the shield, the script will ask on which hand it will be equipped
				GUI.skin=finalSkin;
				GUI.Window(20, new Rect(Screen.width/2.5f, Screen.height/8, Screen.width/4, Screen.height/3), equipW, "", "Window2");
				GUI.skin=g;
			}
			GUI.skin=finalSkin;
			if(GUI.Button(new Rect(Screen.width-70,Screen.height-18,70,22), "Bag")){	//If Player press the "Bag" button, the script will open/close the bag
				if(open==false){
					open=true;	
				}else{
					open=false;	
				}
			}
			GUI.skin=g;
			if(open){
				//12 slots
				GUI.skin=finalSkin;
				GUI.Box(new Rect(Screen.width-225,Screen.height-225, 240, 225), "", "Window2");	
				GUI.skin=g;
				GUI.Label(new Rect(Screen.width-120,Screen.height-185, 200, 165), "Bag", "BagLabel");
				GUI.Label(new Rect(Screen.width-105,Screen.height-43, 200, 165), "Coins: "+coins.ToString(), "BagLabel");
				
				//All slots are similiar, so only one is commented 
				
				//First line (4)
				
				GUI.Box(new Rect(Screen.width-190,Screen.height-160, 35, 35), "");		
				if(i>-1){
					GUI.Button(new Rect(Screen.width-190,Screen.height-160, 35, 35), (Texture2D)Resources.Load("Items/"+items[0].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot1 = new Rect(Screen.width-190,Screen.height-160, 35, 35);
      			  	if (slot1.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){		//If cursor is hovering over this item and the <Left Shift> is pressed,
        				GUI.Window(0, new Rect(Screen.width-340,Screen.height-310,150,150), InfoWindow, items[0].Split('!')[2]);	//the script will show all information about that item
						curI = 0;
					}
					//If Player doesn't talking with Vendor and right-mouse button pressed
					if (slot1.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[0].Split('!')[2]=="Food"){										//If it is food, it will be deleted and the current health (or energy) will increase							
							UpdateHealth(int.Parse(items[0].Split('!')[8]));
							UpdateEnergy(int.Parse(items[0].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=1; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[0].Split('!')[2]=="Book"){									//If it is book, we will see the book's text
							book=true;
							bookText=items[0].Split('!')[10];
							bookName=items[0].Split('!')[0];
						}else
						if(items[0].Split('!')[2]=="Weapon"){								//If it is weapon or shield, we will equip it
							weapon=true;
							weaponName=items[0].Split('!')[0];
							weaponSlot=0;
						}
						
							
					}
					//If Player is talking with Vendor and right-mouse button pressed, the item will be sold
					if (slot1.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						vBag=false;
						string coinsS;
						coinsS = items[0].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[0]);
				
						int iR;
						for(iR=1; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-145,Screen.height-160, 35, 35), "");
				if(i>0){
					GUI.Button(new Rect(Screen.width-145,Screen.height-160, 35, 35), (Texture2D)Resources.Load("Items/"+items[1].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot2 = new Rect(Screen.width-145,Screen.height-160, 35, 35);
      			  	if (slot2.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-295,Screen.height-310,150,150), InfoWindow, items[1].Split('!')[2]);
						curI = 1;
					}
					if (slot2.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[1].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[1].Split('!')[8]));
							UpdateEnergy(int.Parse(items[1].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=2; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[1].Split('!')[2]=="Book"){
							book=true;
							bookText=items[1].Split('!')[10];
							bookName=items[1].Split('!')[0];
						}else
						if(items[1].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[1].Split('!')[0];
							weaponSlot=1;
						}
							
					}
					if (slot2.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[1].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[1]);
				
						int iR;
						for(iR=2; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-100,Screen.height-160, 35, 35), "");
				if(i>1){
					GUI.Button(new Rect(Screen.width-100,Screen.height-160, 35, 35), (Texture2D)Resources.Load("Items/"+items[2].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot3 = new Rect(Screen.width-100,Screen.height-160, 35, 35);
      			  	if (slot3.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-250,Screen.height-310,150,150), InfoWindow, items[2].Split('!')[2]);
						curI = 2;
					}
					if (slot3.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[2].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[2].Split('!')[8]));
							UpdateEnergy(int.Parse(items[2].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=3; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[2].Split('!')[2]=="Book"){
							book=true;
							bookText=items[2].Split('!')[10];
							bookName=items[2].Split('!')[0];
						}else
						if(items[2].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[2].Split('!')[0];
							weaponSlot=2;
						}
							
					}
					if (slot3.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[2].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[2]);
				
						int iR;
						for(iR=3; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-55,Screen.height-160, 35, 35), "");
				if(i>2){
					GUI.Button(new Rect(Screen.width-55,Screen.height-160, 35, 35), (Texture2D)Resources.Load("Items/"+items[3].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot4 = new Rect(Screen.width-55,Screen.height-160, 35, 35);
      			  	if (slot4.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-205,Screen.height-310,150,150), InfoWindow, items[3].Split('!')[2]);
						curI = 3;
					}
					if (slot4.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[3].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[3].Split('!')[8]));
							UpdateEnergy(int.Parse(items[3].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=4; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[3].Split('!')[2]=="Book"){
							book=true;
							bookText=items[3].Split('!')[10];
							bookName=items[3].Split('!')[0];
						}else
						if(items[3].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[3].Split('!')[0];
							weaponSlot=3;
						}
							
					}
					if (slot4.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[3].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[3]);
				
						int iR;
						for(iR=4; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				
				//Second line (4)
				
				GUI.Box(new Rect(Screen.width-190,Screen.height-120, 35, 35), "");
				if(i>3){
					GUI.Button(new Rect(Screen.width-190,Screen.height-120, 35, 35), (Texture2D)Resources.Load("Items/"+items[4].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot5 = new Rect(Screen.width-190,Screen.height-120, 35, 35);
      			  	if (slot5.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-340,Screen.height-270,150,150), InfoWindow, items[4].Split('!')[2]);
						curI = 4;
					}
					if (slot5.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[4].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[4].Split('!')[8]));
							UpdateEnergy(int.Parse(items[4].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=5; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[4].Split('!')[2]=="Book"){
							book=true;
							bookText=items[4].Split('!')[10];
							bookName=items[4].Split('!')[0];
						}else
						if(items[4].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[4].Split('!')[0];
							weaponSlot=4;
						}
							
					}
					if (slot5.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[4].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[4]);
				
						int iR;
						for(iR=5; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-145,Screen.height-120, 35, 35), "");	
				if(i>4){
					GUI.Button(new Rect(Screen.width-145,Screen.height-120, 35, 35), (Texture2D)Resources.Load("Items/"+items[5].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot6 = new Rect(Screen.width-145,Screen.height-120, 35, 35);
      			  	if (slot6.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-295,Screen.height-270,150,150), InfoWindow, items[5].Split('!')[2]);
						curI = 5;
					}
					if (slot6.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[5].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[5].Split('!')[8]));
							UpdateEnergy(int.Parse(items[5].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=6; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[5].Split('!')[2]=="Book"){
							book=true;
							bookText=items[5].Split('!')[10];
							bookName=items[5].Split('!')[0];
						}else
						if(items[5].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[5].Split('!')[0];
							weaponSlot=5;
						}
							
					}
					if (slot6.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[5].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[5]);
				
						int iR;
						for(iR=6; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-100,Screen.height-120, 35, 35), "");
				if(i>5){
					GUI.Button(new Rect(Screen.width-100,Screen.height-120, 35, 35), (Texture2D)Resources.Load("Items/"+items[6].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot7 = new Rect(Screen.width-100,Screen.height-120, 35, 35);
      			  	if (slot7.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-250,Screen.height-270,150,150), InfoWindow, items[6].Split('!')[2]);
						curI = 6;
					}
					if (slot7.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[6].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[6].Split('!')[8]));
							UpdateEnergy(int.Parse(items[6].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=7; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[6].Split('!')[2]=="Book"){
							book=true;
							bookText=items[6].Split('!')[10];
							bookName=items[6].Split('!')[0];
						}else
						if(items[6].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[6].Split('!')[0];
							weaponSlot=6;
						}
							
					}
					if (slot7.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[6].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[6]);
				
						int iR;
						for(iR=7; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-55,Screen.height-120, 35, 35), "");
				if(i>6){
					GUI.Button(new Rect(Screen.width-55,Screen.height-120, 35, 35), (Texture2D)Resources.Load("Items/"+items[7].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot8 = new Rect(Screen.width-55,Screen.height-120, 35, 35);
      			  	if (slot8.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-205,Screen.height-270,150,150), InfoWindow, items[7].Split('!')[2]);
						curI = 7;
					}
					if (slot8.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[7].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[7].Split('!')[8]));
							UpdateEnergy(int.Parse(items[7].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=8; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[7].Split('!')[2]=="Book"){
							book=true;
							bookText=items[7].Split('!')[10];
							bookName=items[7].Split('!')[0];
						}else
						if(items[7].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[7].Split('!')[0];
							weaponSlot=7;
						}
							
					}
					if (slot8.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[7].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[7]);
				
						int iR;
						for(iR=8; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				
				//Third line (4)
				
				GUI.Box(new Rect(Screen.width-190,Screen.height-80, 35, 35), "");
				if(i>7){
					GUI.Button(new Rect(Screen.width-190,Screen.height-80, 35, 35), (Texture2D)Resources.Load("Items/"+items[8].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot9 = new Rect(Screen.width-190,Screen.height-80, 35, 35);
      			  	if (slot9.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-340,Screen.height-230,150,150), InfoWindow, items[8].Split('!')[2]);
						curI = 8;
					}
					if (slot9.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[8].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[8].Split('!')[8]));
							UpdateEnergy(int.Parse(items[8].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=9; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[8].Split('!')[2]=="Book"){
							book=true;
							bookText=items[8].Split('!')[10];
							bookName=items[8].Split('!')[0];
						}else
						if(items[8].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[8].Split('!')[0];
							weaponSlot=8;
						}
							
					}
					if (slot9.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[8].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[8]);
				
						int iR;
						for(iR=9; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-145,Screen.height-80, 35, 35), "");	
				if(i>8){
					GUI.Button(new Rect(Screen.width-145,Screen.height-80, 35, 35), (Texture2D)Resources.Load("Items/"+items[9].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot10 = new Rect(Screen.width-145,Screen.height-80, 35, 35);
      			  	if (slot10.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-295,Screen.height-230,150,150), InfoWindow, items[9].Split('!')[2]);
						curI = 9;
					}
					if (slot10.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[9].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[9].Split('!')[8]));
							UpdateEnergy(int.Parse(items[9].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=10; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[9].Split('!')[2]=="Book"){
							book=true;
							bookText=items[9].Split('!')[10];
							bookName=items[9].Split('!')[0];
						}else
						if(items[9].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[9].Split('!')[0];
							weaponSlot=9;
						}
							
					}
					if (slot10.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[9].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[9]);
				
						int iR;
						for(iR=10; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-100,Screen.height-80, 35, 35), "");
				if(i>9){
					GUI.Button(new Rect(Screen.width-100,Screen.height-80, 35, 35), (Texture2D)Resources.Load("Items/"+items[10].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot11 = new Rect(Screen.width-100,Screen.height-80, 35, 35);
      			  	if (slot11.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-250,Screen.height-230,150,150), InfoWindow, items[10].Split('!')[2]);
						curI = 10;
					}
					if (slot11.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[10].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[10].Split('!')[8]));
							UpdateEnergy(int.Parse(items[10].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=11; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[10].Split('!')[2]=="Book"){
							book=true;
							bookText=items[10].Split('!')[10];
							bookName=items[10].Split('!')[0];
						}else
						if(items[10].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[10].Split('!')[0];
							weaponSlot=10;
						}
							
					}
					if (slot11.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[10].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[10]);
				
						int iR;
						for(iR=11; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
				GUI.Box(new Rect(Screen.width-55,Screen.height-80, 35, 35), "");
				if(i>10){
					GUI.Button(new Rect(Screen.width-55,Screen.height-80, 35, 35), (Texture2D)Resources.Load("Items/"+items[11].Split('!')[7], typeof(Texture2D)), "Texture");
					Rect slot12 = new Rect(Screen.width-55,Screen.height-80, 35, 35);
      			  	if (slot12.Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){
        				GUI.Window(0, new Rect(Screen.width-205,Screen.height-230,150,150), InfoWindow, items[11].Split('!')[2]);
						curI = 11;
					}
					if (slot12.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && !vBag && !use){
        				if(items[11].Split('!')[2]=="Food"){
							UpdateHealth(int.Parse(items[11].Split('!')[8]));
							UpdateEnergy(int.Parse(items[11].Split('!')[9]));
							StartCoroutine("NotBuy");
							int iR;
							for(iR=12; iR<items.Length-1; iR++){
								items[iR-1]=items[iR];
							}
							i--;
							string final = "";
							for(int i2 = 0;i2<=i;i2++)
							{
								final=final+items[i2]+"@";
							}
							GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^0^false");
							
						}else
						if(items[11].Split('!')[2]=="Book"){
							book=true;
							bookText=items[11].Split('!')[10];
							bookName=items[11].Split('!')[0];
						}else
						if(items[11].Split('!')[2]=="Weapon"){
							weapon=true;
							weaponName=items[11].Split('!')[0];
							weaponSlot=11;
						}
							
					}
					if (slot12.Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(1) && vBag){
						string coinsS;
						coinsS = items[11].Split('!')[3];
						coins+=int.Parse(coinsS);
						GameObject.Find("INFO").GetComponent("INFO").SendMessage("SetCoins", coins.ToString());
						StartCoroutine("Buy");
        				GameObject.Find(vendor+":NPC").GetComponent("Vendor").SendMessage("Sell", items[11]);
				
						int iR;
						for(iR=12; iR<items.Length-1; iR++){
							items[iR-1]=items[iR];
						}
						i--;
						
						string final = "";
						for(int i2 = 0;i2<=i;i2++)
						{
							final=final+items[i2]+"@";
						}
						GameObject.Find("WEB_Vendor").GetComponent("VendorWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final+"^"+coinsS+"^false");
						
					}
				}
				
			}
		}
	}
	
	IEnumerator Buy(){
		vBag=false;
		use=true;
		yield return new WaitForSeconds(0.5f);
		vBag=true;
		use=false;
	}
	
	IEnumerator NotBuy(){
		use=true;
		yield return new WaitForSeconds(0.5f);
		use=false;
	}
	
	
	
	void UpdateHealth(int h){
		Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonNetwork.player.customProperties["Game_race"] }, 
						{ "Game_class", PhotonNetwork.player.customProperties["Game_class"] }, 
						{ "Level", PhotonNetwork.player.customProperties["Level"] }, 
						{ "Health", float.Parse(PhotonNetwork.player.customProperties["Health"].ToString())+h}, 
						{ "Energy", PhotonNetwork.player.customProperties["Energy"]} };
						PhotonNetwork.player.SetCustomProperties(prop);	
	}
	
	void UpdateEnergy(int h){
		Hashtable prop = new Hashtable() 
						{ { "Game_race", PhotonNetwork.player.customProperties["Game_race"] }, 
						{ "Game_class", PhotonNetwork.player.customProperties["Game_class"] }, 
						{ "Level", PhotonNetwork.player.customProperties["Level"] }, 
						{ "Health", PhotonNetwork.player.customProperties["Health"] }, 
						{ "Energy", float.Parse(PhotonNetwork.player.customProperties["Energy"].ToString())+h} };
						PhotonNetwork.player.SetCustomProperties(prop);	
	}
	
	[RPC]
	public void ChangeChar(string c){
		if(c.Split(':')[1]=="weapon"){
			GameObject.Find(c.Split(':')[0]+":player/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigLArmCollarbone/CATRigLArm1/CATRigLArm2/CATRigLArmPalm/"+c.Split(':')[2]).GetComponent<Renderer>().enabled = true;
		}
		if(c.Split(':')[1]=="shield"){
			GameObject.Find(c.Split(':')[0]+":player/Viking/CATRigHub001/CATRigSpineCATRigSpine1/CATRigSpine2/CATRigHub002/CATRigRArmCollarbone/CATRigRArm1/CATRigRArm2/CATRigRArmPalm/"+c.Split(':')[2]).GetComponent<Renderer>().enabled = true;
		}
	}
}
