using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public enum Type
    {
        Idle,
        Sword,
        Bow
    }

    public bool isUsing;
    public Type type;
    public Arrow arrow;
    public Transform fire_point;

    private void Update()
    {
        if(type != Type.Idle)
        {
            isUsing = true;

        }
        else
        {
            isUsing = false;
        }
    }

}
