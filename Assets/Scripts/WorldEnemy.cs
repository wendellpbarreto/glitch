using UnityEngine;
using System.Collections;

public class WorldEnemy
{
	public string worldId;
	public string enemyId;

	public WorldEnemy(){
	}

	public WorldEnemy(string worldId, string enemyId){
		this.worldId = worldId;
		this.enemyId = enemyId;
	}
}

