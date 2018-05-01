using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Observable {
    public bool switch_on;
    public bool point_on;
    public Element element;
    public Light point;

    private void Start()
    {
        element = GetComponent<Element>();
    }
    
    void light_source

    private void Update()
    {
        if (switch_on)
        {
            point.gameObject.SetActive(true);

            IEnumerator iterr = this.observers.GetEnumerator();
            while (iterr.MoveNext())
            {
                Observer tmp = iterr.Current as Observer;
                draw_debug_wire(tmp.transform, Color.yellow);
            }
        }
        else
        {
            point.gameObject.SetActive(false);
            IEnumerator iterr = this.observers.GetEnumerator();
            while (iterr.MoveNext())
            {
                Observer tmp = iterr.Current as Observer;
                draw_debug_wire(tmp.transform, Color.white);
            }
        }
    }

    void notify_all()
    {
        IEnumerator iterr = this.observers.GetEnumerator();
        while (iterr.MoveNext())
        {
            Gate tmp = iterr.Current as Gate;
            tmp.notify(this);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Element other_element = other.GetComponent<Element>();
        if(other_element != null)
        {
            switch_on = other_element.type == element.type ? true : false;
            Debug.Log("fail : " + gameObject.name + ", " + other.name + " | " + switch_on);
            if (switch_on)
            {
                if (!point_on)
                {
                    point_on = true;
                }
                else
                {
                    switch_on = false;
                    point_on = false ;
                    notify_all();
                    return;
                }

                notify_all();
            }
            else
            {
                point_on = false;
                notify_all();
            }
        }
        else
        {
            Debug.Log("fail : " + gameObject.name + ", " + other.name);
        }
    }
}
