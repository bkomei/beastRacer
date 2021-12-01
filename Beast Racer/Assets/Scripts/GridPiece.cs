using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPiece : MonoBehaviour
{
    Rigidbody2D rb;
    public SetupGrid manager;
    public GameObject visuals;
    public enum PieceType
    {
        Blue,
        Red,
        Green,
        Yellow
    }

    public PieceType type;

    public SpriteRenderer spriteRenderer;
    public Sprite good;
    public Sprite neutral;
    public Sprite bad;

    /*public GridPiece leftPiece;
    public GridPiece rightPiece;
    public GridPiece upPiece;
    public GridPiece downPiece;*/


    public void SetupPiece(SetupGrid manager, PieceType newType)
    {
        // setup a reference to the setupgrid script in case we need data like the grid width
        this.manager = manager;
        type = newType;

        rb = GetComponent<Rigidbody2D>();

        //set some placeholder colors based on type
        if (type == PieceType.Blue)
        {
            spriteRenderer.sprite = good;
        }

        if (type == PieceType.Red)
        {
            spriteRenderer.sprite = neutral;
        }

        if (type == PieceType.Green)
        {
            spriteRenderer.sprite = neutral;
        }

        if (type == PieceType.Yellow)
        {
            spriteRenderer.sprite = bad;
        }

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        //using raycasts to check our neighbors to avoid Math
        /*RaycastHit hit;
        if(Physics.Raycast(origin:transform.position, direction:Vector3.up, out hit))
        {
            if (hit.collider.gameObject.GetComponent<GridPiece>())
            {
                upPiece = hit.collider.gameObject.GetComponent<GridPiece>();
            }
        }

        if (Physics.Raycast(origin: transform.position, direction: Vector3.down, out hit))
        {
            if (hit.collider.gameObject.GetComponent<GridPiece>())
            {
                downPiece = hit.collider.gameObject.GetComponent<GridPiece>();
            }
        }

        if (Physics.Raycast(origin: transform.position, direction: Vector3.left, out hit))
        {
            if (hit.collider.gameObject.GetComponent<GridPiece>())
            {
                leftPiece = hit.collider.gameObject.GetComponent<GridPiece>();
            }
        }

        if (Physics.Raycast(origin: transform.position, direction: Vector3.right, out hit))
        {
            if (hit.collider.gameObject.GetComponent<GridPiece>())
            {
                rightPiece = hit.collider.gameObject.GetComponent<GridPiece>();
            }
        }*/

        //freeze grid piece in place


    }


    /*private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "reap")
        {
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "controller")
        {
            gameObject.SetActive(false);


            if (type == PieceType.good)
            {
                ScoreText.goodCount += 1;
                print("GOOD");
                OverallScoreScript.instance.AddBigPoint();
            }

            if (type == PieceType.neutral)
            {
                ScoreText.neutralCount += 1;
                print("NEUTRAL");
                OverallScoreScript.instance.AddPoint();
            }

            if (type == PieceType.neutral2)
            {
                ScoreText.neutralCount += 1;
                print("NEUTRAL");
                OverallScoreScript.instance.AddPoint();
            }

            if (type == PieceType.bad)
            {
                ScoreText.badCount += 1;
                print("BAD");
                OverallScoreScript.instance.SubtractPoint();
            }

        }
    }
    */



}
