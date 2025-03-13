using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3T1 : MonoBehaviour
{
    Coroutine coroutine;
    void OnTriggerEnter()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Event());
        }
    }

    IEnumerator Event()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Event3");
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }
}
