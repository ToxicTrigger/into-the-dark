using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwitch : Observable {
    //모든 스위치가 공통적으로 가지는 항목 
    // 스위치의 on off 여부와 모든 옵저버에게 신호전달

    public AudioSource on_sound;
    public bool switch_on = false; //활성화 되어있는지 여부
    protected bool use_enable;

    AudioSource hit_sound;

    public void notify_all()
    {
        for (int i = 0; i < this.observers.Count; i++)
        {
            //스위치(퍼즐)은 현재 횃불의 활성화 가능 여부에 관여하므로 신호는 항상 횃불에 보낼것임
            Observer torch = this.observers[i] as Observer;
            torch.notify(this);
        }
    }

    //스위치가 꺼지거나 켜질 때 마다 신호를 보내야함...
    public virtual void set_switch(bool _onoff)
    {
        switch_on = _onoff;
        if (_onoff == true)
        {
            //hit_sound = on_sound;
            //hit_sound.Play();
            on_sound.Play();
            //gameObject.GetComponent<AudioSource>().Play();
        }
        notify_all();
    }

    public void set_use_enable(bool _set_boll)
    {
        use_enable = _set_boll;
    }

    public bool get_switch()
    {
        return switch_on;
    }

    public bool get_use_enable()
    {
        return use_enable;
    }

    public virtual void off_switch_set()
    {
        switch_on = false;
    }
}
