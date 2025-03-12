using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    // Start is called before the first frame update
    public void AddToInventory(ItemSO item)
    {
        inventoryUI.Add(item);
    }
}
