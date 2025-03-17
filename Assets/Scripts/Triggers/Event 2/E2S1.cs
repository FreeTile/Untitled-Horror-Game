using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2S1 : EventHandler
{
    public override IEnumerator Event()
    {
        Debug.Log("Screamer");
        yield return new WaitForSeconds(0.25f);
        //break the bulb in the room
        this.transform.parent.gameObject.SetActive(false);
    }
}
