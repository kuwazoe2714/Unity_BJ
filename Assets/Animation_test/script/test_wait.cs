using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_wait : CustomYieldInstruction
{
    // TODO ManagerにあったwaitInstanceを移してみた(問題なければコメント消す)
    private static test_wait waitInstance;
    public static bool IsWait { get; set; }

    public static test_wait Create()
    {
        waitInstance = new test_wait();
        // ここででtrue設定していることに違和感
        IsWait = true;
        return waitInstance;
    }

    public override bool keepWaiting
    {
        get
        {
            return IsWait;
        }
    }

    public static void Wait()
    {
        IsWait = true;
    }

    public static void WaitRelease()
    {
        IsWait = false;
        waitInstance = null;
    }
}
