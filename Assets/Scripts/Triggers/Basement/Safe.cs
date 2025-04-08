using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{

    [SerializeField] private GameObject Basement;
    [SerializeField] private GameObject LongBasement;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Shelf;
    void Update()
    {
        if(GetComponent<SpringJoint>() != null)
        {
            ChangeBasement();
        }
    }

    void ChangeBasement()
    {
        //Disable longbasement
        LongBasement.SetActive(false);
        //enable basement
        Basement.SetActive(true);
        //Player pos + 47.5m
        CharacterController controller = Player.GetComponent<CharacterController>();
        controller.enabled = false;
        Player.transform.position = Player.transform.position + new Vector3(0f, 0f, 47.5f);
        controller.enabled = true;
        //Safe pos + 47.5m
        Shelf.transform.position = Shelf.transform.position + new Vector3(0f, 0f, 47.5f);
        gameObject.GetComponent<Safe>().enabled = false;
    }
}
