using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public List<GameObject> positions = new List<GameObject>();
    public int Level;
    public bool lookPlayer;
    public Transform player;
    public Vector3 offset;
	
	void Update () {
        int cantFind = 0;
        IEnumerator iter = positions.GetEnumerator();
        while(iter.MoveNext())
        {
            GameObject tmp = iter.Current as GameObject;
            Detecter tmp1 = tmp.GetComponent<Detecter>();
            if (tmp1.is_fined)
            {
                Level = int.Parse(tmp.name);
                cantFind += 1;
            }
        }
        if(cantFind != 0)
        {
            transform.position = Vector3.Lerp(transform.position, positions[Level].transform.position, Time.deltaTime);
            lookPlayer = false;
        }
        else if(cantFind == 0)
        {
            lookPlayer = true;
        }

        if (lookPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 10);
        }

    }


	
}
