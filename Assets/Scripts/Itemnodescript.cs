using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System.Linq.Expressions;

public class Itemnodescript : Selectable 
{
    [SerializeField] public TextMeshProUGUI countDisplay;
    [SerializeField] private FlashLight Charge;
    public ItemSO itemInfo;
    public int ItemAmount;


    // Start is called before the first frame update
    public void GetItemCount(int count)
    {
        countDisplay.text = count.ToString();
        ItemAmount = count;        
        Debug.Log(ItemAmount);
    }

    public void Init(ItemSO item)
    {
        itemInfo = item;
        GetComponent<Image>().sprite = itemInfo.icon;
    }

    public void OnSelect()
    {
        GetComponentInParent<InventoryUI>().ItemName.text = itemInfo.name;  
        GetComponentInParent<InventoryUI>().ItemDescription.SetText(itemInfo.description); 
        GetComponentInParent<InventoryUI>().ItemIcon.GetComponent<Image>().sprite = itemInfo.icon;  
    }

    public void Onclick ()
    { 
        if(itemInfo.consumable == true)
        {
            GetComponentInParent<InventoryUI>().ConsumeItem(itemInfo);
            if (ItemAmount <= 0){ Destroy(gameObject); }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(IsHighlighted()){ OnSelect(); }
    }
}
