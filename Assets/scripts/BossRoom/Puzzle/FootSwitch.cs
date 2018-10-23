﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSwitch : Observer {

    enum State
    {
        Up,
        Down,

    }
    State state;

    public GameObject switch_ground;
    public float move_speed;
    public Vector3 idle_position;
    public float y_up_pos;
    SoundManager sound_manager;
    public List<Collider> p_coll;

    IEnumerator move_corutine;

    public int on_count =0;

    public bool is_time_switch;

    public TargetUI target_ui;

	void Start () {
        p_coll = new List<Collider>();
        move_corutine = ground_move(Vector3.up);

        sound_manager = SoundManager.get_instance();
        if(switch_ground != null)
            idle_position = switch_ground.transform.position;
    }	

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player") || other.CompareTag("Enemy"))
        {
            if (!p_coll.Contains(other))
            {
                on_count++;
                if(this.name == "FootSwitch_tuto")
                    Debug.Log(this.name + " || "+other.gameObject.name);
            }
            ground_move_ctrl(Vector3.up);

            if(other.CompareTag("Enemy"))
            {
                other.transform.Find("DestroyCheck(Clone)").GetComponent<DestroyCheck>().add_observer(this);
            }
            else
            {
                if (!p_coll.Contains(other))
                    p_coll.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Player") || other.CompareTag("Enemy"))
        {
            if (other.CompareTag("Enemy"))
            {
                on_count--;
                other.transform.Find("DestroyCheck(Clone)").GetComponent<DestroyCheck>().remove_observer(this);
            }
            else if (p_coll.Contains(other))
            {
                p_coll.Remove(other);
                on_count--;
            }
            off_switch();
        }
    }

    IEnumerator ground_move(Vector3 _move_dir)
    {
        if (target_ui != null && _move_dir == Vector3.up)
            target_ui.gameObject.SetActive(true);
        else if (target_ui != null && _move_dir == Vector3.down)
            target_ui.gameObject.SetActive(false);

        while (true)
        {
            switch_ground.transform.position += _move_dir * move_speed * Time.deltaTime;

            if(_move_dir == Vector3.down)
            {
                if (is_time_switch && switch_ground.GetComponent<TimeSwitch>().get_switch())
                {
                    state = State.Down;
                    switch_ground.GetComponent<TimeSwitch>().off_switch();
                    switch_ground.GetComponent<TimeSwitch>().set_use_enable(true);
                }
            }

            yield return new WaitForSeconds(0.01f);

            if (_move_dir == Vector3.up &&
                switch_ground.transform.position.y > idle_position.y + y_up_pos)
            {
                switch_ground.transform.position = new Vector3(idle_position.x, idle_position.y + y_up_pos, idle_position.z);
                state = State.Up;
                if (is_time_switch)
                    switch_ground.GetComponent<TimeSwitch>().set_use_enable(true);
                break;
            }
            else if (_move_dir == Vector3.down &&
                     switch_ground.transform.position.y < idle_position.y)
            {
                switch_ground.transform.position = idle_position;
                if (is_time_switch && switch_ground.GetComponent<TimeSwitch>().get_switch())
                {
                    state = State.Down;
                    switch_ground.GetComponent<TimeSwitch>().off_switch();
                }
                break;
            }
        }
    }

    public void set_ground(GameObject _ground)
    {
        switch_ground = _ground;
        idle_position = switch_ground.transform.position;
    }

    public void ground_move_ctrl(Vector3 _dir)
    {
        //Debug.Log("땅 움직이는 방향" + _dir);
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
        if (FindObjectOfType<BossRoomManager>())
        {
            if (is_time_switch && on_count <= 0 && BossRoomManager.get_instance().get_ancient_weapon().get_state() != AncientWeapon.State.Activated)
            {
                ground_move_ctrl(Vector3.down);
                switch_ground.GetComponent<TimeSwitch>().set_use_enable(false);
            }
            else if (!is_time_switch && on_count <= 0)
            {
                ground_move_ctrl(Vector3.down);
            }
        }
        else
        {
            if (is_time_switch && on_count <= 0)
            {
                ground_move_ctrl(Vector3.down);
                switch_ground.GetComponent<TimeSwitch>().set_use_enable(false);
            }
        }
    }

}
