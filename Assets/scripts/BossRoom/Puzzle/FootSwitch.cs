using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSwitch : MonoBehaviour {

    TimeSwitch switch_ground;
    public float move_speed;
    public Vector3 idle_position;
    public float y_up_pos;

    IEnumerator move_corutine;

    public int on_count =0;

	void Start () {
        move_corutine = ground_move(Vector3.up);
    }	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            on_count++;
            Debug.Log("발판에 올라옴");

            switch_ground.set_use_enable(true);
            ground_move_ctrl(Vector3.up);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            on_count--;
            Debug.Log("발판에서 내려감");
            if (on_count <= 0 && BossRoomManager.get_instance().get_ancient_weapon().get_state() != AncientWeapon.State.Activated )
            {
                switch_ground.set_use_enable(false);
                ground_move_ctrl(Vector3.down);
            }
        }
    }

    IEnumerator ground_move(Vector3 _move_dir)
    {
        while (true)
        {
            switch_ground.transform.position += _move_dir * move_speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);

            if(_move_dir == Vector3.up &&
                switch_ground.transform.position.y > idle_position.y + y_up_pos)
            {
                switch_ground.transform.position = new Vector3(idle_position.x, idle_position.y + y_up_pos, idle_position.z);
                break;
            }
            else if(_move_dir == Vector3.down &&
                switch_ground.transform.position.y < idle_position.y)
            {
                switch_ground.transform.position = idle_position;
                if (switch_ground.get_switch())
                {
                    switch_ground.off_switch();
                }
                break;
            }
        }

    }

    public void set_ground(TimeSwitch _ground)
    {
        switch_ground = _ground;
        idle_position = switch_ground.transform.position;
    }

    public void ground_move_ctrl(Vector3 _dir)
    {
        StopCoroutine(move_corutine);
        move_corutine = ground_move(_dir);
        StartCoroutine(move_corutine);
    }

}
