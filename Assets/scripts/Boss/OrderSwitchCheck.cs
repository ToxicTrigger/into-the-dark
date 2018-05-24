using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSwitchCheck : MonoBehaviour {
    //

    public OrderSwitch[] order_switch_list;

    [Tooltip("현재 몇번째인지")]
    public int count;
    public bool success;

    public void Start()
    {
        count = 1;
        success = false;
    }

    public void check_number(int _num)
    {
        success = ( _num == count ? true : false );
        if (!success)
        {
            for (int i = 0; i < order_switch_list.Length; i++)
            {
                order_switch_list[i].set_switch(false);
            }
            count = 1;
        }
        else
            count++;
    }
}
