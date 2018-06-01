using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private static UIManager instance = null;

    public static UIManager get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(UIManager)) as UIManager;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }

    IEnumerator ui_timer;
    public UI_RushAttackCount attack_count;

    public void play_attack_timer(int _time)
    {
        ui_timer = second_timer(_time);
        StartCoroutine(ui_timer);
    }


    IEnumerator second_timer(int _time)
    {
        for (int i = 0; i <= _time; i++)
        {
            attack_count.renew_time(_time-i);
            yield return new WaitForSeconds(1.0f);
        }
        //1초에 한번씩 체크 (1초를 n번 반복) 하며 ui를 갱신시켜줌
        attack_count.renew_time(0);
    }
}
