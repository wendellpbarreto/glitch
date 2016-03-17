using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goblin : PlayerAttributes {

	public Goblin()  {
        this.strenght = 10;
        this.dextrery = 10;
        this.vitality = 10;
        this.inteligence = 10;

		this.currentHp = MaxHP ();
		this.currentMp = MaxMP ();

		this.skills = new List<BaseSkill>();
		this.skills.Add (new GoblinRegularAttack(this));
    }
}
