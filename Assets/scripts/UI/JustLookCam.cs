﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustLookCam : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
