using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    public GameObject inventory;

    new void Update()
    {
        base.Update();

        if (this.hasTalking)
        {
            this.target.GetComponent<PlayerMove>().spawn_point = this.transform;
            this.target.GetComponent<Damageable>().Hp = this.target.GetComponent<Damageable>().Max_Hp;

            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }
}
