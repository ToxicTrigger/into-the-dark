using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyhaBuilder
{
    string name;
    int age;

    static SyhaBuilder b;

    public SyhaBuilder setName(string name)
    {
        this.name = name;
        return this;
    }

    public SyhaBuilder setAge(int Age)
    {
        age = Age;
        return this;
    }

    public Syha build()
    {
        Syha s = new Syha();
        s.age = age;
        s.name = name;
        return s;
    }
}

public class Syha
{
    public string name;
    public int age;
    

    public static SyhaBuilder New()
    {
        SyhaBuilder b = new SyhaBuilder();
        return b;
    }
}

public class Observable : MonoBehaviour {
    public List<Observer> observers;

    public void draw_debug_wire(Transform target, Color color)
    {
        Debug.DrawLine(transform.position, target.position, color);
        Syha s = Syha.New().setAge(1).build();
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
