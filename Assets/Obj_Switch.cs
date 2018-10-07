using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Switch : MonoBehaviour {

    [SerializeField]
    Rigidbody [] boxRigs;


    void Awake()
    {
        GameObject[] boxies = GameObject.FindGameObjectsWithTag("Boxies");

        boxRigs = new Rigidbody[ boxies.Length];
        for(int i =0; i<boxies.Length; ++i)
        {
            boxRigs[i] = boxies[i].GetComponent<Rigidbody>();
        }
    }

    void OnTriggerStay(Collider other)
    {

        if(other.tag == "Player")
        {
            for(int i =0; i< boxRigs.Length; ++i)
            {
                boxRigs[i].AddForce(Vector3.forward * 10f, ForceMode.Impulse);
            }
        }

    }

}
