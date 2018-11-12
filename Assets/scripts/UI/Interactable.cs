using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("대상과 상호 작용 가능한 거리")]
    public float talk_distance = 1.0f;

    [Tooltip("상호작용할 대상")]
    public Transform target;

    [Tooltip("대상과 상호작용 상태 인가?")]
    public bool hasTalking;

    [Tooltip("상호작용 상태 일 때 활성화 되어야 하는 것들")]
    public List<GameObject> events;
    public EButton e;
    // 대상이 거리안에 있나?
    public bool get_capture_area;

    protected void Awake()
    {
        target = GameObject.FindObjectOfType<Player>().transform;
        e = GameObject.FindGameObjectWithTag("E").GetComponent<EButton>();
    }

    bool up_push_button, down_push_button;

    void Talk()
    {
        if( get_capture_area )
        {
            if( e != null )
            {
                if(!up_push_button)
                {
                    e.Up();
                    up_push_button = true;
                }
                
                if( Input.GetButtonDown("Submit") )
                {
                    if(!down_push_button)
                    {
                        e.Down();
                        down_push_button = true;
                    }
                    
                    if( !hasTalking )
                    {
                        hasTalking = true;
                    }
                    else
                    {
                        hasTalking = false;
                    }
                }
            }
        }
        else
        {
            up_push_button = false;
            down_push_button = false;
            hasTalking = false;
            if( e != null ) e.Down();
        }
    }
    

    void update_()
    {
        
        float dis = Vector3.Distance(target.position , transform.position);
        if( dis <= talk_distance )
        {
            get_capture_area = true;
            Talk();
        }
        else if(dis > talk_distance && dis < 10)
        {
            up_push_button = false;
            down_push_button = false;
            get_capture_area = false;
            hasTalking = false;
            e.Down();
        }
    }

    void update_event()
    {
        if( hasTalking )
        {
            IEnumerator iter = events.GetEnumerator();
            while( iter.MoveNext() )
            {
                GameObject tmp = iter.Current as GameObject;
                tmp.SetActive(true);
            }
        }
        else
        {
            IEnumerator iter = events.GetEnumerator();
            while( iter.MoveNext() )
            {
                GameObject tmp = iter.Current as GameObject;
                tmp.SetActive(false);
            }
        }
    }
    
    public void Update()
    {
        update_();
        update_event();
    }
}
