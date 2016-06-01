using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterLevel  {
	public static List<int> xpForLevel;
	public static int GetLevelByXp(int xp){
		for (int i = 0; i < xpForLevel.Count - 2; i++)
			if (xp < xpForLevel [i + 1])
				return i+1;
		return xpForLevel.Count+1;
	}
}
