/*
Ultimate MMORPG Kit - GameScripts/CraftItem - Client's script - skeletarik@gmail.com - 2013
This script is for profession. Add this script to the Main Camera. Set <quantity> to the quantity of the items that need for creating the necessary item.
For each element in <thingsForCraft> set the name of the item. Set <craftItem> to the name of the necessary item. Set <profession> to the profession
that you need to create the necessary item. You can read about custom setting more in the Documentation.
*/

using UnityEngine;
using System.Collections;

public class CraftItem : MonoBehaviour {

	public int quantity;					//How many items we need
	public string[] thingsForCraft;			//Which items we need
	public string craftItem;				//Which item we will craft
	public string profession;				//Which profession you need to craft that item
	
	public void ReturnCraftInfo(){
		string final = "";
		for(int i = 0;i<=quantity-1;i++)
		{
			final=final+thingsForCraft[i]+",";
		}
		final=final+"@"+craftItem+"@"+profession;
		GetComponent("Profession").SendMessage("GetCraftItems", final);
	}
	
}
