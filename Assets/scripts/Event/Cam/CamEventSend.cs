using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEventSend : MonoBehaviour
{
    public ActionCamera ac;
    public PlayerMove pm;
    public int TransformNum;
    Player player;

    public bool UseZoomInOut;
    public bool zoomIn;
    public float fov, zoom_speed;
    public bool Change_angle;
    public Vector3 euler_angle;
    public Vector3 offset;
    public float change_ac_speed;

    public void Start()
    {
        ac = FindObjectOfType<ActionCamera>();
        pm = FindObjectOfType<PlayerMove>();
        player = FindObjectOfType<Player>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.name.Equals("Player") )
        {
            if(UseZoomInOut)
            {
                ac.ZoomInOut(fov, zoomIn, zoom_speed );
            }
            if(!Change_angle)
            {
                ac.Angle = ac.Pins[ TransformNum ].eulerAngles;
                ac.SetStateTarget(ac.Pins[ TransformNum ] , ActionCamera.State.Move_Pin);
            }
            else
            {
                ac.Angle = euler_angle;
                ac.Offset = offset;
                ac.SetStateTarget(player.transform , ActionCamera.State.Follow);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.gameObject.name.Equals("Player") )
        {
            if(fov >= ac.default_fov)
            {
                ac.ZoomInOut(ac.default_fov , true , zoom_speed);
            }
            else
            {
                ac.ZoomInOut(ac.default_fov , false , zoom_speed);
            }
            if(Change_angle)
            {
                ac.Offset = ac.default_offset;
            }
            ac.Angle = new Vector3(21,0,0);
            ac.action_speed = change_ac_speed;
            ac.SetStateTarget(player.transform , ActionCamera.State.Follow);
        }
    }


}
