/*
Ultimate MMORPG Kit - GameScripts/SpellBook - Client's script - skeletarik@gmail.com - 2013
This script is for Player's spells (skills). Player can open this window by clicking on the "Spells" button 
or using the "K" key. Also he can Drag&Drop them to the skills panel.
Spells can be set in the SpellEditor. Look ReadMe, WhatsNew files and videos on YouTube for more detailed information.
*/
using UnityEngine;
using System.Collections;

public class SpellBook : MonoBehaviour {

	public static bool show;						//Is window opened?
	public GUISkin finalSkin;		//Good-looking GUISkin
	public GUISkin defaultSkin;		//Default GUISkin
	
	bool started;					//Is the game started?
	string pathS;
	string[] spells;
	public static string[] fitLvl;
	string[] highLvl;
	string allInfo;
	int i=0;
	int hL;
	public static int fL;
	int curS=-1;
	public static Rect dragOb;
	Rect curOb;
	int width;
	int height;
	public static bool dragging;
	public static int dragS;
	
	void GameStartedSB(){			//Receive information about Player's profession
		started=true;
		pathS = Application.dataPath+"/UMK_Files/SpellFileC.umk";
		spells = new string[Crypt.lineCount(pathS)];
		WriteAllLines();
		fitLvl=new string[spells.Length];
		highLvl=new string[spells.Length];
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.K)){			//If Player presses the <K> button, spells' window will be opened/closed
			if(show==false){
				show=true;	
			}else{
				show=false;	
			}
		}
	}
	
	void OnGUI(){
		if(started){
			hL=0;
				fL=0;
				for(int iS=1; iS<spells.Length;iS++){
					if(int.Parse(spells[iS].Split(';')[4])>INFO.ReturnLevel()){
						highLvl[hL]=spells[iS];
						hL++;
					}else{
						fitLvl[fL]=spells[iS];
						fL++;
					}
					
				}
			GUI.skin=finalSkin;
			if(GUI.Button(new Rect(Screen.width-340,Screen.height-40,125,22), "Spells")){		//If Player presses the "Profession" button,
				if(show==false){																	//the profession's window will be opened/closed
					show=true;	
				}else{
					show=false;	
				}
			}
			if(show){
				GUI.depth=101;
				GUI.Window(101, new Rect(100, Screen.height/6,700,350), spellsWindow, "");	
				GUI.depth=0;
			}else{
				dragging=false;	
			}
			if(dragging){
				GUI.skin=defaultSkin;
				int di = GUI.depth;
				GUI.depth=2;
				GUI.Button(dragOb,(Texture2D)Resources.Load("Spells/"+fitLvl[dragS].Split(';')[6],typeof(Texture2D)),"Box");
				GUI.skin=finalSkin;
				GUI.depth=di;
			}
			if(Event.current.type == EventType.MouseDown && dragging){
    							dragging=false;
							}
		}
	}
	
	void spellsWindow(int id){
				
				height=100;
				width=50;
				for(i=0; i<24;i=i){
					
					int i2=0;
					width=50;
					for(i2=0; i2<8;i2++){
						dragOb=new Rect(width-360 + Event.current.mousePosition.x, height-Screen.height/6 + Event.current.mousePosition.y,25,25);
						curOb=new Rect(width,height,50,50);
						if(i>23){
							break;
						}
						if(i<fL){
							if(Event.current.type == EventType.MouseDown && curOb.Contains(Event.current.mousePosition) && !dragging){
    							dragging=true;
							}
							if(GUI.Button(curOb,(Texture2D)Resources.Load("Spells/"+fitLvl[i].Split(';')[6],typeof(Texture2D)),"Box")){
								curS=i;
								dragS=curS;
							}
							
						}else{
							GUI.Button(curOb,"","Box");
						}
						i++;
						width+=60;
					}
					height+=60;
				}
		
		GUILayout.BeginArea(new Rect(100,0,700,350));
			GUILayout.BeginVertical();
			GUILayout.Space(30);
			GUILayout.Label("Spell Book",GUILayout.Width(700));
			GUILayout.Space(50);
			GUILayout.BeginArea(new Rect(60,100,670,500));
				
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(550,100,100,500));
				if(curS!=-1){
					GUILayout.Label(fitLvl[curS].ToString().Split(';')[5],"CursedText");
				}
				GUILayout.Space(5);
				if(curS!=-1){
					GUILayout.Label("Type:  "+((SpellTypes)int.Parse(fitLvl[curS].ToString().Split(';')[2])).ToString(),"PlainText");
				}
				GUILayout.Space(5);	
				if(curS!=-1){
					GUILayout.Label("Strength:  "+int.Parse(fitLvl[curS].ToString().Split(';')[1]).ToString(),"PlainText");
				}
				GUILayout.Space(5);
				if(curS!=-1){
					GUILayout.Label("Description:","PlainText");
					GUILayout.TextArea(fitLvl[curS].ToString().Split(';')[8],"PlainText");
				}
			GUILayout.EndArea();
		GUILayout.EndArea();
		
		
	}
	
	void WriteAllLines(){
		string str2;
		System.IO.StreamReader file = new System.IO.StreamReader(pathS);
		str2 = Crypt.Decrypt(file.ReadToEnd(),Crypt.password1,Crypt.password2,"SHA1",2,"16CHARSLONG12345",256);		
		spells = str2.Split('\n');
		
		file.Close();	
	}
}
