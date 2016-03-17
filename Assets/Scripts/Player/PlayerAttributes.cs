using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttributes : BaseAttributes {

	protected List<BaseSkill> skills;

	public List<BaseSkill> GetSkills(){
		return skills;
	}

	public void TakeDamage(float damage){
		this.currentHp -= damage;
	}
}
