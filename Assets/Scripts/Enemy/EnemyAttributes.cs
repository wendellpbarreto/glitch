using UnityEngine;
using System.Collections;

public class EnemyAttributes : BaseAttributes {

	public EnemyAttributes(){
		this.strenght = 10;
		this.dextrery = 10;
		this.vitality = 10;
		this.inteligence = 10;

		this.currentHp = MaxHP ();
		this.currentMp = MaxMP ();
	}

	public float GetDamage(){
		return 100;
	}
		
	public void TakeDamage(float damage){
		this.currentHp -= damage;
	}
}
