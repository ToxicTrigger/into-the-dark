using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSwitch : Observer {

    public GameObject switch_ground;
    public float move_speed;
    public Vector3 idle_position;
    public float y_up_pos;
    SoundManager sound_manager;

    IEnumerator move_corutine;

    public int on_count =0;

    public bool is_time_switch;

	void Start () {
        move_corutine = ground_move(Vector3.up);

        sound_manager = SoundManager.get_instance();
        if(switch_ground != null)
            idle_position = switch_ground.transform.position;
    }	

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") && other.name == "Player") || other.CompareTag("Enemy"))
        {
            on_count++;
            Debug.Log("발판에 올라옴");

            if(is_time_switch)
                switch_ground.GetComponent<TimeSwitch>().set_use_enable(true);

            ground_move_ctrl(Vector3.up);
            if(other.CompareTag("Enemy"))
            {
                other.transform.Find("DestroyCheck(Clone)").GetComponent<DestroyCheck>().add_observer(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player") && other.name == "Player") || other.CompareTag("Enemy"))
        {
            on_count--;
            Debug.Log("발판에서 내려감");

            off_switch();

            if (other.CompareTag("Enemy"))  
            {
                other.transform.Find("DestroyCheck(Clone)").GetComponent<DestroyCheck>().remove_observer(this);
            }
        }
    }

    IEnumerator ground_move(Vector3 _move_dir)
    {
        while (true)
        {
            if(!sound_manager.sound_list[(int)SoundManager.SoundList.rumble].isPlaying)
                sound_manager.play_sound(SoundManager.SoundList.rumble);
            switch_ground.transform.position += _move_dir * move_speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);

            if (_move_dir == Vector3.up &&
                switch_ground.transform.position.y > idle_position.y + y_up_pos)
            {
                switch_ground.transform.position = new Vector3(idle_position.x, idle_position.y + y_up_pos, idle_position.z);
                break;
            }
            else if (_move_dir == Vector3.down &&
                switch_ground.transform.position.y < idle_position.y)
            {
                switch_ground.transform.position = idle_position;
                if (is_time_switch && switch_ground.GetComponent<TimeSwitch>().get_switch())
                {
                    switch_ground.GetComponent<TimeSwitch>().off_switch();
                }
                break;
            }
        }
        sound_manager.stop_sound(SoundManager.SoundList.rumble, true);
    }

    public void set_ground(GameObject _ground)
    {
        switch_ground = _ground;
        idle_position = switch_ground.transform.position;
    }

    public void ground_move_ctrl(Vector3 _dir)
    {
        if (move_corutine != null)
        {
            StopCoroutine(move_corutine);
            move_corutine = ground_move(_dir);
            StartCoroutine(move_corutine);
        }
    }

    public override void notify(Observable observable)
    {
        if (observable.gameObject.GetComponent<DestroyCheck>())
        {
            on_count--;
            off_switch();
        }
    }

    void off_switch()
    {
        if (is_time_switch && on_count <= 0 && BossRoomManager.get_instance().get_ancient_weapon().get_state() != AncientWeapon.State.Activated)
        {
            switch_ground.GetComponent<TimeSwitch>().set_use_enable(false);
            ground_move_ctrl(Vector3.down);
        }
        else if(!is_time_switch)
        {
            ground_move_ctrl(Vector3.down);
        }
    }

}
