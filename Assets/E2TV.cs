using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2TV : EventHandler
{
    public GameObject TV;

    public override IEnumerator Event()
    {
        TV.SetActive(true);
        Destroy(TV, 2f);
        return base.Event();
    }

}
