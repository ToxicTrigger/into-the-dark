using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomRelocation : MonoBehaviour {

    private static BossRoomRelocation instance = null;

    public static BossRoomRelocation get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(BossRoomRelocation)) as BossRoomRelocation;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }

    /// ///////////////////////////////////////////////////
    /// 
    public enum Relocation_Turn
    {
        One=0,
        Two
    }

    public Relocation_Turn current_turn;

    [System.Serializable]
    public class position_setd
    {
        //public Transform[] time_position;
        //public Transform[] order_position;
        [Tooltip("Switch를 다 채우고 Order를 채울것")]
        public Transform[] switch_position;
        public Transform[] water_position;
        public GameObject[] water_object;   //만약 웅덩이가 제각기 모양이 다르다면 이 곳에서 넣어줍시다. 
        public Transform[] enemy_position;
    }
    //보스룸이 바뀔 때 요소들 (스위치, 물웅덩이, 잡몹)
    //물웅덩이는 크기가 제각각 다른가요?

    [Tooltip("reloc_set 1에 time과 order의 배치 1개가 들어감")]
    public position_setd[] reloc_set;


    void Start () {
        //current_turn = Relocation_Turn.One;
	}	

	void Update () {
		
	}    

    public void togle_set()
    {
        if(current_turn == Relocation_Turn.One)
        {
            current_turn = Relocation_Turn.Two;
        }
        else if(current_turn == Relocation_Turn.Two)
        {
            current_turn = Relocation_Turn.One;
        }
    }

}
