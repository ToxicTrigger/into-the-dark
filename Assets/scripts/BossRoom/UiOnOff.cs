using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOnOff : MonoBehaviour {

    public UiAlphaCtrl ui;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Player"))
        {
            ui.onoff_ui(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            ui.onoff_ui(false);
        }
    }
}
