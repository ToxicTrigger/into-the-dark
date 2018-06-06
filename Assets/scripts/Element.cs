using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour {
    public enum Type
    {
        None,
        Void,
        Fire,
        Water,
        Light
    }

    public Type type;
}
