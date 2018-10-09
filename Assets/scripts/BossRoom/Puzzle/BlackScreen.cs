using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlackScreen : Observable
{
    //화면 페이드인 아웃을 위한 이미지

    RectTransform rt;
    Image black_image;
    public Color color;

    public enum ScreenState
    {
        Fade_In,
        Fade_Out
    };
    ScreenState state;

	void Start () {        
        rt = this.GetComponent<RectTransform>();
        black_image = this.GetComponent<Image>();
        rt.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        black_image.color = color;
        state = ScreenState.Fade_Out;
    }
	
	void Update () {
		
	}

    public void change_screen(ScreenState _state)
    {
        rt.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        if(_state == ScreenState.Fade_In)
            StartCoroutine(fade_in());
        else
            StartCoroutine(fade_out());
    }

    IEnumerator fade_in()
    {
        while (true)
        {
            color.a -= 0.01f;
            black_image.color = color;
            if (color.a <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        state = ScreenState.Fade_In;
    }

    //어두워짐
    IEnumerator fade_out()
    {
        while (true)
        {
            color.a += 0.01f;
            black_image.color = color;
            if (color.a >= 1)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        state = ScreenState.Fade_Out;
        notify_all();
    }

    public void notify_all()
    {
        for (int i = 0; i < this.observers.Count; i++)
        {
            //스위치(퍼즐)은 현재 횃불의 활성화 가능 여부에 관여하므로 신호는 항상 횃불에 보낼것임
            Observer _observers = this.observers[i] as Observer;
            _observers.notify(this);
        }
        observers.Clear();
    }

    public ScreenState get_screen_state()
    {
        return state;
    }
}
