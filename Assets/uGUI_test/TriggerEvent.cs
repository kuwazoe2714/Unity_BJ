using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEvent : MonoBehaviour {

    private Toggle toggle1;

    public GameSystem ParentCanvas { get; set; }
    public int MyNumber { get; set; }

    private void Start()
    {
        toggle1 = GetComponent<Toggle>();
        toggle1.group = transform.parent.GetComponent<ToggleGroup>();
    }

    public void OnTriggerThis()
    {
        Debug.Log(toggle1.isOn);
        Debug.Log(MyNumber);

        ParentCanvas.ChangePlayerType(MyNumber);
    }

    
}
