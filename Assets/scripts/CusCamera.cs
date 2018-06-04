using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusCamera : MonoBehaviour {
    public List<GameObject> positions = new List<GameObject>();
    public int Level;
    private bool is_look_player;
    public Transform player;
    public Vector3 offset;
    public Vector3 tmp_pos;

    private void Start()
    {
        tmp_pos = transform.position;
    }

    void Update () {
        int cantFind = 0;

        IEnumerator iter = positions.GetEnumerator();
        while(iter.MoveNext())
        {
            GameObject tmp = iter.Current as GameObject;
            Detecter tmp1 = tmp.GetComponent<Detecter>();
            if (tmp1.is_find)
            {
                if(tmp1.target != null & !tmp1.target.tag.Equals("Totem"))
                {
                    Level = int.Parse(tmp.name);
                    cantFind += 1;
                }
            }
        }

        if(cantFind != 0)
        {
            transform.position = Vector3.Lerp(transform.position, positions[Level].transform.position, Time.deltaTime);
            is_look_player = false;
        }
        else if(cantFind == 0)
        {
            is_look_player = true;
        }

        if (is_look_player)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 10);
        }
    }
}
