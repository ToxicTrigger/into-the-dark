using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPlatform : Platform
{
    public GameObject Cliff;
    public float speed = 2.0f;
    public float event_time = 0;
    public float y;

    public Vector3 Down, Up;
    private void Awake()
    {
        Down = Cliff.transform.position;
        Down.y -= 30;
        Up = Cliff.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case EventState.Off:
                Vector3 pos = Vector3.Lerp(Cliff.transform.position, Down, Time.deltaTime);
                Cliff.transform.position = pos;
                break;

            case EventState.On:
                pos = Vector3.Lerp(Cliff.transform.position, Up, Time.deltaTime);
                Cliff.transform.position = pos;
                break;
        }
    }
}
