using FMOD;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2 : MonoBehaviour
{
    [SerializeField]
    private Door door;

    [SerializeField]
    private List<PuzzlePiece> pieces;
    [SerializeField]
    private List<GameObject> piecesObj;

    public EventReference DoorUnlocked;

    private void OnTriggerEnter(Collider other)
    {
        int index = piecesObj.IndexOf(other.gameObject);
        if (index != -1)
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.localPosition = pieces[index].position;
            other.transform.localRotation = pieces[index].rotation;

            piecesObj.RemoveAt(index);
            pieces.RemoveAt(index);

            if (piecesObj.Count == 0)
            {
                RuntimeManager.PlayOneShot(DoorUnlocked);
                door.isLocked = false;
            }
        }
    }
}
