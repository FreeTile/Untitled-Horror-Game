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
}

// class to keep track of inventory data

// dictionRY of itewms, INT AMOUNT
 // removwe / ADD / FUNCTIONS

// screen class
 // RESOURCES.LOAD("KEYDATA")

