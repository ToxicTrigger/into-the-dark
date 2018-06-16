using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypeChange : MonoBehaviour {
	public Player player;
	public Weapon.Type type;
	public GameObject sword, bow;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();
		bow = transform.GetChild(0).gameObject;
		sword = transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		type = player.weapon.type;
		switch(type)
		{
			case Weapon.Type.Bow : bow.SetActive(true); sword.SetActive(false); break;
			case Weapon.Type.Sword : bow.SetActive(false); sword.SetActive(true); break;
		}
	}
}
