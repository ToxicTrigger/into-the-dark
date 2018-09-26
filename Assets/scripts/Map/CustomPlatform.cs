using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlatform : MonoBehaviour
{
    public float speed;
    public GameObject text;

    public void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            text.SetActive(true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            Vector3 pos = transform.position;
            pos.y += speed;
            transform.position = pos;
        }
    }
}
