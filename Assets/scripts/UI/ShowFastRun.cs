using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFastRun : MonoBehaviour
{
    public TotuUI ui;
    public int state;
    bool In;

    private void OnTriggerEnter(Collider collision)
    {
        if(!In)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                ui.state = state;
                In = true;
            }
        }

    }
}
