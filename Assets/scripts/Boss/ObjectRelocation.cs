using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRelocation : MonoBehaviour {
    //보스룸 오브젝트를 재배치한다.

    public Vector3 room_range;  //방의 범위를 지정한다.

    [Header("room split")]  //방을 몇 분할 할 것인지
    public int width_box;
    public int height_box;

    public GameObject[] room_obj;   //방에 들어갈 오브젝트 리스트

    struct Box
    {
        public float diameter;  //지름
        public Vector3 center;  //중앙 위치
    }

    Box[] box_list;

	void Start () {
        box_list = new Box[width_box * height_box]; //가로 x 세로 칸만큼 박스 리스트를 생성
        float width = room_range.x / width_box;
        float height = room_range.z / height_box;
        float width_temp;
        float height_temp;

        for(int i =0; i <box_list.Length; i++)
        {
            
        }
	}
	
	void Update () {
		
	}
}
