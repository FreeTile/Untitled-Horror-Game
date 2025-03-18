using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6T1 : EventHandler
{
    public override IEnumerator Event()
    {
        yield return new WaitForSeconds(0.5f);
        //play steps sounds
        yield return new WaitForSeconds(2f);
        this.transform.parent.gameObject.SetActive(false);
    }
}
