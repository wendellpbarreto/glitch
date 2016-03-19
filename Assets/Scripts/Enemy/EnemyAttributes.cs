using UnityEngine;
using System.Collections;

public class EnemyAttributes : BaseAttributes {

	public float currentHp;
	public float currentMp;

	public EnemyAttributes(){
		this.strenght = 10;
		this.dextrery = 10;
		this.vitality = 10;
		this.inteligence = 10;

		this.currentHp = MaxHP ();
		this.currentMp = MaxMP ();
	}

	public float GetDamage(){
		return 1;
	}

	public float MaxHP() {
		return this.vitality * 1000;
	}

	public float MaxMP() {
		return this.inteligence * 100;
	}

	public bool IsAlive(){
		return this.currentHp > 0;
	}
		
	public void TakeDamage(float damage){
		this.currentHp -= damage;
	}
}
