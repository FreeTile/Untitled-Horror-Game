using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1S1 : MonoBehaviour
{
    Coroutine coroutine;
    void OnBecameVisible()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Event());
        }
    }

    IEnumerator Event()
    {
        Debug.Log("Screamer");
        yield return new WaitForSeconds(0.25f);
        this.gameObject.SetActive(false);
        //break the bulb in the room
        this.transform.parent.gameObject.SetActive(false);
    }
}
