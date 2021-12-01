using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGrid : MonoBehaviour
{
    public GameObject gridPiecePrefab;
    /*public GameObject goodPiece;
    public GameObject neutralPiece;
    public GameObject badPiece;*/


    public int length = 6;
    public int width = 6;
    public float padding = 1.5f;
    public List<GridPiece> pieces;
    // Start is called before the first frame update

    public void Start()
    {
        Setup();
    }

    public void Setup()
    {
        // this creates a grid of width by length size, you can use the padding variable to increase the space between each piece
        // I got rid of any offsetting to avoid a lot of math, you can manually move the Grid parent object to position the grid how you want
        pieces = new List<GridPiece>();
        for (int i = length; i > 0; i--)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject pieceObject = Instantiate(gridPiecePrefab,
                    new Vector3(j * padding, i * padding, 0)
                    , Quaternion.identity);

                pieceObject.transform.SetParent(transform);
                pieceObject.name = "(" + i + "," + j + ")";
                GridPiece piece = pieceObject.GetComponent<GridPiece>();
                if (piece != null)
                {
                    pieces.Add(piece);
                }
            }
        }

        //now we run through each piece we instantiated and randomly assign a type and give them a reference to this script
        //the manager reference is in case the grid pieces need to know the size of the grid ever
        foreach (GridPiece piece in pieces)
        {
            if (piece != null)
            {
                piece.SetupPiece(this, (GridPiece.PieceType)Random.Range(0, 4));
            }
        }
    }
}
