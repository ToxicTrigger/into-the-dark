using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTrans : MonoBehaviour
{
    public Transform des;
    public Vector3 offset;
    void Update()
    {
        Vector3 pos = Vector3.Lerp(transform.position, des.position + offset, 100);
        transform.position = pos;
    }
}
