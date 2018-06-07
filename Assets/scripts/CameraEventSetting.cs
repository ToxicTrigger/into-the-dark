using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEventSetting : MonoBehaviour {
    //이벤트를 세팅한다. 현재 기본적으로 정해진 구간을 이동한다.

    [Tooltip("해당 이벤트에서 이동할 포인트를 설정한다.")]
    public Transform[] target;

	void Start () {
		
	}	
	
	void Update () {
		
	}

    public Transform[] get_target_list()
    {
        return target;
    }
}
