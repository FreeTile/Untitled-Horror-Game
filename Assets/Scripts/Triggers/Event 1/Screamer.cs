using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour
{
    void OnBecameVisible()
    {
        Debug.Log("Screamer");
        this.gameObject.SetActive(false);
    }
}
