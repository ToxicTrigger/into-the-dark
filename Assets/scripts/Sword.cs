using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    public Player player;
    public Element type;
    public float power = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (player.weapon.type == Weapon.Type.Sword)
        {

            Debug.Log(type.type);
            if( other.tag.Equals("Element"))
            {
                Element e = other.GetComponent<Element>();
            }
            if (other.tag.Equals("Enemy"))
            {
                Look enemy = other.GetComponent<Look>();
                type = player.cur_attack_type;
                enemy.onAttack(power);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
