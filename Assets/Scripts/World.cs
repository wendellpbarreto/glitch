using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World
{
	public string id;
	public string name;
	public int xpReward;
	public int goldReward;
	public string itemTier;
	public List<WorldEnemy> worldEnemies;

	public World(){
	}

	public World(string id, string name, int xpReward, int goldReward, string itemTier){
		this.id = id;
		this.name = name;
		this.xpReward = xpReward;
		this.goldReward = goldReward;
		this.itemTier = itemTier;
		this.worldEnemies = new List<WorldEnemy> ();
	}

	public void GiveReward(){
		
	}
}

