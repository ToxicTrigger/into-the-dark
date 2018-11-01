using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleAnimation : MonoBehaviour
{
    public Animator ani;
    public GameObject UI;
    public GameObject Char;
    public ActionCamera cam;
    public bool has_input_end, has_fade_out, somting_click;
    public int input;

    public bool end;

    public void Titlebool(int has_input)
    {
        if( has_input == 1 )
        {
            has_input_end = true;
        }
    }

    public void FadeOut(int outFade)
    {
        if( outFade == 1 )
        {
            has_fade_out = true;
        }
    }
    
    public void Click(int target)
    {
        if( somting_click )
        {
            somting_click = false;
        }
        else
        {
            somting_click = true;
        }

        input = target;
    }

    private void Start()
    {
        UI.SetActive(false);
        Char.SetActive(false);
        cam.action_speed = 0;
        cam.enabled = false;
        
    }

    void Update()
    {
        if( has_input_end )
        {
            if( Input.anyKeyDown )
            {
                ani.SetBool("input" , true);
                has_input_end = false;
            }
        }

        if( somting_click )
        {
            ani.SetBool("out" , true);
        }

        if( has_fade_out )
        {
            if(!end)
            {
                switch( input )
                {
                    case 0:
                        Char.SetActive(true);
                        UI.SetActive(true);

                        
                        cam.enabled = true;
                        cam.action_speed =0.1f;
                        gameObject.SetActive(false);
                      
                        break;
                    case 3:
                        Application.Quit();
                        break;

                }
                end = true;
            }

        }
    }
}
