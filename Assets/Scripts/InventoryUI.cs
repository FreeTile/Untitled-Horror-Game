using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;

public class InventoryUI : MonoBehaviour
{
    //Item Inventory information
    public PlayerInventory InventoryInfo;
    public GameObject ItemNodePrefab;
    public List<ItemSO> Items;
    public int _PillsCount;
    public int _BatteryCount;
    public int _HasKey1;
    public int _HasKey2;

    [Space] // UI
    public GameObject Inventory;
    public TMP_Text ItemName;
    public TMP_Text ItemDescription;
    public Sprite ItemIcon;

    public void Add(ItemSO item)
    {
        Items.Add(item);
    }

    public void Remove(ItemSO item)
    {
        Items.Remove(item);
    }

    public void AddInventoryNodes()
    {
        // todo 0 : move this to it's own function and call here ( and in the on click later)
        foreach (var item in Items)
        {
            //TODo 1
            // instanriate new prefab instancwe
            // get the compooinenbt for nww itemnode script
            // call init on that script and pass in as param this which will give it a reference back to the inventory screen to update selected item.
            //  init 2ill also take in it's own SO data

            // todo 3 
            // in the neqw ItemNode Script it qill have:
            // publlic init ( gamwobjwect parwent, itemSO dta)
            // here you will use the sodata to set the image of the button and 
            // SPRITE = SODATA.ICON
            
            //todo 4
            // ON CLICK()
             // inventorymNgwe.clickedbtn(sodataa)
        }
    }

    //todo 2
    // in this invewntory script crEATE A SEET SELECTED FUNCTION( INT ID)
    // this will set an id of selected item, and also populate the data on the right with info from thw SOdaata.
       // only show use btn if consumable

    // todo 5
    // we need an onclick for use.
       // remove one from inventory count, update displayed list 

    // Start is called before the first frame update
    void Start()
    {
        AddInventoryNodes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
