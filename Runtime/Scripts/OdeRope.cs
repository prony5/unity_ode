using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Tools/Rope")]
    public class OdeRope : MonoBehaviour
    {
        #region Ispector
        public float lenght = 1;
        public float diameter = 0.01f;
        public int linkCount = 20;
        public int segments = 20;
        public float mass = 1;
        public float limit1 = 45;
        public float limit2 = 45;
        public float friction = 0;
        public Material materialExternal;
        public Material materialInternal;

        public OdeBody startBody;
        public OdeBody endBody;
        [HideInInspector]
        public OdeJoint endJoint;
        [HideInInspector]
        public List<GameObject> links = new List<GameObject>();
        [HideInInspector]
        public SkinnedMeshRenderer skin;
        #endregion

    }

}