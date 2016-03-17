/*
Ultimate MMORPG Kit - GameScripts/Quest - Client's script - skeletarik@gmail.com - 2013
This script is for creating quests. There are a lot of things that you can set. 
For correct work you need to fill all fields in the inspector. 
There are 3 types of quests: killing, collecting and speaking. You can combine them.
<quest_number> is the number of things the Player needs to do (e.g. kill 2 monsters, speak with 3 men and collect 5 flowers - the <quest_number> will be 10 (2+3+5=10))
For item-rewards:
1)	Set <reward_numItems> to the number of item-rewards. 
2)	Add teh scripts of those items.
3)	Drag-and-drop themto the <reward_items>
Also you can set the sound of the made quest.
For more detailed instruction read the Documentation.
*/

using UnityEngine;
using System.Collections;

public class Quest : Photon.MonoBehaviour {
	
	bool made;							//Has this quest already been made?
	bool isActive;						//Is window opened?
	public GUISkin final_skin;			//Good-looking GUISkin
	public GUISkin main;				//Default GUISkin

	public string quest_name;			//The name of the quest
	public string quest_text;			//Quest's text that will display when the Player is taking the quest
	public string quest_textMaking;		//Quest's text that will display when the Player is making the quest
	public string quest_textMade;		//Quest's text that will display when the Player made the quest
	public string quest_textNoQuest;	//Quest's text that will display if the Player has already done the quest
	public int quest_level;				//Necessary Player's level to take this quest
	public int quest_number;			//The number of things the Player needs to do
	public int reward_coins;			//Reward for this quest in coins
	public int reward_numItems;			//How many items will Player take when he makes this quest?
	public int reward_exp;				//Reward for this quest in experience
	public Component[] reward_items;	//Reward for this quest in items
	string[] items_data_end;		
	string[] made_quests;
	int i=0;
	int height=100;
	int cur_number=0;
	bool num_made;
	bool GUIQuest;
	float questHeight;
	float alpha;
	bool empty;
	int page;
	bool curLevel;
	public AudioClip questMade;			//The sound of the made quest
	
	public void OpenQ(){				//Open the window
		isActive=true;
	}
	
	public void CloseQ(){				//Close the window
		isActive=false;
	}
	
	public void PlusNum(){				//If Player do something (kill, speak or collect), <quest_number> will become lower
		if(page!=4){
			cur_number++;	
		}
	}
	
	void Start(){
		items_data_end = new string[reward_numItems];
		reward_items[0].SendMessage("SendStatsQuest");
		if(INFO.ReturnQuests()!=""){
			made_quests = INFO.ReturnQuests().Split('&');					//Receive the information about making and made quest
			foreach(string q in made_quests){
				if(q.Split('$')[0]==quest_name){
					if(q.Split('$')[1]=="making"){
						GetComponent("LabelQ").SendMessage("SetQuest");
						if(INFO.ReturnLevel()>=quest_level){
							GetComponent("LabelQ").SendMessage("ChangeQuestText", "?:0.25:0.25:0.25");	//Set the color and the sign ("?") over the NPC's head
						}
						page=2;
						made=true;
					}else 
					if(q.Split('$')[1]=="made"){
						GetComponent("LabelQ").SendMessage("SetQuest");
						if(INFO.ReturnLevel()>=quest_level){
							GetComponent("LabelQ").SendMessage("ChangeQuestText", ":0.25:0.25:0.25");	//If the quest was made, there would be no sign over NPC's head
						}
						page=4;
						made=true;
					}
				}
			}
		}else{
			made_quests = new string[1];	
			empty=true;
		}
		if(made==false){
			page=0;
			GetComponent("LabelQ").SendMessage("SetQuest");
			if(INFO.ReturnLevel()>=quest_level){
				page=1;
				curLevel=true;
				GetComponent("LabelQ").SendMessage("ChangeQuestText", "!:1:0.92:0.016");		//Set the color and the sign ("!") over the NPC's head
			}else{
				page=4;
			}
		}
	}
	
