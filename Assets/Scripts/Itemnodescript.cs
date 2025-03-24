using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class Itemnodescript : Selectable 
{
    [SerializeField] public TextMeshProUGUI countDisplay;
    private ItemSO itemInfo;
    private int count;

    // Start is called before the first frame update

    public void Init(ItemSO item, int amount)
    {
        itemInfo = item;
        count = amount; 
        GetComponent<Image>().sprite = itemInfo.icon;
        countDisplay.text = count.ToString();
        Debug.Log(amount);
    }

    public void OnSelect()
    {
        GetComponentInParent<InventoryUI>().ItemName.text = itemInfo.name;
        GetComponentInParent<InventoryUI>().ItemDescription.SetText(itemInfo.description);
        GetComponentInParent<InventoryUI>().ItemIcon.GetComponent<Image>().sprite = itemInfo.icon;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsHighlighted()){ OnSelect(); }
    }
}
