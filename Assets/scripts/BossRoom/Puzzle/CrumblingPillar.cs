using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPillar : MonoBehaviour {
    //무너지는 기둥


    //기둥의 각 층을 나타냄
    [System.Serializable]
    public class FloorData
    {
        [Tooltip("해당 층의 gameobject")]
        public PillarFloor pillar_model;
        public PillarFloor pillar_clone;
        [Tooltip("해당 층이 떨어질 위치")]
        public Transform crumbling_position;
        public float speed;
        public float rot_speed;
        [Tooltip("균열이 일어날 것인가? || 체크 = true")]
        public bool crack;
    }

    public FloorData[] floor_list;
    public bool is_crumbling;

    private void Start()
    {
        init_floor();
    }

    public void crumbling_all()
    {
        for(int i =0; i< floor_list.Length; i++)
        {
            floor_list[i].pillar_clone.set_state(floor_list[i].crumbling_position.position, floor_list[i].crack, floor_list[i].speed, floor_list[i].rot_speed);
        }
        is_crumbling = true;
    }
    
    public void init_floor()
    {
        if (is_crumbling)
        {
            for (int i = 0; i < floor_list.Length; i++)
            {
                PillarFloor _pillar = Instantiate(floor_list[i].pillar_model, floor_list[i].pillar_model.transform.position, floor_list[i].pillar_model.transform.rotation, this.transform);
                _pillar.gameObject.SetActive(true);                
                if (floor_list[i].pillar_clone != null)
                    Destroy(floor_list[i].pillar_clone.gameObject);
                floor_list[i].pillar_clone = _pillar;
            }
            is_crumbling = false;
        }
    }

}
