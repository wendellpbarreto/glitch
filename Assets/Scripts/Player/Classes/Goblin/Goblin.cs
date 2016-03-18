using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using KiiCorp.Cloud.Storage;

public class Goblin : PlayerAttributes {

	public Goblin()  {
		this.strenght = 10f;
		this.dextrery = 10f;
		this.vitality = 10f;
		this.inteligence = 10f;

		this.currentHp = MaxHP ();
		this.currentMp = MaxMP ();

		this.skills = new List<BaseSkill>();
		this.skills.Add (new GoblinRegularAttack(this));
	}
}
