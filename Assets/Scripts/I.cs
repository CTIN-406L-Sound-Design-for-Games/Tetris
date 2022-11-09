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

    private bool rotated;
    protected override void RotateForward()
    {
        rotated = !rotated;
        pivot.transform.Rotate(Vector3.forward, rotated?90:-90);
        SoundManager.PlayFlipUp();
    }

    protected override void RotateBack()
    {
        rotated = !rotated;
        pivot.transform.Rotate(Vector3.forward, rotated?90:-90);
        SoundManager.PlayFlipDown();
    }
}

