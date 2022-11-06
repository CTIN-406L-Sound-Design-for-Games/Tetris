using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I : Tetrimino, ITetrimino
{
    public float speed = 1.0f;
    public float coolOffTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    TetriminoType ITetrimino.GetType()
    {
        return Type;
    }

    
}

