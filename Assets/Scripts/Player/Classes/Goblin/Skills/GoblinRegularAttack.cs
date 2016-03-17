using UnityEngine;
using System.Collections;

public class GoblinRegularAttack : BaseSkill {
	private Goblin owner;

	public GoblinRegularAttack (Goblin owner){
		this.owner = owner;
		this.cooldown = 1f;
	}

	public override float GetDamage () {
		return owner.Dextrery * 10;
	}
	public override float GetRange () {
		return 5;
	}
	public override string GetAnimationName(){
		return "attack3";
	}
}
