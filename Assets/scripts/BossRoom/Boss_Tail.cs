using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tail : MonoBehaviour
{
    public Transform child;

    public Vector3 boneAxis = new Vector3(0.0f, -1.0f, 0.0f);

    public float dragForce = 0.4f;
    public float stiffnessForce = 0.01f;

    float springLength;

    Vector3 currTipPos;
    Vector3 prevTipPos;

    Quaternion localRotation;

    Transform tr;

    public Boss_Worm boss;
    public Boss_State state;

    void Awake()
    {
        tr = transform;
        localRotation = transform.localRotation;
    }

    private void Start()
    {        
        if (child != null)
        {
            springLength = Vector3.Distance(tr.position, child.position);
            currTipPos = child.position;
            prevTipPos = child.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (state.get_state() == Boss_State.State.Groggy)
        {
            if (other.CompareTag("Arrow"))
            {
                boss.add_damage();
                Destroy(other.gameObject);
            }
            if (other.CompareTag("Sword"))
            {
                boss.add_damage();
            }
        }
    }

    Boss_Tail head_seach(Transform tail)
    {
        if (tail.parent.GetComponent<Boss_Tail>() == null)
        {
            return tail.GetComponent<Boss_Tail>();
        }
        else
        {
            return head_seach(tail.parent);
        }
    }

    public void move_update(Vector3 _move_dir)
    {

        tr.localRotation = Quaternion.identity * localRotation;

        float sqrDt = Time.deltaTime * Time.deltaTime;

        Vector3 force = tr.rotation * (boneAxis * stiffnessForce) / sqrDt;

        force += (prevTipPos - currTipPos) * dragForce / sqrDt;


        Vector3 temp = currTipPos;

        currTipPos = (currTipPos - prevTipPos) + currTipPos + (force * sqrDt);
        currTipPos = ((currTipPos - tr.position).normalized * springLength) + tr.position;

        prevTipPos = temp;

        Vector3 originAxis = tr.TransformDirection(boneAxis);
        Quaternion targetDir = Quaternion.FromToRotation(originAxis, currTipPos - tr.position);

        tr.rotation = targetDir * tr.rotation;
    }

    IEnumerator rot_set_timer()
    {

        yield return new WaitForSeconds(0.5f);
    }

}
