using UnityEngine;
using System.Collections;

public enum EnemyQuality {
	Normal,
	Elite,
	Rare
}

public class Enemy : BaseAttributes {
	public string id;
	public string name;
	public string prefabName;
	public EnemyQuality quality;

	public Enemy(){
		
	}

	public Enemy(string id, string name, string prefabName, float s, float d, float i, float v){
		this.id = id;
		this.name = name;
		this.prefabName = prefabName;
		this.strenght = s;
		this.dextrery = d;
		this.vitality = i;
		this.inteligence = v;
	}

	public float GetDamage(){
		return 1;
	}
}
