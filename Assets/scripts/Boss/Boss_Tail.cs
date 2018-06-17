using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tail : MonoBehaviour
{
    //보스 꼬리 _ 애니메이션(?) 부분만 담당하는 스크립트임

    //public bool boss_head;
    //Quaternion parent_tail_rot; //부모 tail의 Rotation정보를 받아올 변수
    //Quaternion this_tail_rot_temp;    //보관용
    //Quaternion this_tail_rot;     //자신 tail의 Rotation정보를 저장할 변수 (목표값)

    //Transform parent_tr;
    //Vector3 target_pos;
    //float bone_distance;

    //Boss_Tail head_tail;

    //public float rot_speed_min;    //꼬리의 회전지연
    //public float rot_speed;
    //public float rot_delay;

    //public float speed;
    //public float delay;

    //public bool move_on;










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

    Boss_Worm _parent;

    void Awake()
    {
        if(transform.parent.GetComponent<Boss_Worm>()) _parent = transform.parent.GetComponent<Boss_Worm>();
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

        //parent_tail_rot = Quaternion.identity;
        //this_tail_rot_temp = this.transform.rotation;
        //head_tail = head_seach(this.transform);
        //bone_distance = Vector3.Distance(transform.parent.position, transform.position);
        //parent_tr = transform.parent.transform;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_parent.action_state == Boss_Worm.Action.Groggy)
        {
            if (other.CompareTag("Arrow"))
            {
                Debug.Log("dd");
                _parent.add_damage();
                Destroy(other.gameObject);
            }
            _parent.add_damage();
            Destroy(other.gameObject);
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



        //if (transform.parent.GetComponent<Boss_Tail>() == null) boss_head = true;
        //else
        //{
        //    boss_head = false;
        //    //parent_tail_rot = transform.parent.rotation; //부모의 회전 정보를 받아옴
        //}

        ////현재 내가 머리라면
        //if (boss_head)
        //{
        //    //움직임이 있을때만 회전한다.
        //    if (_move_dir != Vector3.zero)
        //    {
        //        this_tail_rot = Quaternion.identity;

        //        //Quaternion this_tail_rot_x = Quaternion.identity;
        //        //float y_angle = Mathf.Atan2(_move_dir.x, _move_dir.z) * Mathf.Rad2Deg;
        //        //float x_angle = 0;

        //        //if (_move_dir.y != 0)
        //        //{
        //        //    x_angle = Mathf.Atan2(-_move_dir.y, _move_dir.z) * Mathf.Rad2Deg;
        //        //}

        //        //this_tail_rot.eulerAngles = new Vector3(x_angle, y_angle, 0);

        //        this_tail_rot.SetLookRotation(_move_dir);   //ㅅㅂ

        //        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this_tail_rot, rot_speed * Time.deltaTime);
        //        move_on = true;
        //    }
        //    else move_on = false;
        //}
        //else //머리가 아니라면 앞 꼬리의 정보를 받아와 계산한다.
        //{
        //    //if (head_tail.move_on)
        //    //{
        //    //    this.transform.rotation = this_tail_rot_temp;
        //    //    speed = rot_speed;
        //    //    this_tail_rot = parent_tail_rot;
        //    //}
        //    //else
        //    //{
        //    //    speed = rot_speed;
        //    //}


        //    //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this_tail_rot, speed * Time.deltaTime);

        //    //this_tail_rot_temp = this.transform.rotation;
        //    //parent_tail_rot = transform.parent.rotation;

        //    //부모의 바로 뒤, 이동 방향의 반대에 위치한 곳으로 하지만 이동 방향을 바라보는 

        //    Vector3 target_dis = (parent_tr.TransformDirection(Vector3.forward) * -bone_distance) + parent_tr.position;

        //    Debug.Log("parent_pos = \"" + parent_tr.position + "\" || target_pos = \"" + target_pos + "\" || parent_pos - target_pos = \"" + (parent_tr.position - target_pos) + "\"");

        //    target_pos = target_dis;

        //    Debug.DrawLine(target_pos, parent_tr.position, Color.red);

        //    Vector3 originAxis = transform.TransformDirection(Vector3.back);
        //    Quaternion targetDir = Quaternion.identity;
        //    targetDir = Quaternion.FromToRotation(originAxis, transform.position - target_pos);
        //    //targetDir.SetLookRotation(parent_tr.TransformDirection(Vector3.forward));


        //    // Debug.Log("my_forward = \"" + transform.TransformDirection(transform.forward) + "\" target_forward = \"" + (target_pos - transform.position) + "\"" + " Angle = \"" + Quaternion.FromToRotation(originAxis, transform.TransformDirection(target_pos)) + "\"");
        //    //Debug.Log("my_forward = \"" + transform.TransformDirection(transform.forward) + "\" target_forward = \"" + transform.TransformDirection(target_pos - transform.position).normalized + "\"");

        //    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * targetDir, rot_speed * Time.deltaTime);

        //    this_tail_rot_temp = this.transform.rotation;

        //}
    }

    IEnumerator rot_set_timer()
    {

        yield return new WaitForSeconds(0.5f);
    }

}
