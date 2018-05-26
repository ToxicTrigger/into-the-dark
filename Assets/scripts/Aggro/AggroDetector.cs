using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))] 
public class AggroDetector : MonoBehaviour {
    public SphereCollider sc;
    public bool flag;
    public AggroAI ao;


    void Start () {
        sc = GetComponent<SphereCollider>();
        ao = transform.parent.GetComponent<AggroAI>();
	}
}
