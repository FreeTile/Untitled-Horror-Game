using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirggerActivator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enableGOs;

    [SerializeField]
    private List<GameObject> disableGOs;

    [SerializeField]
    string logMessage = "";

    // Start is called before the first frame update
    private void OnTriggerEnter()
    {
        foreach (GameObject obj in enableGOs)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in disableGOs)
        {
            obj.SetActive(false);
        }

        Debug.Log( this.name + " triggered: " + logMessage);
    }
}
