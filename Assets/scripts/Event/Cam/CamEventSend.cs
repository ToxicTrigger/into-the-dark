using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEventSend : MonoBehaviour
{
    public ActionCamera ac;
    public PlayerMove pm;
    public float time = 1;
    public int TransformNum;
    Player player;

    public bool UseZoomInOut;
    public bool zoomIn;
    public float fov, zoom_speed;

    [Tooltip("기본 속도(1) * action_speed")]
    public float action_speed;

    public void Start()
    {
        ac = FindObjectOfType<ActionCamera>();
        pm = FindObjectOfType<PlayerMove>();
        player = FindObjectOfType<Player>();
    }

    IEnumerator Stop_PlayerMove()
    {
        CharacterController cc = FindObjectOfType<CharacterController>();
        //cc.enabled = false;

        yield return new WaitForSeconds(time + 1.2f);
        //cc.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.name.Equals("Player") )
        {
            if(UseZoomInOut)
            {
                ac.ZoomInOut(fov, zoomIn, zoom_speed );
            }
            
            ac.Angle = ac.Pins[TransformNum].eulerAngles;
            ac.SetStateTarget(ac.Pins[TransformNum] , ActionCamera.State.Move_Pin, ac.default_speed * action_speed);
            StartCoroutine(Stop_PlayerMove());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.gameObject.name.Equals("Player") )
        {
            if (UseZoomInOut)
            {
                if (fov >= ac.default_fov)
                {
                    ac.ZoomInOut(ac.default_fov, true, zoom_speed);
                }
                else
                {
                    ac.ZoomInOut(ac.default_fov, false, zoom_speed);
                }
            }
            
            ac.Angle = ac.default_angle;
            //ac.transform.eulerAngles = ac.default_angle;
            ac.SetStateTarget(player.transform , ActionCamera.State.Follow, ac.default_speed);
        }
    }


}
