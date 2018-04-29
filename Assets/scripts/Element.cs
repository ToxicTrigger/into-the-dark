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
        Ice,
        Lava
    }
    
    public Type type;
}
