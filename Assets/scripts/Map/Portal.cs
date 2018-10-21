using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform des;
    public Animator fade;
    public bool turn_on_pin;
    public CalcPinDist cpd;
    public bool change_ac_angle;
    public ActionCamera ac;
    public Vector3 euler_angle;
    public Vector3 offset;
    public int h = 1, v = 1;
    public bool reverse;

    IEnumerator move(Collider other, float time)
    {
        float speed = cpd.Speed;
        fade.SetBool("fade", true);
        yield return new WaitForSeconds(time / 2);
        other.transform.position = des.position;
        cpd.enabled = turn_on_pin;
        cpd.Speed = 10;
        yield return new WaitForSeconds(time);

        FindObjectOfType<PlayerMove>().reverse = reverse;
        FindObjectOfType<PlayerMove>().cus_x = h;
        FindObjectOfType<PlayerMove>().cus_z = v;

        ac.Angle = euler_angle;
        ac.Offset = offset;
        ac.SetStateTarget(FindObjectOfType<Player>().transform, ActionCamera.State.Follow);

        yield return new WaitForSeconds(time);
        cpd.Speed = speed;
        fade.SetBool("fade", false);
        FindObjectOfType<PlayerMove>().spawn_point.position = des.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            StartCoroutine(move(other, 1.0f));
        }
    }
}
