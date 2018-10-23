using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOnOff : MonoBehaviour {

    public GameObject ui;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Player"))
        {
            ui.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            ui.SetActive(false);
        }
    }
}
