using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbModel : MonoBehaviour {
    //방해 스위치의 모델스크립트, 실제적으로 방해하고 모습을 표시하는등의 행위는 이곳에서 진행

    List<BasicSwitch> model_list = new List<BasicSwitch>();

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Switch"))
        {
            BasicSwitch other_switch = other.GetComponent<BasicSwitch>();

            model_list.Add(other_switch); //방해하는 오브젝트에 추가
            other_switch.set_use_enable(false); //사용 불가능으로 체크한다.
        }
    }

    public void switch_destroy()
    {
        for(int i =0; i < model_list.Count ; i++)
        {
            model_list[i].set_use_enable(true);   //사용 가능으로 바꿔준다.
        }
        //방해하던 모든 스위치를 사용 가능하게 바꿔줬다면 나 자신을 꺼줌
        this.gameObject.SetActive(false);
        model_list.Clear();
    }
}
