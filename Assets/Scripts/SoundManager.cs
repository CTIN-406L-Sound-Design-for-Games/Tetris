using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    

    public static void PlayRight()
    {
        AkSoundEngine.PostEvent("shape_right", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayLeft()
    {
        AkSoundEngine.PostEvent("shape_left", GameObject.Find("WwiseGlobal"));
    }

    public static  void PlayFlipUp()
    {
        AkSoundEngine.PostEvent("shape_flip", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayFlipDown()
    {
        AkSoundEngine.PostEvent("shape_flip", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayDrop()
    {
        AkSoundEngine.PostEvent("shape_land", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayLineClear()
    {
        AkSoundEngine.PostEvent("clear_row", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayStage1()
    {
        AkSoundEngine.PostEvent("stage_1", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayStage2()
    {
        AkSoundEngine.PostEvent("stage_2", GameObject.Find("WwiseGlobal"));
    }

    public static void PlayStage3()
    {
        AkSoundEngine.PostEvent("stage_3", GameObject.Find("WwiseGlobal"));
    }
}
