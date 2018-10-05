using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 180806 수정됨.
public class AlphaCaster : MonoBehaviour
{
    public Player player;
    public bool draw_ui;
    public float speed;
    public Color alpha;
    public string layerName;

    // 새로 추가된 변수 입니다! 
    // 해당 변수가 활성화 된 상태에서는 초기 init 단계에서 객체 각각의 color 값을 기억하도록 합니다.
    public bool remember_initial_value;
    public bool is_Totem_UI;

    public List<Image> uis;
    public List<Text> texts;

    //아래 두 리스트 또한 새로 추가 됩니다.
    public List<Color> ui_colors;
    public List<Color> text_colors;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        uis = new List<Image>();
        texts = new List<Text>();
        if (remember_initial_value)
        {
            ui_colors = new List<Color>();
            text_colors = new List<Color>();
        }

        Image[] img = FindObjectsOfType<Image>();
        Text[] tex = FindObjectsOfType<Text>();

        int ui = LayerMask.NameToLayer(layerName);

        IEnumerator iter = img.GetEnumerator();
        while (iter.MoveNext())
        {
            Image i = iter.Current as Image;
            if (i.gameObject.layer.Equals(ui))
            {
                uis.Add(i);
                if (remember_initial_value)
                {
                    ui_colors.Add(i.color);
                }
            }
        }

        iter = tex.GetEnumerator();
        while (iter.MoveNext())
        {
            Text i = iter.Current as Text;
            if (i.gameObject.layer.Equals(ui))
            {
                texts.Add(i);
                if (remember_initial_value)
                {
                    text_colors.Add(i.color);
                }
            }
        }
    }

    bool EventOn;
    IEnumerator up()
    {
        EventOn = true;
        if (remember_initial_value)
        {
            bool done = false;
            while (!done)
            {

                IEnumerator iter = uis.GetEnumerator();
                while (iter.MoveNext())
                {
                    Image img = iter.Current as Image;
                    float a = img.color.a;
                    //TODO 컬러 값 회귀 코드 적용해야 함
                    img.color = alpha;
                }
                iter = texts.GetEnumerator();
                while (iter.MoveNext())
                {
                    Text img = iter.Current as Text;
                    img.color = alpha;
                }
                yield return new WaitForSeconds(0.001f);
            }
        }
        else
        {
            while (alpha.a <= 1)
            {
                alpha.a += speed;

                IEnumerator iter = uis.GetEnumerator();
                while (iter.MoveNext())
                {
                    Image img = iter.Current as Image;
                    img.color = alpha;
                }
                iter = texts.GetEnumerator();
                while (iter.MoveNext())
                {
                    Text img = iter.Current as Text;
                    img.color = alpha;
                }

                yield return new WaitForSeconds(0.001f);
            }
        }

        EventOn = false;
    }

    public void Up()
    {
        if (!EventOn)
            StartCoroutine(up());
    }

    public void Down()
    {
        if (!EventOn)
            StartCoroutine(down());
    }

    IEnumerator down()
    {
        EventOn = true;
        while (alpha.a >= 0)
        {
            alpha.a -= speed;

            IEnumerator iter = uis.GetEnumerator();
            while (iter.MoveNext())
            {
                Image img = iter.Current as Image;
                img.color = alpha;
            }
            iter = texts.GetEnumerator();
            while (iter.MoveNext())
            {
                Text img = iter.Current as Text;
                img.color = alpha;
            }

            yield return new WaitForSeconds(0.05f);
        }
        EventOn = false;
    }

    float tick;
    bool hasOn;

    void Update()
    {
        if (hasOn)
        {
            if (tick >= 3)
            {
                tick = 0;
                hasOn = false;
                Down();
            }
        }
        if (!is_Totem_UI)
        {
            if (!player.is_target_something.Equals(0) || player.is_fighting_something)
            {
                hasOn = true;
                tick = 0;
                Up();
            }
            else
            {
                tick += Time.deltaTime;
            }
        }
        else
        {
            if (player != null)
            {
                if (player.is_build_totem)
                {
                    hasOn = true;
                    tick = 0;
                    Up();
                }
                else
                {
                    tick += Time.deltaTime;
                }
            }
        }
    }
}
