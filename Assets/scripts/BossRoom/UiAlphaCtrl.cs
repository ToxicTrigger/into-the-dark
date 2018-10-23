using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAlphaCtrl : MonoBehaviour {

    public Image image;
    public Text text;
    public float speed;

    IEnumerator timer;
    public void onoff_ui(bool _onoff)
    {
        if(timer != null)
            StopCoroutine(timer);
        timer = alpha_ctrl(_onoff);
        StartCoroutine(timer);
    }

    IEnumerator alpha_ctrl(bool _onoff)
    {
        Color _image_color = Color.white , _text_color = Color.white;
        if (_onoff)
        {
            _image_color.a = 0;
            _text_color.a = 0;
        }
        else
        {
            _image_color.a = 1;
            _text_color.a = 1;
        }

        while(true)
        {
            if(_onoff)
            {
                _text_color.a += speed;
                _image_color.a += speed;
            }
            else
            {
                _text_color.a -= speed;
                _image_color.a -= speed;
            }

            text.color = _text_color;
            image.color = _image_color;

            yield return new WaitForSeconds(0.01f);

            if (_onoff && _text_color.a >= 1 && _image_color.a >= 1)
                break;
            else if (!_onoff && _text_color.a <= 0 && _image_color.a <= 0)
                break;
        }
    }

    
}
