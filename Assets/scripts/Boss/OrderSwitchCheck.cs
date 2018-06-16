using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSwitchCheck : MonoBehaviour {
    //

    [Tooltip("스위치의 순서대로 위에서부터 넣어주세요.")]
    public OrderSwitch[] order_switch_list;

    [Tooltip("현재 몇번째인지")]
    public int count;
    public bool success;

    public void Start()
    {
        count = 1;
        success = false;
        for(int i =0; i<order_switch_list.Length; i++)
        {
            order_switch_list[i].order_number = i + 1;  //이곳에 넣은 순서대로 스위치에 순서를 세팅한다.
        }
    }

    public void check_number(int _num)
    {
        if (count == 0) count = 1;
        
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
        {
            order_switch_list[_num-1].set_switch(true);
            if (count < order_switch_list.Length) count++;
            else count = 1;
        }
    }
}
