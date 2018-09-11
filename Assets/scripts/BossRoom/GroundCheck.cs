using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
    //플레이어가 땅에 있는지에 대한 체크

    public float cnt;
    public bool is_ground;

	void Start () {
		
	}

    private void Update()
    {
        if(is_ground)
        {
            cnt += Time.deltaTime;
            //사운드 교체
            if(cnt >= 20)
            {
                //사운드 2교체
                //1초마다 초침소리 재생
            }
            else if(cnt >=30)
            {
                //사운드 정지
                //보스의 솟아오르기 공격 실행
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            is_ground = true;
            CancelInvoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //캐릭터가 땅을 벗어나고 0.5초가 지나면 카운트 리셋, 
            Invoke("clear_gyard", 0.5f);
        }
    }

    void clear_guard()
    {
        is_ground = false;
        cnt = 0;        
    }
}
