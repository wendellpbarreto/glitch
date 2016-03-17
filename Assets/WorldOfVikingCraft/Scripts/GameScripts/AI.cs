/*
Ultimate MMORPG Kit - GameScripts/AI - Client's script - skeletarik@gmail.com - 2013
This script is for enemies. It contains simple AI. You can choose type of the enemy: agressive or not. 
If it is agressive, enemy will attack the Player when he enters in his trigger. You can edit the radius in the inspector.
Enemy has two different attacks: usual hit and critical hit. You can edit the possibility of the critical hit in the inspector (<attack2chance> in the Server!).
If you don't want to use the critical hit, set <attack2chance> to 0. 
For correct work you need to set 4 animations clips. 
Also you can set enemy's rotation, moving and attacking speed in the inspector (the server's values MUST be equal to the client's values!).

NB! It is the client's version of the script. The Server's version of the script also exists. You can find it in the Server folders.
*/

using UnityEngine;
using System.Collections;

public class AI : Photon.MonoBehaviour {

	public bool aggressive;			//Will enemy attack at once when player enters in his trigger?
	public AnimationClip hit;		//Animation clip for usual hit
	public AnimationClip hit2;		//Animation clip for critical hit
	public AnimationClip run;		//Animation clip for run
	public AnimationClip idle;		//Animation clip for idle
	
	Transform enemy;				//Automatically-detected target. 
	
	public int rotationSpeed=1;		//Enemy's rotation speed
	public int movingSpeed=1;		//Enemy's moving speed
	public float speedAttack1=1.0f;	//Enemy's attacking speed
	
	static bool at;					//Is enemy attacking?
	bool numAttack;					//Usual or critical hit
	
	bool oneT;
		
	void OnTriggerEnter(Collider other){						//If player enters in the enemy's trigger, the enemy will attack
		if(other.gameObject.name.Split(':')[1]=="player"){
			if(aggressive==true){
				enemy = other.gameObject.transform;
				at=true;
			}
		}
	}
	
	void OnTriggerExit(Collider other){							//If player exits from the enemy's trigger, the enemy will stop attacking
		if(enemy!=null){
			if(other.gameObject.name == enemy.gameObject.name){
				at=false;
				enemy = null;
			}
		}
	}
	
	void Update(){
		if(at==true && photonView!=null && !HeroStats.booH){
			photonView.RPC("Attack", PhotonTargets.All);
		}
		if(HeroStats.booH){
			oneT=false;
			photonView.RPC("StopAttack", PhotonTargets.MasterClient);
		}
		if(HeroStats.booH==false && !oneT){
			photonView.RPC("AttackAgain", PhotonTargets.MasterClient);
			oneT=true;
		}
	}
	
	[RPC]
	void StopAttack(){
		at=false;	
	}
	
	[RPC]
	void AttackAgain(){
		at=true;	
	}
		
	[RPC]
	void RandomAttack(bool b){
		numAttack=b;				//Receive from Server what type of hit enemy will use
	}
	
	[RPC]
		void Attack(){
			if(GetComponent<Animation>().clip.name!="death"){
				at=false;
				if(enemy!=null){
					/*We use the Viking's model. So, the "real" model is called Viking, and it is a children of the Charprefab. 
					 * If you added your own model, you need to get transform of it.*/
					GameObject trueEnemy = GameObject.Find(enemy.name+"/Viking");
					//Attacking only if the enemy is looking at the player. In other way the enemy will run or turn to the player
					if(Mathf.Abs(transform.position.x-trueEnemy.transform.position.x)<=1.5f && Mathf.Abs(transform.position.z-trueEnemy.transform.position.z)<=1.5f){
						if(speedAttack1 > 0){
							speedAttack1 -= Time.deltaTime;			//Timer
						}
						if(speedAttack1 <= 0){						//Only if it is time for attack, the enemy will attack
							if(numAttack==true){		
								gameObject.GetComponent<Animation>().clip = hit2;	//There the enemy uses usual hit or critical hit. 
							}else{
								gameObject.GetComponent<Animation>().clip = hit;
							}
							gameObject.GetComponent<Animation>().Play();
							StartCoroutine("Ability");
							speedAttack1=1.0f;						//Reset timer
						}
						StartCoroutine("Ability");					//Reset bool
					}else{											//If the enemy isn't looking at the player, the enemy will run or turn to the player
						gameObject.GetComponent<Animation>().clip = run;
						gameObject.GetComponent<Animation>().Play();
						transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(trueEnemy.transform.position - transform.position), rotationSpeed * Time.deltaTime);
						transform.position += transform.forward * movingSpeed * Time.deltaTime;
						at=true;
					}
				}
			}
		}
	
	IEnumerator Ability(){
		at=false;
		yield return new WaitForSeconds(0);
		at=true;
	}
}