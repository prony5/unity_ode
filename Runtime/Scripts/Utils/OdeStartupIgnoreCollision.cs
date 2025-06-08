using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Utils/StartupIgnoreCollision")]
    public class OdeStartupIgnoreCollision : MonoBehaviour
    {
        [OdeReadOnlyOnPlay]
        public float time = 1;

        [OdeReadOnlyOnPlay]
        public List<OdeBody> ignoreCollisions = new List<OdeBody>();

        void OnEnable()
        {
            lock (OdeWorld.Sync)
            {
                OdeWorld.OnBeforeStep += SetIgnore;
            }
        }

        void SetIgnore()
        {
            int i = 0;

            while (i < ignoreCollisions.Count)
            {
                if (time <= 0 || ignoreCollisions[i] == null || ignoreCollisions[i].collision.active == false)
                    ignoreCollisions.RemoveAt(i);
                else
                {
                    ignoreCollisions[i].collision.active = false;
                    i++;
                }
            }

            OdeWorld.OnBeforeStep -= SetIgnore;
        }


        // Update is called once per frame
        void Update()
        {
            if (ignoreCollisions.Count == 0)
                return;

            int i = 0;
            while (i < ignoreCollisions.Count)
            {
                if (time < OdeWorld.Statistics.time)
                {
                    ignoreCollisions[i].collision.active = true;
                    ignoreCollisions.RemoveAt(i);
                }
                else
                    i++;
            }
        }
    }
}
