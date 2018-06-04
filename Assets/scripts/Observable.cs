using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observable : MonoBehaviour {
    public List<Observer> observers;

    public void draw_debug_wire(Transform target, Color color)
    {
        Debug.DrawLine(transform.position, target.position, color);
    }

    public void add_observer(Observer observer)
    {
        observers.Add(observer);
    }

    public void remove_observer(Observer observer)
    {
        observers.Remove(observer);
    }
}
