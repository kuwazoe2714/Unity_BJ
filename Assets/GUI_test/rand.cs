using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rand : MonoBehaviour {

    Vector3 rValue = Vector3.zero;          // ランダムで箱が回転
    public Color[] cList;                   // 色のリストを作成

	// Use this for initialization
	void Start () {
        Invoke("ChangeState", 1f);
	}
	
	// Update is called once per frame
	void Update () {
        // ランダムで指定された角度を新しい角度に。
        transform.Rotate(rValue);
	}

    // 次の変更に遷移関数
    void ChangeState()
    {
        RotateChange();
        ColorChange();
        Invoke("ChangeState", Random.Range(1f, 2f));
    }

    // 角度変更関数
    void RotateChange()
    {
        // ランダムで角度変更
        float rX = Random.Range(-1f, 1f);
        float rY = Random.Range(-1f, 1f);
        float rZ = Random.Range(-1f, 1f);

        // 新しい角度を代入
        rValue = new Vector3(rX, rY, rZ);
    }

    //色変更関数
    void ColorChange()
    {
        // cListの中からランダムで色を選択
        int nColorIndex = Random.Range(0, cList.Length - 1);

        // ランダムで選択した色を代入
        GetComponent<Renderer>().material.color = cList[nColorIndex];

    }

    // GUIからの操作関数
    void OnGUI()
    {
        if (rValue == Vector3.zero)
        {
            // 更新を始めるGUIを配置する
            if (GUI.Button(new Rect(10, 10, 150, 30), "Start"))
            {
                ChangeState();
            }
        }
        else
        {
            // 更新を止めるGUIを配置する
            if (GUI.Button(new Rect(10, 10, 150, 30), "Stop"))
            {
                rValue = Vector3.zero;
                CancelInvoke();
            }
        }
    }
}
