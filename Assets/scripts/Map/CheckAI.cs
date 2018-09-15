using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAI : Switch
{
    private List<GameObject> AIs;
    private bool Empty;

    public void OnTriggerEnter(Collider collision)
    {
        if( collision.gameObject.CompareTag("Enemy") )
        {
            if( !AIs.Contains(collision.gameObject) )
            {
                AIs.Add(collision.gameObject);
            }
        }
    }

    private void remove_dead_ai()
    {
        for( int i = 0 ; i < AIs.Count ; ++i )
        {
            GameObject item = AIs[ i ];
            if( item == null )
            {
                AIs.RemoveAt(i);
            }
        }
    }

    public new void Update()
    {
        Empty = AIs.Count == 0 ? true : false;
        this.OnOff = !Empty;
        remove_dead_ai();
    }
}
