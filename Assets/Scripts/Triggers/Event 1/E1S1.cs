using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1S1 : EventHandler
{
    public override IEnumerator Event()
    {
        SanityManager.Instance.CheckFearLevel(10, 50);
        yield return new WaitForSeconds(0.5f);
        yield return base.Event();
    }
}
