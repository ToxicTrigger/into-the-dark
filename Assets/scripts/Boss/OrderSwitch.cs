using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSwitch : BasicSwitch {
    //순서대로 입력해야 하는 스위치

    public Light _light;

    public OrderSwitchCheck check;

    [Tooltip("해당 스위치의 순서를 입력")]
    public int order_number; 

	void Start () {
        _light.gameObject.SetActive(false);
    }
	

    private void OnTriggerEnter(Collider other)
    {
        //순서대로 입력해야하는 스위치는 빛의 공격에만 반응하므로 추후 변경해야함
        if(other.CompareTag("Arrow") && !get_switch())
        {
            //빛의 공격이 들어오면 무조건 성공시킨다.
            set_switch(true);
            check.check_number(order_number);
        }
    }

    public override void set_switch(bool _onoff)
    {
        Debug.Log(_onoff);
        switch_on = _onoff;
        notify_all();

        _light.gameObject.SetActive(_onoff);
    }


}
