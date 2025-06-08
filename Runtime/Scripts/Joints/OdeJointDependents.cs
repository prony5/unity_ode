using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Utils/Joint Dependents")]
    public class OdeJointDependents : MonoBehaviour
    {
        public bool targetValueReference;
        public ATDependent[] Dependents = new ATDependent[0];

        private OdeJointMove _joint;

        [System.Serializable]
        public struct ATDependent
        {
            public OdeJointMove joint;
            public float scaler;
        }

        void Start()
        {
            _joint = GetComponent<OdeJointMove>();
            if (_joint == null)
                Destroy(this);
        }

        void FixedUpdate()
        {
            /*
            var value = targetValueReference ? _joint.ValueTarget : _joint.Value;
            foreach (var d in Dependents)
                if (d.joint && d.joint.Inited)
                    d.joint.ValueTarget = d.scaler * value;
             */
        }
    }
}