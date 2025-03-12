using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger1 : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Window knocks");
    }

}
