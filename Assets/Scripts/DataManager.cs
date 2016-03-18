using UnityEngine;
using System.Collections;

public static class DataManager {

	private static PlayerAttributes playerAttributes;

	public static void SetPlayerAttributes(PlayerAttributes attrs){
		DataManager.playerAttributes = attrs;
	}

	public static PlayerAttributes GetPlayerAttributes(){
		return DataManager.playerAttributes;
	}
}
