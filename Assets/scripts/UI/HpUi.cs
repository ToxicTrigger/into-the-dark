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
        for( int i = 0 ; i < (int)( player_hp.Max_Hp / 100 ) ; ++i )
        {
            Hp_bars[ i ].enabled = true;
            Hp_Reds[ i ].enabled = true;
            Hps[ i ].SetActive(true);
            Vector3 des;

            //End 옮기는 코드 문제 있는 듯.
            if( i == 0 )
            {
                Hp_bars[ i ].fillAmount = ( player_hp.Hp * 0.01f );
                des = new Vector3(196 , -1.6f , 0);
                End.anchoredPosition3D = Vector3.Lerp(End.anchoredPosition3D , des , Time.deltaTime * 2);
            }
            else
            {
                des = new Vector3(196 + ((i+1) * 256) , -1.6f , 0);
                End.anchoredPosition3D = Vector3.Lerp(End.anchoredPosition3D , des , Time.deltaTime * 2);
                Hp_bars[ i ].fillAmount = ( ( player_hp.Max_Hp / 100 ) - ( i ) ) * ( ( player_hp.Hp * 0.01f ) - i );
            }

            if( i == 2)
            {
                des = new Vector3(196 + ( ( i + 2 ) * 185 ) , -1.6f , 0);
                End.anchoredPosition3D = Vector3.Lerp(End.anchoredPosition3D , des , Time.deltaTime * 2);

            }

            Hp_Reds[ i ].fillAmount = Mathf.Lerp(Hp_Reds[ i ].fillAmount , Hp_bars[ i ].fillAmount , Time.deltaTime);
        }

        for( int i = 2 ; i > (int)( player_hp.Max_Hp / 100 ) - 1 ; --i )
        {
            Hp_bars[ i ].enabled = false;
            Hp_Reds[ i ].enabled = false;
            Hps[ i ].SetActive(false);
        }
    }
}
