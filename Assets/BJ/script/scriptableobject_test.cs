using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class scriptableobject_test : ScriptableObject {

    [SerializeField]
    private string str;

    public string Str { get { return str; } }

}
