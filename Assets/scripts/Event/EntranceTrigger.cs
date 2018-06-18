using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour {
    //보스룸 입장 이벤트 조작용
    
    public Boss_Worm boss;
    public Player player;
    public Transform boss_pos;
    Animator animator;
    Animation animation;
    bool ongoing;

    public AudioSource howling_sound;
    
    [Tooltip("몇번 흔들건지")]
    public int c_shake_cnt;
    [Tooltip("반복 속도 ")]
    public float c_shake_speed;
    public float c_shake_power;

    void Start () {
        animator = boss.gameObject.GetComponent<Animator>();
        animation = boss.gameObject.GetComponent<Animation>();
        ongoing = false;
	}
	
	void Update () {

        if (ongoing)
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
            {
                ongoing = false;
                boss.action_ready(Boss_Worm.Action.Rush_Attack);
                boss.edge_attack = false;
                EventManager.get_instance().off_event();
                Destroy(gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            boss.action_ready(Boss_Worm.Action.Ready);
            boss.transform.position = boss_pos.position;
            Quaternion quat = Quaternion.identity;
            //quat.SetLookRotation(Vector3.right);
            quat = Quaternion.Euler(new Vector3(0, 90, 0));
            boss.transform.rotation = quat;

            boss.edge_attack = true;
            animator.SetBool("howling", true);
            StartCoroutine(timer());
            EventManager.get_instance().event_setting(gameObject.GetComponent<EventPlot>());
        }
    }

    IEnumerator timer()
    {
        
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("howling", false);
        ongoing = true;
        howling_sound.Play();
        yield return new WaitForSeconds(0.5f);
        EventManager.get_instance().camera_shake(c_shake_power, c_shake_cnt, c_shake_speed, EventManager.Direction.Left_Right,0.0f);
        //사운드 재생
    }
}
