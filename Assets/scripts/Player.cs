using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    bool is_attack;
    public Animator ani;
    Vector3 click_pos;
    public Weapon weapon;
    public LineRenderer line;
    public ThirdPersonCharacter tpc;
    float origin_move_speed;

    public Element cur_attack_type;
    public Camera cam;
    public AudioSource bow_fullback, bow_release;
    public Queue<GameObject> totems;
    public int cur_totems, installable_totems;
    public GameObject totem;
    public bool has_targeting_totem;
    public Image Hp;
    public Text HpTex;
    public Text totem_cnt;
    public Damageable damageable;
    public BoxCollider Sword;
    public ThirdPersonUserControl input_con;

    public float bow_time;
    public string cur_ani;

    public void Start()
    {

        weapon = GetComponent<Weapon>();
        cur_attack_type = GetComponent<Element>();
        tpc = GetComponent<ThirdPersonCharacter>();
        origin_move_speed = tpc.m_MoveSpeedMultiplier;
        totems = new Queue<GameObject>(0);
        damageable = GetComponent<Damageable>();
        cam = Camera.main;
        //  UI-issue
        //  UI 를 다른 씬으로 분리 하여 작업하고 merge 시에 따로 관리 할 수 있도록 하는게 좋을 것 같아요.
        //  다른 커밋에서 가령 제가 작업한 UI 가 있거나 미혜씨가 작업하신 UI 가 있을 때 
        //  너무 번거롭게 해결 해야 하는 것 같아 보이기도 하구요.
        //  아니면 방어 코드를 작성하는 것도 방법일 것 같구요. 현재 커밋에선 if ( null ) 로 체크 하고 있습니당.
        if (totem_cnt != null)
            totem_cnt.text = "3 /" + (3 - cur_totems).ToString();
    }

    public void setSwordEnable(int val)
    {
        if (val == 0)
        {
            Debug.Log("Off");
            Sword.enabled = false;
        }
        else
        {
            Debug.Log("On");
            Sword.enabled = true;
        }
    }

    void gen_arrow()
    {
        RaycastHit hit;
        Vector3 mouse = Input.mousePosition;
        if (Physics.Raycast(cam.ScreenPointToRay(mouse), out hit, 10000))
        {

        }

        Vector3 nor = transform.forward.normalized;
        GameObject arrow = GameObject.Instantiate(weapon.arrow.gameObject, weapon.fire_point.position, Quaternion.LookRotation(click_pos), null);
        arrow.GetComponent<Arrow>().look = weapon.fire_point.forward;
        if (has_targeting_totem)
        {
            arrow.GetComponent<Arrow>().has_targeting_totem = true;
        }



        if (bow_time >= 3.2f & bow_time < 4f)
        {
            Debug.Log("빛화살!");
            arrow.GetComponent<Arrow>().type.type = cur_attack_type.type;
            //강공격 여부 ㅇㅇ 
        }
        Destroy(arrow, 5.0f);
        AggroManager.get_instance().gen_aggro(transform.position, 10 + bow_time, 3);
    }
    void swap_element_type()
    {
        cur_attack_type.type = Element.Type.Light;
        t = cur_attack_type.type;
    }
    void calc_click_pos(bool sword)
    {
        has_targeting_totem = false;
        RaycastHit hit, coll;
        Vector3 mouse = Input.mousePosition;
        if (Physics.Raycast(cam.ScreenPointToRay(mouse), out hit, 10000))
        {
            click_pos = hit.point;
            click_pos.y = transform.position.y + 0.1f;

            if (!sword)
                line.gameObject.SetActive(true);

            Vector3 pos = transform.position;
            pos.y += 0.5f;

            if (!sword)
                line.SetPosition(0, pos);
            if (Physics.Linecast(pos, click_pos, out coll))
            {
                Debug.Log(coll.collider.name + "" + coll.collider.tag);
                if (coll.collider.CompareTag("TotemAggro"))
                {
                    Debug.Log("name : " + coll.collider.gameObject.name);
                    click_pos = coll.collider.gameObject.transform.position;
                    has_targeting_totem = true;
                    line.startColor = Color.yellow;
                    line.endColor = Color.yellow;
                }
            }
            else
            {
                line.startColor = Color.white;
                line.endColor = Color.white;
            }
            Debug.DrawLine(pos, click_pos, Color.red);


            if (!sword)
                line.SetPosition(1, click_pos);
            transform.LookAt(click_pos);
        }
    }

    public Element.Type t;
    public void editElementTypeInAni(int input)
    {
        switch (input)
        {
            // None
            case 0: cur_attack_type.type = Element.Type.None; break;
            case 1: cur_attack_type.type = t; break;
        }

    }
    int totem_size;
    void gen_totem()
    {
        if (totems != null)
            cur_totems = totems.Count;
        installable_totems = 3 - cur_totems;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (totems.Count > 2)
            {
                Destroy(totems.Dequeue());
            }

            if (totem_size > 2)
            {
                totem_size = 0;
            }

            Totem[] tot = FindObjectsOfType<Totem>();
            for (int i = 0; i < tot.Length; i++)
            {

                if (Vector3.Distance(tot[i].gameObject.transform.position, transform.position) <= 1)
                {
                    Destroy(tot[i]);
                }
            }

            GameObject t = Instantiate(totem, transform.position, Quaternion.identity, null);
            totems.Enqueue(t);
            if (totem_cnt != null) totem_cnt.text = "3 /" + installable_totems.ToString();
            totem_size += 1;
            ParticleCollider.instance.ps.trigger.SetCollider(1 + totem_size, t.transform.GetChild(0));
        }
    }

    public void FixedUpdate()
    {
        Hp.fillAmount = (damageable.Hp / 100);
        HpTex.text = damageable.Hp.ToString();

        if (!is_attack)
        {
            ani.SetBool("Attack", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            swap_element_type();
        }
    }

    void step_ani()
    {
        switch (cur_ani)
        {
            case "Swing_0":
                transform.position += transform.forward.normalized * 0.02f;
                break;
            case "Swing_1":
                transform.position += (transform.forward.normalized * 0.03f);
                break;
            case "Jump":
                transform.position += (transform.forward.normalized * 0.05f);
                break;
        }

    }

    void Update()
    {
        Vector3 tmp = transform.position;
        tmp.y = 10f;
        gen_totem();
        cur_ani = ani.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        step_ani();

        if (Input.GetButton("Fire1"))
        {
            ani.SetBool("Attack", true);
            weapon.type = Weapon.Type.Sword;

            calc_click_pos(true);
        }
        else if (Input.GetButton("Fire2"))
        {
            weapon.type = Weapon.Type.Bow;
            bow_time += Time.deltaTime;
            if (!bow_fullback.isPlaying)
            {
                bow_fullback.Play();
            }
            tpc.m_MoveSpeedMultiplier = 0;
            calc_click_pos(false);
        }
        ani.SetFloat("Bow_Fire", bow_time);

        if (Input.GetMouseButtonUp(1))
        {
            if (weapon.isUsing)
            {
                if (bow_time >= 0.5f)
                {
                    calc_click_pos(false);
                    gen_arrow();

                    bow_time = 0f;
                    weapon.type = Weapon.Type.Idle;
                    line.gameObject.SetActive(false);
                    tpc.m_MoveSpeedMultiplier = origin_move_speed;
                    if (!bow_release.isPlaying)
                    {
                        bow_fullback.Stop();
                        bow_release.Play();
                    }
                }
                else
                {
                    bow_time = 0f;
                    weapon.type = Weapon.Type.Idle;
                    line.gameObject.SetActive(false);
                    tpc.m_MoveSpeedMultiplier = origin_move_speed;
                    if (!bow_release.isPlaying)
                    {
                        bow_fullback.Stop();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //TODO :: melee attack here
            weapon.type = Weapon.Type.Idle;
            is_attack = false;
        }

        //현재 재생중인 ani 상태가 Run || Player_Idle 일 때만 입력을 받아오게 했습니다.
        //진작에 이렇게 할껄. 읽으신 후에 주석은 제거해주세요~ 
        if (cur_ani.Equals("Run") || cur_ani.Equals("Player_Idle"))
        {
            input_con.enabled = true;
        }
        else
        {
            input_con.enabled = false;
        }
    }
}
