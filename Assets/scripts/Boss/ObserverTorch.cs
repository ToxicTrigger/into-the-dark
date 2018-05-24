using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverTorch : Observer {

    public ObservableTorch observerble_torch;
    public int switch_on_cnt=0;
   
    public override void notify(Observable observable)
    {
        BasicSwitch torch = observable as BasicSwitch;

        if (torch.get_switch())
        {
            switch_on_cnt++;

            if(switch_on_cnt == 3)
            {
                observerble_torch.use_enabled = true;
            }
        }
        else
        {
            if(0 < switch_on_cnt)switch_on_cnt--;
            observerble_torch.use_enabled = false;
        }

    }

}
