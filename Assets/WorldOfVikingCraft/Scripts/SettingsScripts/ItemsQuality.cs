/*
Ultimate MMORPG Kit - SettingsScripts/ItemsQuality - Client's script - skeletarik@gmail.com - 2013
This script is simply for comfortable setting of the different colours. 
Drag-and-drop this script to the "Code" gameObject and set any colours you want. 
Then copy RGB values and divide them by 255. (255=1. R/255=x; G/255=y; B/255=z. Then set your own Color of items like that:	Color c = new Color(x,y,z)).
*/

using UnityEngine;
using System.Collections;

public class ItemsQuality : MonoBehaviour {
	public Color bad;
	public Color normal;
	public Color rare;
}
