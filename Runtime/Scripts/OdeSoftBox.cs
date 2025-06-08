using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Tools/SoftBox")]
    public class OdeSoftBox : MonoBehaviour
    {
        #region Ispector
        public Vector3 size = Vector3.one;
        public float mass = 1;
        [Range(1, 50)]
        public byte boxesInRow = 3;
        [Range(0, float.PositiveInfinity)]
        public float breakForce = float.PositiveInfinity;
        [OdeReadOnly]
        public int boxesCount = 0;
        #endregion

        public class Box
        {
            public Vector3 center;
            public Vector3 size;
            public Box(Vector3 aCenter, Vector3 aSize)
            {
                center = aCenter;
                size = aSize;
            }
        }
        [HideInInspector]
        public List<Box> boxes = new List<Box>();
        [HideInInspector]
        public List<GameObject> boxObjs = new List<GameObject>();
        [HideInInspector]
        public byte lastBoxesInRow = 2;
        [HideInInspector]
        public Vector3 lastSize = Vector3.one;
    }

}