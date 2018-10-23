using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFastRun : MonoBehaviour
{
    public TotuUI ui;
    public int state, match_state;

    private void OnTriggerEnter(Collider collision)
    {
        if( collision.gameObject.name.Equals("Player"))
        {
            ui.state = state;
            //if (ui.state == match_state)
            //{
              //  ui.state = state;
            //}
        }
    }
}
