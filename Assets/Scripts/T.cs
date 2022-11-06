using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T : Tetrimino, ITetrimino
{
    // Start is called before the first frame update
    void Start()
    {

    }

    TetriminoType ITetrimino.GetType()
    {
        return Type;
    }


}

