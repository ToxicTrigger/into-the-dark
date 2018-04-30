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

    [ExecuteInEditMode]
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
    [ExecuteInEditMode]
    void draw_debug_wire(Transform target, Color color)
    {
        Debug.DrawLine(transform.position, target.position, color);
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
            if(switch_on)
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
                switch_on = false;
                notify_all();
            }
        }
        else
        {
            Debug.Log("fail : " + gameObject.name + ", " + other.name);
        }
    }
}
