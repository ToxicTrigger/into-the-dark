using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tail : MonoBehaviour
{
    public Transform child;

    public Vector3 boneAxis = new Vector3(0.0f, -1.0f, 0.0f);

    public float dragForce = 0.4f;
    public float stiffnessForce = 0.01f;

    float springLength;

    public Vector3 springForce = new Vector3(0.0f, -0.0001f, 0.0f);
    Vector3 currTipPos;
    Vector3 prevTipPos;

    Quaternion localRotation;

    Transform originTr;
    Transform tr;

    public Boss_Worm boss;
    public Boss_State state;

    void Awake()
    {
        //if(transform.parent.GetComponent<Boss_Worm>()) _parent = transform.parent.GetComponent<Boss_Worm>();
        tr = transform;
        localRotation = transform.localRotation;
    }

    private void Start()
    {        
        if (child != null)
        {
            springLength = Vector3.Distance(tr.position, child.position);
            currTipPos = child.position;
            prevTipPos = child.position;
        }
        //state = _parent.GetComponent<Boss_State>();
        //state = head_seach(this.transform)._parent.GetComponent<Boss_State>();
    }

    //꼬리 충돌체크용
    public void OnTriggerEnter(Collider other)
    {
        if (state.get_state() == Boss_State.State.Groggy)
        {
            if (other.CompareTag("Arrow"))
            {
                Debug.Log("활에 맞았어!");
                boss.add_damage();
                Destroy(other.gameObject);
            }
            if (other.CompareTag("Sword"))
            {
                Debug.Log("칼에 맞았어!");
                boss.add_damage();
            }
        }
    }

    Boss_Tail head_seach(Transform tail)
    {
        if (tail.parent.GetComponent<Boss_Tail>() == null)
        {
            return tail.GetComponent<Boss_Tail>();
        }
        else
        {
            return head_seach(tail.parent);
        }
    }

    //꼬리의 움직임을 업데이트함
    public void move_update(Vector3 _move_dir)
    {

        tr.localRotation = Quaternion.identity * localRotation;

        float sqrDt = Time.deltaTime * Time.deltaTime;

        // 움직일 위치 계산
        Vector3 force = tr.rotation * (boneAxis * stiffnessForce) / sqrDt;

        force += (prevTipPos - currTipPos) * dragForce / sqrDt; // 역방향으로 끄는 힘
        //force += springForce / sqrDt; // 해당 축으로 더 빠르게 펴지기 위함


        // 자식의 이전 위치와 현재 위치 갱신
        Vector3 temp = currTipPos;

        currTipPos = (currTipPos - prevTipPos) + currTipPos + (force * sqrDt);
        currTipPos = ((currTipPos - tr.position).normalized * springLength) + tr.position;

        prevTipPos = temp;

        // 방향 계산
        // 기준 축에서 
        Vector3 originAxis = tr.TransformDirection(boneAxis);
        //방향(벡터)를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다는 뜻
        //TransformDirection 함수는 인자 direction(호출한 게임 객체의 로컬 좌표계 기준으로 정의된 것으로 보고)을 월드 좌표계 기준으로 표현된(변환된) 벡터를 반환한다.
        //뼈의 축(0,-1,0)의 월드좌표가 알고싶은듯
        Quaternion targetDir = Quaternion.FromToRotation(originAxis, currTipPos - tr.position);
        //특정 방향에서 다른 방향으로 회전한다. FromToRotation(Vector3 from, Vector3 to)

        tr.rotation = targetDir * tr.rotation;
    }

    IEnumerator rot_set_timer()
    {

        yield return new WaitForSeconds(0.5f);
    }

}
