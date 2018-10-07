using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.layer != LayerMask.NameToLayer("Player") && !other.gameObject.CompareTag("Sword"))
        {
            other.GetComponent<Damageable>().Damaged(1 , 0.1f);
        }
    }
}
