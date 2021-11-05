using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L : Tetrimino, ITetrimino
{
    // Start is called before the first frame update
    void Start()
    {

    }

    TetriminoType ITetrimino.GetType()
    {
        return Type;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForUserInput();
        //UpdateGrid();

    }

    void CheckForUserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Root.transform.Translate(Vector3.left);

            if (!FindObjectOfType<GameMaster>().CheckIsValidPosition())
            {
                Root.transform.Translate(Vector3.right);
                return;
            }
            soundManager.PlayLeft();

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Root.transform.Translate(Vector3.right);
            if (!FindObjectOfType<GameMaster>().CheckIsValidPosition())
            {
                Root.transform.Translate(Vector3.left);
                return;
            }
            soundManager.PlayRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Pivot.transform.Rotate(Vector3.forward, 90);
            if (!FindObjectOfType<GameMaster>().CheckIsValidPosition())
            {
                Pivot.transform.Rotate(Vector3.forward, -90);
                return;
            }
            soundManager.PlayFlipUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            Pivot.transform.Rotate(Vector3.forward, -90);

            if (!FindObjectOfType<GameMaster>().CheckIsValidPosition())
            {
                Pivot.transform.Rotate(Vector3.forward, 90);
                return;
            }
            soundManager.PlayFlipDown();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Root.transform.Translate(Vector3.down);

            if (!FindObjectOfType<GameMaster>().CheckIsValidPosition())
            {
                Root.transform.Translate(Vector3.up);
                return;
            }

            soundManager.PlayDrop();
            //UpdateGrid();
        }
    }



}

