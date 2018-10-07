using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaInfo : MonoBehaviour
{
    public Text areaTitle, areaName;
    public Image L, R;
    public Color text_color;
    public bool EventOn;
    public float speed = 0.01f;
    IEnumerator up()
    {
        while (text_color.a <= 1)
        {
            text_color.a += speed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Up()
    {
        if (!EventOn)
            StartCoroutine(up());
    }

    public void Down()
    {
        StartCoroutine(down());
    }

    IEnumerator down()
    {
        while (text_color.a >= 0)
        {
            text_color.a -= speed;
            yield return new WaitForSeconds(0.01f);
        }
    }
    void Start()
    {
        text_color = areaTitle.color;
    }

    // Update is called once per frame
    void Update()
    {
        areaTitle.color = text_color;
        areaName.color = text_color;
        L.color = text_color;
        R.color = text_color;
    }
    IEnumerator UpDown()
    {
        Up();
        yield return new WaitForSeconds(3.0f);
        Down();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            areaTitle.text = other.name;
            areaName.text = other.transform.GetChild(0).name;
            StartCoroutine(UpDown());
        }
    }
}
