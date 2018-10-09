using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Image Main, Sub;
    public Player player;
    public Sprite use_sword, unuse_sword, use_bow, unuse_bow;
    public Image alpha;
    public Color a;
    public float aa;
    public bool changed;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(!changed)
        {
            if( player.weapon.type == Weapon.Type.Bow )
            {
                Main.sprite = use_bow;
                Sub.sprite = unuse_sword;
                aa = 1;
            }
            else if( player.weapon.type == Weapon.Type.Sword )
            {
                Main.sprite = use_sword;
                Sub.sprite = unuse_bow;
                aa = 1;
            }
            changed = true;
        }
        aa = Mathf.Lerp(aa , 0 , Time.deltaTime * 2);
        a.a = aa;
        alpha.color = a;
    }
}
