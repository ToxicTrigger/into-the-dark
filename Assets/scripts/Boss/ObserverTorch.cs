using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverTorch : Observer {

    public ObservableTorch observerble_torch;
    public int switch_on_cnt=0;
    [Tooltip("클리어에 필요한 스위치 개수 (일반적으로 해당 횃불에 신호를 보내는 스위치의 개수로 구성)")]
    public int switch_num_for_clear;

    public override void notify(Observable observable)
    {
        BasicSwitch torch = observable as BasicSwitch;
        //Debug.Log("torch.name = \"" + torch.name + " || torch.get_switch() = \"" + torch.get_switch());
        if (torch.get_switch())
        {
            switch_on_cnt++;

            if(switch_on_cnt == switch_num_for_clear)
            {
                observerble_torch.on_light();
            }
        }
        else
        {
            if (0 < switch_on_cnt)
            {
                switch_on_cnt--;
                if (observerble_torch.torch_state == ObservableTorch.State.On)
                {
                    observerble_torch.off_light();
                }
            }

        }

    }

}
