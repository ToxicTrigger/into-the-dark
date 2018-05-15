using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detecter : MonoBehaviour {
    public bool is_find;
    public GameObject target;

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("Heejin") || other.tag.Equals("Totem"))
        {
            if (gameObject.tag.Equals("Cam pos"))
            {
                if (other.tag.Equals("Totem"))
                {
                    return;
                }
            }
            is_find = true;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Heejin") || other.tag.Equals("Totem"))
        {
            is_find = false;
            target = null;
        }
    }
}
