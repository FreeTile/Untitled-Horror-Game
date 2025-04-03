using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryUI : MonoBehaviour
{
    //Item Inventory information
    public PlayerInventory InventoryInfo;
    public Button ItemNodePrefab;
    public List<ItemSO> Items;
    public Dictionary<ItemSO.Items, Itemnodescript> Nodes=  new Dictionary<ItemSO.Items, Itemnodescript>();
    public int _PillsCount;
    public int _BatteryCount;
    public bool _HasKey1;
    public bool _HasKey2;


    [Space] // UI
    public GameObject InventoryContainer;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public Image ItemIcon;

    public void Add(ItemSO item)
    {
        Items.Add(item);
        Initialized(item);
    }

    public void Remove(ItemSO item)
    {
        Items.Remove(item);
    }
    public void NodeInstantiate(ItemSO item, int amount)
    {
        var itemNode = Instantiate(ItemNodePrefab, InventoryContainer.transform.position, InventoryContainer.transform.rotation, InventoryContainer.transform);
        itemNode.GetComponent<Itemnodescript>().Init(item);
        
        if(item.consumable == true)
        {
            itemNode.GetComponent<Itemnodescript>().GetItemCount(amount);
        }

        Nodes.Add(item.Type, itemNode.GetComponent<Itemnodescript>());
    }


    //Add note and add items to the Ui
    private void Initialized(ItemSO item)
    {
        switch (item.Type)
        {
            case ItemSO.Items.Pills:
                if(Nodes.ContainsKey(item.Type) && item.Type == ItemSO.Items.Pills)
                {
                    _PillsCount++;
                    foreach (var node in Nodes)
                    {
                        if(node.Key == ItemSO.Items.Pills)  
                        {
                            node.Value.GetItemCount(_PillsCount);  
                        }
                    }
                }
                else 
                {
                    _PillsCount++;
                    NodeInstantiate(item, _PillsCount);
                }
                break;
            case ItemSO.Items.batteris:
                if(Nodes.ContainsKey(item.Type) && item.Type == ItemSO.Items.batteris)
                {
                    _BatteryCount++;
                    foreach (var node in Nodes)
                    {
                        if (node.Key == ItemSO.Items.batteris)
                        {
                            node.Value.GetItemCount(_BatteryCount); 
                        }
                    }
                }
                else
                {
                    _BatteryCount++;
                    NodeInstantiate(item, _BatteryCount);
                }
                break;
            case ItemSO.Items.Chapter1Keys:
                _HasKey1= true;

                var keyNode1 = Instantiate(ItemNodePrefab, InventoryContainer.transform.position, InventoryContainer.transform.rotation, InventoryContainer.transform);
                keyNode1.GetComponent<Itemnodescript>().Init(item);
                break;
            case ItemSO.Items.Chapter2Keys:
                _HasKey2= true;

                var keyNode2 = Instantiate(ItemNodePrefab, InventoryContainer.transform.position, InventoryContainer.transform.rotation, InventoryContainer.transform);
                keyNode2.GetComponent<Itemnodescript>().Init(item);
                break;
            default:
                break;
        }

    }

    //When player consume item removes items from list and updates the count 
    public void ConsumeItem(ItemSO item)
    {
        if (item.consumable == true)
        {
            switch (item.Type)
            {
                case ItemSO.Items.Pills:
                    Remove(item);
                    _PillsCount--;
                    // Add sanity ++
                    foreach (var node in Nodes)
                    {
                        if (node.Key == ItemSO.Items.Pills)
                        {
                            node.Value.GetItemCount(_PillsCount);
                        }
                    }
                    break;
                case ItemSO.Items.batteris:
                    Remove(item);
                    _BatteryCount--;
                    FlashLight.IncreaseCharge(10);

                    foreach (var node in Nodes)
                    {
                        if (node.Key == ItemSO.Items.batteris)
                        {
                            node.Value.GetItemCount(_BatteryCount);
                        }
                    }
                    break;
                default:
                    break;

            }
        }
    }

    void Start()
    {
        _BatteryCount = 0;
        _PillsCount = 0;    
        foreach (var item in Items)
        {
            Initialized(item);
        }
    }
}
