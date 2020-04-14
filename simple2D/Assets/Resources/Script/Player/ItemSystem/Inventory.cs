using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public Item[] itemList = new Item[9];
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    //public Inventory 

    public bool AddItem(Item item)
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;
                return true;
            }
        }
        return false;
    }

}
