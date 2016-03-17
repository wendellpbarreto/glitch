/*
Ultimate MMORPG Kit - GameScripts/Profession - Client's script - skeletarik@gmail.com - 2013
This script is for Player's profession. At the beginning he won't have any profession. But he will be able to learn it from the NPCs that will have the <CraftLearn> script.
If you want to make a custom recipes, you need to follow this steps:
	1)	Set <quantityI> to the number of all items that will take part in the professions.
	2)	Add all items' scripts that will take part in the professions.
	3)	Drag-and-drop these scripts to the <items>
	4)	Set <quantityCraftI> to the number of items that you can make
	5)	For each item you can make add "CraftItem" script and fill their fields
	6)	Drag-and-drop these "CraftItem" scripts to the <craftItems>
Also you can add your own sound of craft.
For more detailed instruction read the Documentation.
*/

using UnityEngine;
using System.Collections;

public class Profession : MonoBehaviour {
	bool show;						//Is window opened?
	public GUISkin finalSkin;		//Good-looking GUISkin
	public GUISkin defaultSkin;		//Default GUISkin
	string profession;				//Auto-detecting Player's profession (receive from "INFO" script)
	bool started;					//Is the game started?
	bool haveProf;					//Has Player any profession?
	int prof_level;					//Level of Player's profession
	int i=0;
	int iD=0;
	int height;
	int max_prof=75;
	bool worldItem;
	bool use;
	
	public Component[] items;		//All items (materials and items that you can create)
	public int quantityI;			//Number of all items
	public Component[] craftItems;	//Items that you can create
	public int quantityCraftI;		//Number of items you can create
	string[] items_data_end;			
	string[] craftItems_data;
	public AudioClip craftSound;	//Sound of craft
	
	public void GetCraftItems(string c){	//Receive information about items you can create
		craftItems_data[i]=c;
		i++;
	}
	
	public void WorldItem(string w){		//Player can create new items only if he is near the workshop
		if(w=="true"){					
			worldItem=true;
		}
		if(w=="false"){
			worldItem=false;
		}
	}
	
	public void GetStats(string data){		//Receive information about all items
		items_data_end[iD]=data;
		iD++;
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.P)){	//If Player presses the <P> button, the profession's window will be opened/closed
			if(show==false){
				show=true;	
			}else{
				show=false;	
			}
		}
	}
	
	void GameStartedP(string p){			//Receive information about Player's profession
		started=true;
		if(p.Length>3){
			haveProf=true;
			profession = p.Split('%')[0];
			prof_level = int.Parse(p.Split('%')[1]);
			
		}
		items_data_end = new string[quantityI];	
		craftItems_data = new string[quantityCraftI];	
		items[0].SendMessage("SendStatsCraft");
		GetComponent("CraftItem").SendMessage("ReturnCraftInfo");
	}
	
	public void UpdateProf(string p){		//If Player create new item, his profession's level will increase
		if(p.Length>3){
			haveProf=true;
			profession = p.Split('%')[0];
			prof_level = int.Parse(p.Split('%')[1]);
		}
	}
	
	void OnGUI(){
		if(started){
			GUI.skin=finalSkin;
			if(GUI.Button(new Rect(Screen.width-385,Screen.height-18,125,22), "Profession")){		//If Player presses the "Profession" button,
				if(show==false){																	//the profession's window will be opened/closed
					show=true;	
				}else{
					show=false;	
				}
			}
			if(show){
				GUI.Window(50, new Rect(0, Screen.height/6,300,350), profWindow, "");	
			}
		}
	}
	
	void profWindow(int id){
		GUILayout.BeginVertical();
		GUILayout.Space(50);
		if(!haveProf){
			GUILayout.Label("Your Profession");			//If Player hasn't any profession, the script will show only window's title
		}else{
			GUILayout.Label(profession);
		}
		if(haveProf){
			GUI.skin = defaultSkin;
			GUI.Box(new Rect(50,140,200,25), "");
			if(prof_level>0){
				GUI.Box(new Rect(53,143,194*(prof_level/(float)max_prof),19), "");		//Display profession's level
			}
			GUI.Label(new Rect(50,140,200,25), prof_level.ToString()+"/"+max_prof.ToString(), "center");
			GUI.skin = finalSkin;
			height=170;
			Rect[] r = new Rect[craftItems_data.Length];
			int cI=0;
			foreach(string craftData in craftItems_data){
				if(craftData.Split('@')[2]==profession){
					r[cI] = new Rect(50,height,200,25);
					if (r[cI].Contains(Event.current.mousePosition) && Input.GetKey(KeyCode.LeftShift)){	//If Player presses the <Left Shift> button and
						GUI.Window(cI, new Rect(260,170,150,150), InfoWindow, "", "Window2");				//the cursor is hovering over the recipe, the script will show
					}																						//the information about creating that item
					if (r[cI].Contains(Event.current.mousePosition) && Input.GetMouseButtonDown(0) && worldItem && !use){
						StartCoroutine("Timer");
						if(Bag.ReturnI()<11){
							string[] needItems = craftData.Split('@')[0].Split(',');
							int things=0;
							int iCI=0;
							foreach(string it in needItems){
								if(it==null){
				
								}
								if(Bag.itemExist(needItems[iCI])==true){
									things++;
								}
								iCI++;
							}
							if(things>=needItems.Length-1){
								Bag.PlusI(Items.ReturnItem(craftData.Split('@')[1]));
								if(prof_level+1<=max_prof){
									prof_level++;
								}
								GetComponent<AudioSource>().clip=craftSound;
								GetComponent<AudioSource>().Play();
								GameObject.Find("WEB_Prof").GetComponent("ProfWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+profession+"%"+prof_level.ToString());
							}else{
								Debug.LogWarning("Ultimate MMORPG Kit warning: Player hasn't enough items to create this thing!");	
							}
						}
					}
					GUI.Box(new Rect(50,height,200,25), "");
					GUI.skin = defaultSkin;
					GUI.contentColor = new Color(0.25f,0.25f,0.25f);
					GUI.Label(new Rect(50,height,200,25), craftData.Split('@')[1], "center");
					GUI.contentColor = Color.white;
					GUI.skin = finalSkin;
					cI++;
					height+=25;
				}
			}
		}
		GUILayout.EndVertical();
		
	}
	
	void InfoWindow(int id){													//Display the informaton about creating the item
		int infHeight=30;
		string[] materials = craftItems_data[id].Split('@')[0].Split(',');
		foreach(string m in materials){
			GUI.skin = defaultSkin;
			GUI.contentColor = new Color(0.25f,0.25f,0.25f);
			GUI.Label(new Rect(40, infHeight, 150,150), m);
			GUI.contentColor = Color.white;
			GUI.skin = finalSkin;
			infHeight+=20;
		}
	}
	
	IEnumerator Timer(){
		use=true;
		yield return new WaitForSeconds(0.5f);
		use=false;
	}
}
