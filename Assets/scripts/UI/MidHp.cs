using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidHp : MonoBehaviour
{
    public Color alpha;
    public Image Red, Yellow, Back;
    public bool start;
    public Damageable damageable;

    private void Start()
    {
        Color a = new Color(1,1,1,0);
        Red.color = a;
        Yellow.color = a;
        Back.color = a;
    }

    // Update is called once per frame
    void Update ()
    {
        if (start)
        {
            alpha.a = Mathf.Lerp(alpha.a, 1, Time.deltaTime);
        }
        else
        {
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime);
        }

        Yellow.fillAmount = Mathf.Lerp(Yellow.fillAmount, damageable.Hp / damageable.Max_Hp, Time.deltaTime * 3);
        Red.fillAmount = Mathf.Lerp(Red.fillAmount, Yellow.fillAmount, Time.deltaTime);

        Red.color = alpha;
        Yellow.color = alpha;
        Back.color = alpha;
	}
}
