using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCheck : Observable {

    bool is_on;

    private void Start()
    {
        is_on = true;
    }

    private void OnApplicationQuit()
    {
        is_on = false;
    }

    public void notify_all()
    {
        //Debug.Log("파괴! = " + this.transform.parent.name);
        for (int i = 0; i < this.observers.Count; i++)
        {
            if (this.observers[i] == null)
                return;
            //스위치(퍼즐)은 현재 횃불의 활성화 가능 여부에 관여하므로 신호는 항상 횃불에 보낼것임
            Observer obj = this.observers[i] as Observer;
            obj.notify(this);
        }
    }

    private void OnDestroy()
    {
        if(is_on)
            notify_all();
    }
}
