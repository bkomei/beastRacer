using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Pieces")]
public class pieceTypeManager : ScriptableObject
{
    public SetupGrid setupgrid;
    public GameObject visuals;
    public enum PieceType
    {
        Blue,
        Red,
        Green,
        Yellow

    }
    public PieceType type;

    





}
