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
using UnityEngine.EventSystems;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    //Item Inventory information
    public PlayerInventory InventoryInfo;
    public Button ItemNodePrefab;
    public List<ItemSO> Items;
    private int _PillsCount;
    private int _BatteryCount;
    public int _HasKey1;
    public int _HasKey2;

    [Space] // UI
    public GameObject InventoryContainer;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public Image ItemIcon;

    public void Add(ItemSO item)
    {
        Items.Add(item);
        AddInventoryNodes();
    }

    public void Remove(ItemSO item)
    {
        Items.Remove(item);
    }

    public int GetNumConsumables(ItemSO.Items type )
    {
        int count = 0;
        foreach (var item in Items)
        {
            if (item.Type == type)
            {
                count++;
            }
        }
        return count;
    }


    private void Initialized()
    {
        foreach (var item in Items)
        {
            for (int i = 0; i < (int)ItemSO.Items.NUM_ITEMS; i++)
            {
                int amount = GetNumConsumables((ItemSO.Items)i);

                if(amount > 0)
                {
                    if(item.Type == (ItemSO.Items)i)
                    {
                        var itemNode = Instantiate(ItemNodePrefab, InventoryContainer.transform.position, InventoryContainer.transform.rotation, InventoryContainer.transform);
                        itemNode.GetComponent<Itemnodescript>().Init(item, amount);



                        //todo 4
                        // ON CLICK()
                        // inventorymNgwe.clickedbtn(sodata)

                        break;
                    }
                
                }

            } 
        }

    }


    public void AddInventoryNodes()
    {
        Initialized();
    }

    private void Onselect()
    {

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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
