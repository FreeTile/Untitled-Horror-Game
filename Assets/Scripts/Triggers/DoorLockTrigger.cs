using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoorLockTrigger : MonoBehaviour
{
    public GameObject FrontDoor;
    public Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door = FrontDoor.GetComponent<Door>();
            door.ProcessMove();
            FrontDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            door.isLocked = true;
        }
        Destroy(gameObject);
    }
}
