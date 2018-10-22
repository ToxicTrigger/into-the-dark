using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : Observer {

    public enum Type
    {
        UpDown,
    }

    [Tooltip("첫 움직임은 Up을 기준으로 한다. (아래 배치)")]
    public Type type;
    public float move_speed;
    public float y_move_distance;
    float default_y_pos;
    //첫 번째 움직임?
    bool is_first =  true;

    IEnumerator move_timer;

    public void Start()
    {
        default_y_pos = transform.position.y;
        
    }

    public override void notify(Observable obj)
    {
        BasicSwitch t_switch = obj as BasicSwitch;

        if (t_switch.get_switch())
        {
            move_this(Vector3.up);
        }
        else
        {
            move_this(Vector3.down);
        }
    }

    void move_this(Vector3 _dir)
    {
        if(move_timer != null)
            StopCoroutine(move_timer);
        move_timer = simple_move(_dir);
        StartCoroutine(move_timer);
    }

    IEnumerator simple_move(Vector3 _dir)
    {
        Vector3 dir = _dir;

        while (true)
        {
            if(!SoundManager.get_instance().sound_list[(int)SoundManager.SoundList.rumble].isPlaying)
                SoundManager.get_instance().play_sound(SoundManager.SoundList.rumble);
            transform.position += dir * move_speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);

            if ((dir == Vector3.up && transform.position.y >= default_y_pos + y_move_distance) || (dir == Vector3.down && transform.position.y <=default_y_pos))
                break;
        }
        SoundManager.get_instance().stop_sound(SoundManager.SoundList.rumble,false);

        move_timer = null;
    }

}
