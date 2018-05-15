using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyLink : MonoBehaviour {

    public GameObject[] body;

    Vector3 bone_position;

    //뒤의 몸들은 자신 앞몸이 지나간 자리와 그 자리에서의 회전값에 맞춰 따라하므로 필요한 정보
    struct BodyData
    {
        public Vector3 body_position;   
        public Quaternion body_rotation;
    }

    BodyData[] body_list;

	void Start () {
        body_list = new BodyData[body.Length - 1];  //몸 오브젝트의 수 만큼 몸 리스트를 생성한다.
        for(int i =0; i <body.Length; i++)  //오브젝트의 현재 정보를 리스트에 넣어준다.
        {
            body_list[i].body_position = body[i].transform.position;
            body_list[i].body_rotation = body[i].transform.rotation;
        }
    }
	
	void Update () {
        //움직이고 있다면,
        if (body[0].GetComponent<BossStemWorm>().action_state != BossStemWorm.Action.Stop)
        {

        }
    }

}
