/*
Ultimate MMORPG Kit - GameScripts/INFO - Client's script - skeletarik@gmail.com - 2013
This script is very important. It contains all information about Player. It receives it from the "ChooseCharacter" script (INFO <- ChooseCharacter <- Chars <- MySQL Database)
From any script you can get any information you want. Just call "INFO.Return...()" where "..." is the thing you want to know (e.g. INFO.ReturnEmail())
This script must be added on the GameObject called "INFO" in the "MainMenu" scene (the first one). Check you have got only one GameObject with this script.
*/

using UnityEngine;
using System.Collections;

public class INFO : MonoBehaviour {
	
	static string[] inf;			//Received information from the "ChooseCharacter" script (not only from that script)
	static string email;			//Account's email
	static int characters;			//The quantity of characters in one account
	static string game_name;		//Character's name
	static string race;				//Character's race
	static string game_class;		//Character's class
	static int level;				//Character's level
	static int exp;					//Character's experience
	static string bag;				//Character's bag
	static int coins;				//Character's money
	static string weapon;			//Character's weapon
	static string shield;			//Character's shield
	static string quests;			//Character's quest (taken and made)
	static string profession;		//Character's profession
	static int ability1;			//The first character's ability
	static int ability2;			//The second character's ability
		
	void Awake(){
		DontDestroyOnLoad(gameObject);	//Don't destroy that GameObject on load! We need it
	}
	
	void Start () {
		ability1 = 0;
		ability2 = 0;	
	}
	
	public void SetCoins(string c){
		coins=int.Parse(c);	
	}
	
	void GetInfo(string i){				//Receive information from the "ChooseCharacter" script
		inf = i.Split(':');
		game_name=inf[0];
		race=inf[1];
		game_class=inf[2];
		level=int.Parse(inf[3]);
		exp=int.Parse(inf[4]);
		bag=inf[5];
		coins=int.Parse(inf[6]);
		weapon=inf[7];
		shield=inf[8];
		quests=inf[9];
		profession=inf[10];
		
		if(Application.loadedLevelName=="CreateCharacter"){
			GameObject.Find("WEB_Create").GetComponent("Create").SendMessage("GetData", email+":"+game_name+":"+race+":"+game_class+":"+race);	
		}
	}
	
	void GetEmail(string i){
		i = i.Remove(0,1);
		if(i.Split(':')[0]=="Correct"){
			email = i.Split(':')[2];
			characters = int.Parse(i.Split(':')[1]);
		}
	}
	
	void GetCharacters(string i){
		i = i.Remove(0,1);
		string[] s = i.Split('^');
		if(s.Length==2){
			characters = int.Parse(i.Split('^')[1]);
		}
	}
	
	public void DeleteChar(string i){
		GameObject.Find("WEB_Delete").GetComponent("Delete").SendMessage("GetData", email+":"+i);	
	}
	
	public static string ReturnInfo(){
		return game_name+":"+race+":"+game_class+":"+level.ToString()+":"+exp.ToString();
	}
	
	public static string ReturnChars(){
		return characters.ToString();
	}
	
	public static string ReturnEmail(){
		return email;
	}
	
	public static string ReturnCoins(){
		return coins.ToString();
	}
	
	public static string ReturnWeapon(){
		return weapon;
	}
	
	public static string ReturnShield(){
		return shield;
	}
	
	public static string ReturnName(){
		return game_name;
	}
	
	public static int ReturnLevel(){
		return level;
	}
	
	public static string ReturnQuests(){
		return quests;
	}
	
	public static string ReturnProfession(){
		return profession;
	}
	
	public static int ReturnExp(){
		return exp;
	}
	
	public static void SetWeapon(string w){
		weapon = w;
	}
	
	public static void SetShield(string s){
		shield = s;
	}
	
	public static void SetQuests(string q){
		quests = q;
	}
	
	public static void SetProfession(string p){
		profession = p;
	}
	
	public static void PlusExp(int e){
		exp = exp+e;
		if(exp>=level*100){					//If Player has more experience that he needs, he will level up
			int more = exp-(level*100);
			level++;
			exp=0;
			exp+=more;
		}
		GameObject.Find("WEB_Exp").SendMessage("GetData", email+"^"+PhotonNetwork.player.name+"^"+level.ToString()+"^"+exp.ToString());	
		GameObject.Find(PhotonNetwork.player.name+":player/Main Camera").GetComponent("UpdateHealthStat");
	}

	public void ReturnAll(){
		GameObject.Find("WEB_Create").SendMessage("SetAll", email+":"+game_name+":"+race+":"+game_class+":"+level.ToString()+":"+exp.ToString());	
	}
	
	public static void GameStarted(){															//Sending to all main scripts that the game has been started
		GameObject.Find("Main Camera").GetComponent("HeroStats").SendMessage("GameStarted","");
		GameObject.Find("Main Camera").GetComponent("Bag").SendMessage("GameStartedBag", bag);
		GameObject.Find("Main Camera").GetComponent("Equipment").SendMessage("GameStartedEq", game_class+":"+level.ToString()+":"+weapon+":"+shield+":"+race);
		GameObject.Find("Main Camera").GetComponent("QuestWindow").SendMessage("GameStartedQW", quests);
		GameObject.Find("Main Camera").GetComponent("Profession").SendMessage("GameStartedP", profession);
		GameObject.Find("Main Camera").GetComponent("ExpWindow").SendMessage("GameStartedExp");
		GameObject.Find("Main Camera").GetComponent("SpellBook").SendMessage("GameStartedSB");
		GameObject.Find("GUIManager").GetComponent("GUIManager").SendMessage("GameStartedGUI");
	}
	
	public static int ReturnValue(string a){
		if(a=="ability1"){
			return ability1;	
		}else
		if(a=="ability2"){
			return ability2;	
		}else{
			return -1;
		}
	}
	
	public void Cooldown(string a){
		if(a=="ability1"){
			StartCoroutine("a1CD","");
		}
		if(a=="ability2"){
			StartCoroutine("a2CD","");
		}
	}
	
	public IEnumerator a1CD(){
		ability1=1;
		while(ability1!=0){
			yield return new WaitForSeconds(1);
			ability1=ability1-1;
		}
	}
	
	public IEnumerator a2CD(){
		ability2=3;
		while(ability2!=0){
			yield return new WaitForSeconds(1);
			ability2=ability2-1;
		}
	}
}
