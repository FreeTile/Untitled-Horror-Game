using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle2Piece", menuName = "ScriptableObjects/Puzzle2Piece", order = 1)]
public class PuzzlePiece : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;
}
