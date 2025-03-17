using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3T1 : EventHandler
{
    public override IEnumerator Event()
    {
        yield return new WaitForSeconds(2);
        //Something happened in the kitchen
        yield return base.Event();
    }
}
