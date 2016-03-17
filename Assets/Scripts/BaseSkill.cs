using UnityEngine;
using System.Collections;

public abstract class BaseSkill {
	protected float cooldown;

	public abstract float GetDamage ();
	public abstract float GetRange ();
	public abstract string GetAnimationName ();
		
	public float GetCooldown(){
		return cooldown;
	}


}
