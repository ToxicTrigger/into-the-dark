using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAlphaCtrl : MonoBehaviour {

	void Start () {
    }
    public SkinnedMeshRenderer mr;
    public float alpha=1;
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
        {
            alpha -= 0.1f;
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            alpha += 0.1f;
        }

        mr.material.color = (new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, alpha));

    }
}
