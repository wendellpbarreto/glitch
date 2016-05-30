using UnityEngine;
using System.Collections;

public class InventoryLoader : MonoBehaviour {

	void OnGUI(){
		int row = 0;
		int col = 0;
		foreach (CharacterItem characterItem in Player.character.inventory.bag){
			Item item = Game.GetItemById (characterItem.itemId);
			GUI.Button(new Rect(Screen.width/4+45+(col*65), 55+row*52, 60, 50), new GUIContent(item.name, item.ToString()));
			col++;
			if (col > 6) {
				col = 0;
				row++;
			}
		}
	}
}
