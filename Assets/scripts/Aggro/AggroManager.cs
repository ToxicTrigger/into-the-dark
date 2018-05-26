using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroManager : MonoBehaviour
{
    private static AggroManager instance;
    [SerializeField]
    private GameObject Aggro;

    public static AggroManager get_instance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(AggroManager)) as AggroManager;
        }
        return instance;
    }

    public GameObject gen_aggro(Vector3 pos, float power, float time)
    {
        GameObject tmp = Instantiate(Aggro, pos, Quaternion.identity, null);
        tmp.GetComponent<AggroObject>().aggro_point = power;
        Destroy(tmp, time);
        return tmp;
    }
}



