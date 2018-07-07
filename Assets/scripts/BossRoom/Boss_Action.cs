using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Action : MonoBehaviour {

    Boss_State state;

	void Start () {
        state = GetComponent<Boss_State>();
	}
	
	void Update () {

        switch (state.get_state())
        {
            case Boss_State.State.Idle:

                break;
            case Boss_State.State.Rush_Attack:

                break;
            case Boss_State.State.Whipping_Attack:

                break;
            case Boss_State.State.Soar_Attack:

                break;
            case Boss_State.State.Groggy:

                break;
            case Boss_State.State.Groggy_End:

                break;
            case Boss_State.State.Death:

                break;
            case Boss_State.State.Ready:

                break;
            case Boss_State.State.Up:

                break;
            default:
                break;
        }

    }
}
