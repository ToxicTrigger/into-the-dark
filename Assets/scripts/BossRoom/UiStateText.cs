using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiStateText : MonoBehaviour {
    //상태 텍스트 출력 스크립트

    public enum TextName
    {
        cognation,
        attack_ready,
        wood_cognition
    };

    public Text text;
    public string[] text_list;
    public Color color;

    public bool show_text;
    public bool is_alpha_ctrl;
    IEnumerator corutine_alpha;

	void Start () {
        off_text_ui();
        text.color = color;
        is_alpha_ctrl = false;
        show_text = true;
        corutine_alpha = alpha_ctrl();
    }

    public void on_text_ui(TextName _state_name)
    {
        text.text = text_list[(int)_state_name];
        if (!is_alpha_ctrl)
        {
            StopCoroutine(corutine_alpha);
            corutine_alpha = alpha_ctrl();
            StartCoroutine(corutine_alpha);
        }
    }    

    public void off_text_ui()
    {
        text.text = null;
    }

    IEnumerator alpha_ctrl()
    {
        is_alpha_ctrl = true;
        while (is_alpha_ctrl)
        {
            if (color.a >= 1)
            {
                show_text = false;
            }

            if(show_text)
                color.a += 0.01f;
            else
                color.a -= 0.01f;

            text.color = color;

            yield return new WaitForSeconds(0.01f);

            if (text.color.a <= 0)
            {
                is_alpha_ctrl=false;
                show_text = true;
                off_text_ui();
            }
        }
    }

}
