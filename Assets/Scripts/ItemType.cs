using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
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
    public ItemSO ItemInfo;
}
