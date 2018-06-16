using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashObject : MonoBehaviour {

    [Tooltip("부숴지는데 영향을 주는 오브젝트 태그")]
    public string target_tag;
    public Rigidbody[] piece;

	void Start () {
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(target_tag))
        {
            for (int i =0; i<piece.Length; i++)
            {
                piece[i].isKinematic = false;
            }
        }
    }

}
