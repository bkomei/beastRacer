using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public BoxCollider2D ofChild;
    public bool isRotated = false;

    // Start is called before the first frame update
    void Start()
    {
        ofChild = GetComponentInChildren<BoxCollider2D>();
        ofChild.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //BOX COLLIDER ENABLE -------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ofChild.enabled = !ofChild.enabled;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ofChild.enabled = false;
        }


        //MOVEMENT ------------------------------------------------------

        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.Rotate(0, 0, 90);
            isRotated = !isRotated;

        }*/

        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x <= 6)
        {
            transform.Translate(1.41f, 0, 0, Space.World);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x >= 2)
        {
            transform.Translate(-1.41f, 0, 0, Space.World);

        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y <= 8)
        {
            transform.Translate(0, 1.35f, 0, Space.World);

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y >= 2)
        {
            transform.Translate(0, -1.35f, 0, Space.World);

        }
    }
}
