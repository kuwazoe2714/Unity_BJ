using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_player : CustomYieldInstruction
{
    public static bool buttonOn = false;

    public override bool keepWaiting
    {
        get
        {
            if(buttonOn)
            {
                return false;
            }
            else
            {
                return (!buttonOn);
            }
        }
    }
}

public class test_player : MonoBehaviour
{
    public void OnButton()
    {
        // ボタンが押されていないなら
        if (test_wait.IsWait != true)
        {
            Test_player.buttonOn = true;
        }
    }


}
