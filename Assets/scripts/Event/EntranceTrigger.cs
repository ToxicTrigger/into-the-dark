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

	void Start () {
        animator = boss.gameObject.GetComponent<Animator>();
        animation = boss.gameObject.GetComponent<Animation>();
        ongoing = false;
	}
	
	void Update () {

        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (ongoing)
        {
            Debug.Log("ongoing");
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
            {
                Debug.Log("러쉬어택연계");
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
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("howling", false);
        ongoing = true;
    }
}
