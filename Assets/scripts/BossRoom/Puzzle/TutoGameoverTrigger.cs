using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoGameoverTrigger : MonoBehaviour {

    Player player;
    Vector3 start_pos;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        start_pos = player.gameObject.transform.position;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Player"))
        {
            player.transform.position = start_pos;
            player.GetComponent<Damageable>().Hp = player.GetComponent<Damageable>().Max_Hp;
        }
    }
}
