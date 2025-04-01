using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    public void AddToInventory(ItemSO item)
    {
        inventoryUI.Add(item);
    }
}
