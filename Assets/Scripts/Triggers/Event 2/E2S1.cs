using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2S1 : MonoBehaviour
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
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }
}