	void Update(){
		if(cur_number==quest_number && !num_made){
			num_made=true;
			page=3;
			GetComponent("LabelQ").SendMessage("SetQuest");
			if(INFO.ReturnLevel()>=quest_level){
				GetComponent("LabelQ").SendMessage("ChangeQuestText", "?:1:0.92:0.016");		//Set the color and the sign ("?") over the NPC's head
			}
			StartCoroutine("QuestMade");
		}
		if(INFO.ReturnLevel()>=quest_level && !curLevel){
			page=0;
			curLevel=true;		
			GetComponent("LabelQ").SendMessage("ChangeQuestText", "!:1:0.92:0.016");		//Set the color and the sign ("!") over the NPC's head
		}
	}
	
	void LateUpdate(){
		made_quests = INFO.ReturnQuests().Split('&');	
		string final=""	;
			for(int i2 = 0;i2<=made_quests.Length-1;i2++)
				{
					final=final+made_quests[i2]+"&";
				}
		
	}
	
	void OnGUI(){
		GUI.skin = final_skin;
		if(GUIQuest){
			GUI.Label(new Rect(Screen.width/5, questHeight, Screen.width/2, 50), "Quest '"+quest_name+"' has been done!", "LargeText");
		}
		if(isActive){
			GUI.Window(10, new Rect(0,Screen.height/6,300,350), dialogWindow, "");
		}
	}
	
	public void GetStats(string data){
		items_data_end[i]=data;
		i++;
	}
	
