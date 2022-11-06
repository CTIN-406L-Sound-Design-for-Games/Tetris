using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J : Tetrimino, ITetrimino
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {

    }

    TetriminoType ITetrimino.GetType()
    {
        return Type;
    }



}

