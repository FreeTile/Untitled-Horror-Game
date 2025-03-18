using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField]
    string debugLog;
    Coroutine coroutine;
    void OnBecameVisible()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Event());
            Debug.Log(debugLog);
        }
    }

    void OnTriggerEnter()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Event());
            Debug.Log(debugLog);
        }
    }

    public virtual IEnumerator Event()
    {
        
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
        yield return null;
    }

}