	void dialogWindow(int id){
		if(page==0){									//There is <quest_text>
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.Label (quest_name, "BoldText");
			GUILayout.Space(10);
			GUILayout.Label (quest_text, "PlainText");
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("I can't")){
					CloseQ();	
				}
				GUILayout.Space(10);
				if(GUILayout.Button("Next")){
					page++;	
				}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}else
		if(page==1){
			height=125;
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.Label ("Your reward for this quest:", "PlainText");
			GUILayout.EndVertical();
			int i=0;
			foreach(string item in items_data_end){
				if(item==null){
				
				}
				GUI.Box(new Rect(45, height,120,40), "");
				GUI.skin = main;
				if(GUI.Button(new Rect(50, height+5,30,30), (Texture2D)Resources.Load("Items/"+items_data_end[i].Split('!')[7], typeof(Texture2D)), "Texture")){
					
				}
				GUI.contentColor = new Color(0.25f,0.25f,0.25f);
				GUI.Label(new Rect(85, height+4, 100,30), items_data_end[i].Split('!')[0]);
				GUI.Label(new Rect(85, height+5, 100,30), items_data_end[i].Split('!')[2], "Ability");
				GUI.contentColor = Color.white;
				GUI.skin = final_skin;
				height+=45;
				i++;
			}
			GUI.Box(new Rect(45, height,120,40), "");
			GUI.skin = main;
			GUI.contentColor = new Color(0.25f,0.25f,0.25f);	
			GUI.Label(new Rect(70, height+2, 100,30), "+"+reward_coins.ToString()+" coins");
			GUI.Label(new Rect(70, height+16, 100,30), "+"+reward_exp.ToString()+" exp");
			GUI.contentColor = Color.white;
			GUI.skin = final_skin;
			height+=45;
			if(GUI.Button(new Rect(35, height, 120, 25), "No, sorry")){
				page=0;	
			}
			if(GUI.Button(new Rect(150, height, 120, 25), "Ok")){
				page=2;
				isActive=false;
				string final = "";
				for(int i2 = 0;i2<=made_quests.Length-1;i2++)
				{
					final=final+made_quests[i2]+"&";
				}
				final=final+quest_name+"$making";
				GetComponent("LabelQ").SendMessage("SetQuest");
				if(INFO.ReturnLevel()>=quest_level){
					GetComponent("LabelQ").SendMessage("ChangeQuestText", "?:0.25:0.25:0.25");
				}
				INFO.SetQuests(final);
				GameObject.Find("WEB_Quest").GetComponent("QuestWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final);
			}
		}else
		if(page==2){											//There is <quest_textMaking>
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.Label (quest_name, "BoldText");
			GUILayout.Space(10);
			GUILayout.Label (quest_textMaking, "PlainText");
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("Bye")){
					CloseQ();	
				}
				GUILayout.Space(10);
				if(GUILayout.Button("Not yet")){
					CloseQ();
				}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}else	
		if(page==3){											//There is <quest_textMade>
			height=125;
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.Label (quest_textMade, "PlainText");
			GUILayout.EndVertical();
			int i=0;
			foreach(string item in items_data_end){
				if(item==null){
				
				}
				GUI.Box(new Rect(45, height,120,40), "");
				GUI.skin = main;
				if(GUI.Button(new Rect(50, height+5,30,30), (Texture2D)Resources.Load("Items/"+items_data_end[i].Split('!')[7], typeof(Texture2D)), "Texture")){
					
				}
				GUI.contentColor = new Color(0.25f,0.25f,0.25f);
				GUI.Label(new Rect(85, height+4, 100,30), items_data_end[i].Split('!')[0]);
				GUI.Label(new Rect(85, height+5, 100,30), items_data_end[i].Split('!')[2], "Ability");
				GUI.contentColor = Color.white;
				GUI.skin = final_skin;
				height+=45;
				i++;
			}
			GUI.Box(new Rect(45, height,120,40), "");
			GUI.skin = main;
			GUI.contentColor = new Color(0.25f,0.25f,0.25f);	
			GUI.Label(new Rect(70, height+2, 100,30), "+"+reward_coins.ToString()+" coins");
			GUI.Label(new Rect(70, height+16, 100,30), "+"+reward_exp.ToString()+" exp");
			GUI.contentColor = Color.white;
			GUI.skin = final_skin;
			height+=45;
			if(GUI.Button(new Rect(150, height, 120, 25), "Ok")){
				page=2;
				isActive=false;
				if(!empty){
					int iEnd=0;
					foreach(string q in made_quests){
							if(q.Split('$')[0]==quest_name){
								made_quests[iEnd] = quest_name+"$made";
							}		
						iEnd++;				
					}
				}
				string final = "";
				for(int i2 =0;i2<=made_quests.Length-1;i2++)
				{
					final=final+made_quests[i2]+"&";
				}
				if(empty){
					final=final+quest_name+"$made&";
					empty=false;
				}else{
					
				}	
				int iBag=0;
				foreach(string it in items_data_end){
					if(it==null){
				
					}
					if(Bag.ReturnI()<11){
						Bag.PlusI(items_data_end[iBag]);
					}else{
						Debug.LogWarning("Ultimate MMORPG warning: Hero hasn't any empty slots in his bag or enough money!");
					}	
					iBag++;
				}
				INFO.SetQuests(final);
				GameObject.Find("WEB_Quest").GetComponent("QuestWeb").SendMessage("GetData", INFO.ReturnEmail()+"^"+PhotonNetwork.player.name+"^"+final);
				GameObject.Find(PhotonNetwork.player.name+":player/Main Camera").GetComponent("Bag").SendMessage("QuestCoins", reward_coins.ToString());
				INFO.PlusExp(reward_exp);
				GetComponent<AudioSource>().clip = questMade;
				GetComponent<AudioSource>().Play();
				page=4;
				GetComponent("LabelQ").SendMessage("SetQuest");
				if(INFO.ReturnLevel()>=quest_level){
					GetComponent("LabelQ").SendMessage("ChangeQuestText", " :0.25:0.25:0.25");		//No sign over NPC's head
				}
			}
		}else
		if(page==4){												//There is <quest_textNoQuest>
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.Label (quest_textNoQuest, "PlainText");
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("Bye")){
					CloseQ();	
				}
				GUILayout.Space(10);
				if(GUILayout.Button("Ok")){
					CloseQ();
				}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
	}
	
	void OnTriggerExit(Collider other){												//If Player exits from NPC's trigger, the window will be closed
		if(other.gameObject.name.Split(':')[0]==INFO.ReturnInfo().Split(':')[0]){
			CloseQ();
		}
	}
	
	IEnumerator QuestMade(){
		GUIQuest=true;
		alpha=1;
		questHeight=150;
		for(int d=0; d<100; d++){
			questHeight-=0.25f;
			alpha-=0.01f;
			yield return new WaitForSeconds(0.001f);
		}
		GUIQuest=false;
	}
}
