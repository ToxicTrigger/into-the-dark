using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputHandler : MonoBehaviour, Toxic.InputListener
{
    public abstract void Work(InputManager im);
}
