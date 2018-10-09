using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUi : MonoBehaviour
{
    public List<Image> Hp_bars;
    public List<Image> Hp_Reds;
    public List<GameObject> Hps;
    public Player player;
    public Damageable player_hp;
    public RectTransform End;

    void Start()
    {
        player = FindObjectOfType<Player>();
        player_hp = player.GetComponent<Damageable>();
    }

    void Update()
    {
        for( int i = 0 ; i < (int)( player_hp.Max_Hp / 100 ); ++i )
        {
            Hp_bars[ i ].enabled = true;
            Hp_Reds[ i ].enabled = true;
            Hps[ i ].SetActive(true);
            Vector3 des;
            int hp = (int)( player_hp.Max_Hp / 100 );

            des = new Vector3(196 + ( ( i + 2 ) * 185 ) , -1.6f , 0);
            Hp_bars[ i ].fillAmount =  ( (player_hp.Hp - i * 100) * 0.01f );
            End.anchoredPosition3D = Vector3.Lerp(End.anchoredPosition3D , des , Time.deltaTime * 2);

            if( Hp_bars[i].fillAmount == 0)
            {
                Hp_Reds[ i ].fillAmount = 0;
            }
            else
            {
                Hp_Reds[ i ].fillAmount = Mathf.Lerp(Hp_Reds[ i ].fillAmount , Hp_bars[ i ].fillAmount , Time.deltaTime * 2);
            }
        }

        for( int i = 2 ; i > (int)( player_hp.Max_Hp / 100 ) - 1 ; --i )
        {
            Hp_bars[ i ].enabled = false;
            Hp_Reds[ i ].enabled = false;
            Hps[ i ].SetActive(false);
        }
    }
}
