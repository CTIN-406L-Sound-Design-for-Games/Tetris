using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O : Tetrimino, ITetrimino
{
    // Start is called before the first frame update
    void Start()
    {

    }

    TetriminoType ITetrimino.GetType()
    {
        return Type;
    }

    protected override void RotateForward()
    {
        return;
    }

    protected override void RotateBack()
    {
        return;
    }
}

