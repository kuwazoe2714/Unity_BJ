using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : CustomYieldInstruction
{
    private static Wait waitInstance;
    // TODO セッター作ってあるんだからゲッターだけで良くない？
    public static bool IsWait { get; set; }

    public static Wait Create()
    {
        waitInstance = new Wait();
        return waitInstance;
    }

    public override bool keepWaiting
    {
        get
        {
            return IsWait;
        }
    }

    public static void WaitGame()
    {
        IsWait = true;
    }

    public static void WaitRelease()
    {
        IsWait = false;
        waitInstance = null;
    }
}
