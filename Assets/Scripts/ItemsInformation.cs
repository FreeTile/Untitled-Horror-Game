using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemInformation", menuName = "ScriptableObject/ItemInformation")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public bool consumable;

    public enum Items
    {
        Pills,
        batteris,
        Chapter1Keys,
        Chapter2Keys,
        Notes1,
        Notes2,

        NUM_ITEMS
    }

    public Items Type;
}

// class to keep track of inventory data

// dictionRY of itewms, INT AMOUNT
 // removwe / ADD / FUNCTIONS

// screen class
 // RESOURCES.LOAD("KEYDATA")

