using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Utils/CenterOfMass")]
    public class OdeCenterOfMass : MonoBehaviour
    {
        public float total { get { return _massTotal; } }
        public Vector3 position { get { return _position; } }
        public bool showCOM = false;
        public Material materialTransparent;
        public Material materialCom;

        [SerializeField]
        private float _massTotal;
        [SerializeField]
        private Vector3 _position;
        private List<OdeBody> _bodies = new List<OdeBody>();
        private bool _inited;

        private bool lastShowCOM = false;
        private class ObjectCOM
        {
            public GameObject gameObject;
            public OdeBody body;
            public MeshRenderer meshRender;
            public Material[] defMaterials;

            public ObjectCOM(OdeBody aBody, GameObject aGameObject)
            {
                body = aBody;
                gameObject = aGameObject;
                gameObject.transform.position = body.Position;

                meshRender = aBody.gameObject.GetComponent<MeshRenderer>();
                if (meshRender != null)
                    defMaterials = meshRender.materials;
            }

            public static GameObject CreateGameObject(float size, Material material, string name = "COM")
            {
                var gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gameObject.name = name;
                gameObject.transform.localScale *= size;
                gameObject.GetComponent<MeshRenderer>().materials = new Material[] { material };
                gameObject.SetActive(false);
                return gameObject;
            }
        }

        private GameObject mainCOM = null;
        private List<ObjectCOM> objectsCOM = new List<ObjectCOM>();

        public void SetCOM(float mass, Vector3 pos)
        {
            _massTotal = mass;
            _position = pos;
        }

        void OnDisable()
        {
            if (_inited)
            {
                _inited = false;
                OdeWorld.OnBeforeStep -= BeforeSimStep;

                SetTransparent(false);

                Destroy(mainCOM);
                foreach (var item in objectsCOM)
                    Destroy(item.gameObject);

                objectsCOM.Clear();
            }
        }

        void Start()
        {
            _bodies.Clear();

            _massTotal = 0;
            foreach (var b in GetComponentsInChildren<OdeBody>())
                if (b.enabled)
                {
                    _massTotal += b.inertial.mass;
                    _bodies.Add(b);
                }

            if (_massTotal <= 0 || _bodies.Count == 0)
                return;

            mainCOM = ObjectCOM.CreateGameObject(0.05f, materialCom);
            foreach (var item in _bodies)
            {
                var obj = new ObjectCOM(item, ObjectCOM.CreateGameObject(0.015f, materialCom, "COM " + item.name));
                obj.gameObject.transform.parent = mainCOM.transform;
                objectsCOM.Add(obj);
            }

            OdeWorld.OnBeforeStep += BeforeSimStep;
            _inited = true;
        }

        void BeforeSimStep()
        {
            Vector3 com = new Vector3();
            foreach (var b in _bodies)
                com += b.Position * (b.inertial.mass / _massTotal);
            _position = com;
        }

        void Update()
        {
            if (_inited == false)
                return;

            //change visible
            if (lastShowCOM != showCOM)
            {
                lastShowCOM = showCOM;

                mainCOM.SetActive(showCOM);
                foreach (var item in objectsCOM)
                    item.gameObject.SetActive(showCOM);

                SetTransparent(showCOM);
            }

            if (showCOM)
            {
                mainCOM.transform.position = _position;
                foreach (var item in objectsCOM)
                    item.gameObject.transform.position = item.body.Position;
            }
        }


        void SetTransparent(bool value)
        {
            foreach (var item in objectsCOM)
            {
                if (item.meshRender != null)
                {
                    if (value)
                        item.meshRender.materials = new Material[] { materialTransparent };
                    else
                        item.meshRender.materials = item.defMaterials;
                }
            }
        }

    }
}
