using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstEvent : Observer {

    public Transform move_target;
    [Tooltip("유지시간")]
    public float []time_list;
    public GameObject boss;
    public Animator boss_ani;
    public int count=-1;
    ActionCamera ac;
    CharacterController p_ctrl;
    Animator p_ani;
    Player player;
    public bool is_shake = false;
    public BossRoomEvent []_event;

    [Space(16)]
    public int shake_tick;
    public float shake_power;
    public float tick_by_tick_time;

    [Space(16)]
    public int shake_tick2;
    public float shake_power2;
    public float tick_by_tick_time2;
    public BlackScreen black_screen;

    IEnumerator _timer;
    void Start () {
        player = FindObjectOfType<Player>();
        p_ani = player.GetComponent<Animator>();
        p_ctrl = FindObjectOfType<CharacterController>();
        ac = FindObjectOfType<ActionCamera>();
        black_screen = FindObjectOfType<BlackScreen>();
        boss_ani = this.GetComponent<Animator>();
        boss_ani.SetBool("is_end", true);
       
    }

    bool is_play;
	void Update () {
        switch (count)
        {
            case 0: //두리번, 캠2번 /흔들림 
                p_ctrl.enabled = false;
                player.transform.position = new Vector3(move_target.position.x,player.transform.position.y,move_target.position.z);
                p_ani.SetBool("around", true);

                ac.SetStateTarget(_event[0].event_scene[0].pin_num,_event[0].event_scene[0].cam_state, _event[0].event_scene[0].action_speed, _event[0].event_scene[0].stand_by_time, ac.Pins[_event[0].event_scene[0].pin_num].eulerAngles, _event[0].event_scene[0].event_name, Vector3.zero);
                if (_timer == null)
                {
                    _timer = timer();
                    StartCoroutine(_timer);
                }
                if(!is_play)
                {
                    is_play = true;
                    ac.Shake(shake_tick, shake_power, tick_by_tick_time);
                }

                break;
            case 1: //스턴, 캠3번, 보스 애니/흔들림
                if (is_shake)
                {
                    ac.Shake(shake_tick2, shake_power2, tick_by_tick_time2);
                    if (_timer == null)
                    {
                        _timer = timer();
                        StartCoroutine(_timer);
                    }
                }
                else
                {
                    ac.SetStateTarget(_event[1].event_scene[0].pin_num, _event[1].event_scene[0].cam_state, _event[1].event_scene[0].action_speed, _event[1].event_scene[0].stand_by_time, ac.Pins[_event[1].event_scene[0].pin_num].eulerAngles, _event[1].event_scene[0].event_name, Vector3.zero);
                    
                    boss_ani.SetBool("is_end", false);
                    p_ani.SetBool("around", false);
                    p_ani.SetBool("stun", true);
                }
                break;
            case 2: //애니 종료
                p_ani.SetBool("stun", false);
                boss_ani.SetBool("is_end", true);
                p_ctrl.enabled = true;
                Destroy(boss, 5.0f);
                Destroy(this, 5.0f);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Player"))
        {
            black_screen.add_observer(this);
            black_screen.change_screen(BlackScreen.ScreenState.Fade_Out);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void notify(Observable observable)
    {
        if (observable.gameObject.GetComponent<BlackScreen>())
        {
            BlackScreen bs = observable as BlackScreen;

            if (bs.get_screen_state() == BlackScreen.ScreenState.Fade_Out)
            {
                black_screen.change_screen(BlackScreen.ScreenState.Fade_In);
                count++;
            }
        }

    }
    public void set_is_shake()
    {
        is_shake = true;
    }

    public void end()
    {
        ac.SetStateTarget(player.transform, ActionCamera.State.Follow, ac.default_speed, 0.0f, ac.default_angle, "player_follow", Vector3.zero); boss_ani.SetBool("is_end", true);
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(time_list[count]);
        count++;
        _timer = null;
    }
}
