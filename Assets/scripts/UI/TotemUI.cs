using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TotemUI : MonoBehaviour
{
    public Sprite Usable , Used;
    public List<Image> totems;
    Player player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>();        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0 ; i < player.installable_totems ; ++i )
        {
            totems[ i ].sprite = Usable;
        }

        for(int i = 4 ; i > player.installable_totems-1; --i)
        {
            totems[ i ].sprite = Used;
        }
    }

}
