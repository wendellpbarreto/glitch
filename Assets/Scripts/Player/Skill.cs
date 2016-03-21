﻿using System;


public class Skill {
	public string id;
	public string animationName;
	public string name;
	public string description;


	//Combat stuff
	public bool isMelee;
	public bool isAoe;

	public float cooldown;
	public float range;
	public float damage;
	public float damagePerLevel;
	public int aoe;
	public int width;

	public float Damage(){
		return Player.character.MainAttributeValue() * (damage+(damagePerLevel*Player.character.SkillLevel(this.id)));
	}
}