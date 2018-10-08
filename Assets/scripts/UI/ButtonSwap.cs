using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwap : MonoBehaviour
{
    public GameObject b1, b2;
    public void swap(int i)
    {
        if(i == 1)
        {
            b1.SetActive(false);
            b2.SetActive(true);
        }
        else
        {
            b1.SetActive(true);
            b2.SetActive(false);
        }
    }
}
