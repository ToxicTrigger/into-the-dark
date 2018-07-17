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
    //그대로 스위치가 들어갈지 모르나 일단!
    //hit 스위치에 대한것도 추가해야함
    [System.Serializable]
    public class Relocation_set
    {
        public Transform[] switch_position;
        public BasicSwitch[] switch_object;
        public Transform[] water_position;
        public GameObject[] water_object; 
        public Transform[] enemy_position;
        public CrumblingPillar[] c_pillar;
    }

    [Tooltip("맵에 스폰할 적 프리팹 넣기")]
    public GameObject enemy;

    [Tooltip("reloc_set 하나에 각 페이즈마다 쓰일 배치가 들어감")]
    public Relocation_set[] reloc_set;

    public BasicSwitch[] hit_switch;

    ArrayList water_list = new ArrayList();

    //재배치 시작!
    public void relocation(int phase)
    {
        int[] random_Array = new int[reloc_set[phase].switch_position.Length];

        int num = -1;

        for (int i = 0; i < random_Array.Length; i++)
        {
            random_Array[i] = num;
        }

        for (int i = 0; i < random_Array.Length; i++)
        {
            num = Random.Range(0, reloc_set[phase].switch_position.Length);

            for (int z = 0; z < random_Array.Length; z++)
            {
                if (random_Array[z] == num)
                {
                    --i;
                    break;
                }

                if (z == random_Array.Length - 1)
                {
                    random_Array[i] = num;
                }
            }

            if(random_Array[i] != -1)
                reloc_set[phase].switch_object[i].transform.position = reloc_set[phase].switch_position[random_Array[i]].transform.position;

        }

        for (int i = 0; i < reloc_set[phase].enemy_position.Length; i++)
        {
            GameObject _enemy = (GameObject)Instantiate(enemy, reloc_set[phase].enemy_position[i].position, Quaternion.identity);
        }

        for (int i = 0; i < water_list.Count; i++)
        {
            Destroy((GameObject)water_list[i]);
        }

        water_list.Clear();

        for (int i = 0; i < reloc_set[phase].water_position.Length; i++)
        {
            GameObject _water = (GameObject)Instantiate(reloc_set[phase].water_object[i],
                                                        reloc_set[phase].water_position[i].position, Quaternion.identity);

            water_list.Add(_water);
        }

        if (reloc_set[phase].c_pillar.Length > 0)
        {
            for (int i = 0; i < reloc_set[phase ].c_pillar.Length; i++)
                reloc_set[phase ].c_pillar[i].crumbling_all();
        }

    }

    public Relocation_set get_reloc(int phase)
    {
        return reloc_set[phase - 1];
    }

}
