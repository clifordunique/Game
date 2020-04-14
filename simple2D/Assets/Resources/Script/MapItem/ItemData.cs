using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public bool isOpen = false;

    public ItemData(Treasure treasure)
    {
        isOpen = treasure.isOpen;
    }
}
