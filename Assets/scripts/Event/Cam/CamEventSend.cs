using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEventSend : MonoBehaviour
{
    public ActionCamera ac;
    public float time = 1;
    public int TransformNum;

    public bool UseZoomInOut;
    public bool zoomIn;
    public float fov, zoom_speed;

    public void Start()
    {
        ac = FindObjectOfType<ActionCamera>();
    }

    IEnumerator Stop_PlayerMove()
    {
        CharacterController cc = FindObjectOfType<CharacterController>();
        cc.enabled = false;
        yield return new WaitForSeconds(time);
        cc.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.name.Equals("PlayerAggro") )
        {
            if(UseZoomInOut)
            {
                ac.ZoomInOut(fov, zoomIn, zoom_speed );
            }
            
            ac.Angle = ac.Pins[TransformNum].eulerAngles;
            ac.SetStateTarget(ac.Pins[TransformNum] , ActionCamera.State.Move_Pin);
            StartCoroutine(Stop_PlayerMove());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.gameObject.name.Equals("PlayerAggro") )
        {
            if(fov >= ac.default_fov)
            {
                ac.ZoomInOut(ac.default_fov , true , zoom_speed);
            }
            else
            {
                ac.ZoomInOut(ac.default_fov , false , zoom_speed);
            }
            
            ac.Angle = ac.default_angle;
            //ac.transform.eulerAngles = ac.default_angle;
            ac.SetStateTarget(ac.cur_target , ActionCamera.State.Follow);
        }
    }


}
