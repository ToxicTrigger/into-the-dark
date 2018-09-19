using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toxic
{
    public class DestroyCheck : Switch
    {
        [SerializeField]
        private GameObject check_target;

        public new void Update()
        {
            this.OnOff = check_target == null ? true : false;
        }
    }
}


