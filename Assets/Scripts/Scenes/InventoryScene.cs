using UnityEngine;
using System.Collections;


public class InventoryScene : MonoBehaviour {
	public GUISkin skin;
	
	// Update is called once per frame
	void OnGUI () {
		GUI.skin = skin;

		GUI.Box (new Rect(0, 5, Screen.width/2-5, Screen.height -5), "Equiped");



		GUI.Box (new Rect(Screen.width/2 + 5, 5, Screen.width/2-5, Screen.height -5), "Bag");

		int row = 0;
		int col = 0;

		foreach (CharacterItem characterItem in Player.character.inventory.bag){
			Item item = Game.GetItemById (characterItem.itemId);
			GUI.Button(new Rect(Screen.width/2+5+(col*65), 55+row*52, 60, 50), new GUIContent(item.name, item.ToString()));
			col++;
			if (col > 5) {
				col = 0;
				row++;
			}
		}
		GUI.Label(new Rect(Screen.width/2+5, Screen.height/2, Screen.width/2-5, Screen.height/2), GUI.tooltip);
	}
}
