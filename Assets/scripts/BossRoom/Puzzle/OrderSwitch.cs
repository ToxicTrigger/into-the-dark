using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSwitch : BasicSwitch {
    //순서대로 입력해야 하는 스위치

    public Light _light;

    public OrderSwitchCheck check;

    [Tooltip("해당 스위치의 순서를 보여줌 (순서의 입력은 Check오브젝트의 order_list설명 참조)")]
    public int order_number; 

	void Start () {
        _light.gameObject.SetActive(false);
        use_enable = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.transform.CompareTag("Sword") || collision.transform.CompareTag("Arrow")) && !get_switch() && use_enable)
        {
            check.check_number(order_number);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //순서대로 입력해야하는 스위치는 빛의 공격에만 반응하므로 추후 변경해야함
        if( (other.CompareTag("Sword") || other.CompareTag("Arrow") )&& !get_switch() && use_enable)
        {
            check.check_number(order_number);
        }
    }

    public override void set_switch(bool _onoff)
    {
        if((switch_on && _onoff == false) || _onoff)
            base.set_switch(_onoff);

        _light.gameObject.SetActive(_onoff);
    }

    public override void off_switch_set()
    {
        _light.gameObject.SetActive(false);
        switch_on = false;
    }

}
