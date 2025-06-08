using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Body")]
    public class OdeBody : MonoBehaviour
    {
        #region Inspector
        public Transform container;
        public bool isKinematic = true;
        public bool useGravity = true;

        [System.Serializable]
        public class Inertial
        {
            [Tooltip("Масса объекта (килограмм), должна быть больше 0.")]
            public float mass = 1;
            [Tooltip("Центр тяжести (метр), относительно трансформации, в локальной системе координат.")]
            public Vector3 com;
            [Tooltip("Моменты инерции, определяются в центре тяжести и выравниваются относительно локальной системы координат.")]
            public Vector3 inertia;
        }
        [OdeReadOnlyOnPlay]
        public Inertial inertial = new Inertial();

        [System.Serializable]
        public class Collision
        {
            public bool active = true;
            [Tooltip("Дополнительный список объектов поиска пересечений. Коллайдеры текущего игрового объекта добавлять не нужно.")]
            public List<Collider> colliders = new List<Collider>();
            [Tooltip("Список объектов, пересечения с которыми учитываться не будут.")]
            public List<OdeBody> ignored = new List<OdeBody>();
        }
        public Collision collision = new Collision();

        [System.Serializable]
        public class Friction
        {
            [Range(0, float.PositiveInfinity), Tooltip("Коэффициент трения. 0 - трение отсутсвует.")]
            public float mu = float.PositiveInfinity;
            [Range(0, 1), Tooltip("Параметр упругости. 0 - поверхность не упруга, 1 - максимальная упругость.")]
            public float bounce = 0.2f;
            public SpringDamper springDamper = new SpringDamper();

            /// <summary>
            /// Параметр уменьшения ошибки нормали контакта, нужен чтобы сделать поверхность мягче
            /// </summary>
            [OdeReadOnly]
            public float ERP;

            /// <summary>
            /// Параметр смешивающей силы соединения нормали контакта, нужен чтобы сделать поверхность мягче.
            /// </summary>
            [OdeReadOnly]
            public float CFM;
        }
        public Friction friction = new Friction();
        #endregion

        public Ode.Net.Body Body { get { return _body; } }
        public bool Inited { get { return _inited; } }
        public Vector3 Position { get { return _position; } }
        public Quaternion Rotation { get { return _rotation; } }

        /// <summary>
        /// Угловая скорость тела в глобальной СК [рад/сек].
        /// </summary>
        public Vector3 VelocityAngular { get { return _velocityAngular; } }

        /// <summary>
        /// Линейная скорость тела в глобальной СК [метр/сек].
        /// </summary>
        public Vector3 VelocityLinear { get { return _velocityLinear; } }

        public Vector3 Acceleration { get; private set; }

        private Ode.Net.Body _body;
        private List<Ode.Net.Collision.Geom> _geoms = new List<Ode.Net.Collision.Geom>();
        private bool _inited = false;

        private Vector3 _position;
        private Quaternion _rotation = Quaternion.identity;
        private Vector3 _velocityAngular;
        private Vector3 _velocityLinear;
        private Ode.Net.Vector3 _lastVelocityLinear;

        private List<OdeJoint> _attached = new List<OdeJoint>();
        private object _attachSync = new object();

        public void Attach(OdeJoint joint)
        {
            if (_inited == false)
            {
                OdeWorld.Debug.LogError("Body->Attach: \"Body not inited, attaching imposable.\" [" + name + "]");
                return;
            }

            lock (_attachSync)
            {
                _attached.Add(joint);
            }
        }

        void OnEnable()
        {
            _inited = false;
            if (OdeWorld.CurrentWorld == null)
            {
                enabled = false;
                return;
            }

            OdeWorld.Debug.LogInfo(string.Format("Body->Enable->Start [{0}]<{1}>", gameObject.name, OdeWorld.Statistics.bodies + 1));

            if (inertial.mass <= 0)
            {
                OdeWorld.Debug.LogError(string.Format("Body->Enable \"Body mass should not be empty or negative.\" [{0}]", gameObject.name));
            }

            if (container != null && container.GetComponent<OdeBody>() != null)
            {
                OdeWorld.Debug.LogError(string.Format("Body->Enable \"The body container should not have OdeBody.\" [{0}]", gameObject.name));
                enabled = false;
                return;
            }

            transform.parent = container;
            _rotation = transform.rotation;
            _position = transform.position + transform.rotation * inertial.com;

            if (isActiveAndEnabled == false)
                return;

            lock (OdeWorld.Sync)
            {
                _body = new Ode.Net.Body(OdeWorld.CurrentWorld);
                _body.Tag = this;

                //set geometry
                var isFirstGeom = true;
                var mass = _body.Mass;
                mass.TotalMass = inertial.mass;

                //read gameObject colliders
                foreach (var item in GetComponents<Collider>())
                    GetGeom(item, ref mass, ref isFirstGeom);
                //read colliders of list
                foreach (var item in collision.colliders)
                    GetGeom(item, ref mass, ref isFirstGeom);

                //SetCOM
                foreach (var item in _geoms)
                    item.OffsetPosition += -inertial.com.ToODE();

                //set mass property 
                if (inertial.inertia.x * inertial.inertia.y * inertial.inertia.z == 0)
                {
#if ODE_DOUBLE_PRECISION
                    if (double.IsNaN(mass.Inertia.Row1.X) || double.IsNaN(mass.Inertia.Row2.Y) || double.IsNaN(mass.Inertia.Row3.Z))
                        mass.Inertia = new Ode.Net.Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);
                    inertial.inertia = new Vector3((float)mass.Inertia.Row1.X, (float)mass.Inertia.Row2.Y, (float)mass.Inertia.Row3.Z);
#else
                    if (float.IsNaN(mass.Inertia.Row1.X) || float.IsNaN(mass.Inertia.Row2.Y) || float.IsNaN(mass.Inertia.Row3.Z))
                        mass.Inertia = new Ode.Net.Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);
                    inertial.inertia = new Vector3(mass.Inertia.Row1.X, mass.Inertia.Row2.Y, mass.Inertia.Row3.Z);
#endif
                }
                else
                    mass.Inertia = new Ode.Net.Matrix3(inertial.inertia.x, 0, 0, 0, inertial.inertia.y, 0, 0, 0, inertial.inertia.z);
                _body.Mass = mass;

                _body.Quaternion = _rotation.ToODE();
                _body.Position = _position.ToODE();

                OdeWorld.OnBeforeStep += BeforeSimStep;
                OdeWorld.OnAfterStep += AfterSimStep;

                _inited = true;

                //Update global statistics
                OdeWorld.Statistics.bodies += 1;
                OdeWorld.Statistics.geoms += _geoms.Count;
            }

            OdeWorld.Debug.LogInfo("Body->Enable->Done [" + gameObject.name + "]");
        }

        void OnDisable()
        {
            if (_inited == false)
                return;
            _inited = false;

            OdeWorld.Debug.LogInfo(string.Format("Body->Disable->Start [{0}][{1}]", gameObject.name, OdeWorld.Statistics.bodies));

            lock (OdeWorld.Sync)
            {
                lock (_attachSync)
                {
                    foreach (var item in _attached)
                        if (item) item.enabled = false;
                }

                //Update global statistics
                OdeWorld.Statistics.bodies -= 1;
                OdeWorld.Statistics.geoms -= _geoms.Count;

                OdeWorld.OnBeforeStep -= BeforeSimStep;
                OdeWorld.OnAfterStep -= AfterSimStep;

                foreach (var item in _geoms)
                    item.Dispose();
                _geoms.Clear();

                _body.Dispose();
            }

            OdeWorld.Debug.LogInfo("Body->Disable->Done [" + gameObject.name + "]");
        }

        void BeforeSimStep()
        {
            friction.springDamper.CalcEprCfm(out friction.ERP, out friction.CFM);

            if (_body.Kinematic != isKinematic)
            {
                _body.Kinematic = isKinematic;
                _body.Force = Vector3.zero.ToODE();
                _body.Torque = Vector3.zero.ToODE();
                _body.AngularVelocity = Vector3.zero.ToODE();
                _body.LinearVelocity = Vector3.zero.ToODE();
            }

            if (_body.GravityMode != useGravity)
                _body.GravityMode = useGravity;

            _lastVelocityLinear = _body.LinearVelocity;
        }

        void AfterSimStep()
        {
            if (isKinematic)
            {
                _body.Quaternion = _rotation.ToODE();
                _body.Position = _position.ToODE();
            }
            else
            {
                _rotation = _body.Quaternion.ToUnity();
                var newPos = _body.Position.ToUnity();
                if ((float.IsNaN(newPos.x) || float.IsNaN(newPos.y) || float.IsNaN(newPos.z)) == false)
                    _position = newPos;
            }

            _velocityAngular = _body.AngularVelocity.ToUnity();
            _velocityLinear = _body.LinearVelocity.ToUnity();
            Acceleration = (_body.LinearVelocity - _lastVelocityLinear).ToUnity() / OdeWorld.Settings.stepTime;
        }

        void Update()
        {
            if (isKinematic)
            {
                _rotation = transform.rotation;
                _position = transform.position + transform.rotation * inertial.com;
            }
            else
            {
                transform.rotation = _rotation;
                transform.position = _position - _rotation * inertial.com;
            }
        }

        public static bool CanCollide(OdeBody b1, OdeBody b2)
        {
            if (b1 == null || b2 == null || b1 == b2)
                return false;

            for (int i = 0; i < b1.collision.ignored.Count; i++)
            {
                if (b1.collision.ignored[i] == b2)
                    return false;
            }
            for (int i = 0; i < b2.collision.ignored.Count; i++)
            {
                if (b2.collision.ignored[i] == b1)
                    return false;
            }

            return true;
        }

        private void SetGeomOffset(Ode.Net.Collision.Geom geom, Collider collider, Vector3 center)
        {
            var scale = collider.transform.lossyScale;

            //Цетр контактной геометрии зависит от абсолютного скалера
            //Вращение контактной геометрии не учитывается
            if (collider.gameObject == gameObject)
                geom.OffsetPosition = Vector3.Scale(scale, center).ToODE();
            else
            {
                geom.OffsetQuaternion = (Quaternion.Inverse(transform.rotation) * collider.transform.rotation).ToODE();
                geom.OffsetPosition =
                  (
                         (Quaternion.Inverse(transform.rotation) * (collider.transform.position - transform.position))
                         +
                          OdeUtils.RotateAroundPivot(Vector3.Scale(scale, center), Vector3.zero, (collider.transform.localRotation))
                    ).ToODE();
            }
        }

        private bool GetGeom(Collider collider, ref Ode.Net.Mass mass, ref bool isFirstGeom)
        {
            if (collider == null || collider.gameObject.activeInHierarchy == false || collider.enabled == false)
                return false;

            var space = OdeWorld.CurrentSpace;
            var scale = collider.transform.lossyScale;

            if (collider.GetType() == typeof(BoxCollider))
            {
                var u = collider as BoxCollider;
                var sz = Vector3.Scale(scale, u.size);
                var geom = new Ode.Net.Collision.Box(space, sz.x, sz.y, sz.z);

                //attach to body
                geom.Body = _body;

                // Set offset
                SetGeomOffset(geom, collider, u.center);

                //add geom in list
                _geoms.Add(geom);

                if (isFirstGeom)
                    Ode.Net.Mass.CreateBoxTotal(inertial.mass, sz.x, sz.y, sz.z, out mass);

                isFirstGeom = false;
                return true;
            }

            if (collider.GetType() == typeof(SphereCollider))
            {
                var u = collider as SphereCollider;
                var sz = Mathf.Min(new float[] { scale.x, scale.y, scale.z });
                var geom = new Ode.Net.Collision.Sphere(space, u.radius * sz);

                //attach to body
                geom.Body = _body;

                // Set offset
                SetGeomOffset(geom, collider, u.center);

                //add geom in list
                _geoms.Add(geom);

                if (isFirstGeom)
                    Ode.Net.Mass.CreateSphereTotal(inertial.mass, u.radius * sz, out mass);

                isFirstGeom = false;
                return true;
            }

            if (collider.GetType() == typeof(CapsuleCollider))
            {
                var u = collider as CapsuleCollider;
                var sz = Mathf.Min(new float[] { scale.x, scale.y, scale.z });
                var radius = u.radius * sz;
                var height = u.height * sz - radius * 2;
                if (height <= 0) height = radius;
                var geom = new Ode.Net.Collision.Capsule(space, radius, height);

                //attach to body
                geom.Body = _body;

                // Set offset
                SetGeomOffset(geom, collider, u.center);
                switch (u.direction)
                {
                    case 0:
                        geom.OffsetQuaternion = (geom.OffsetQuaternion.ToUnity() * Quaternion.Euler(0, 90, 0)).ToODE();
                        break;
                    case 1:
                        geom.OffsetQuaternion = (geom.OffsetQuaternion.ToUnity() * Quaternion.Euler(90, 0, 0)).ToODE();
                        break;
                }

                //add geom in list
                _geoms.Add(geom);

                if (isFirstGeom)
                    Ode.Net.Mass.CreateCapsuleTotal(inertial.mass, (Ode.Net.DirectionAxis)u.direction + 1, radius, height, out mass);

                isFirstGeom = false;
                return true;
            }

            if (collider.GetType() == typeof(TerrainCollider))
            {
                var u = collider as TerrainCollider;
                var terrainData = u.terrainData;
                int terrainWidth = terrainData.heightmapResolution;
                int terrainHeight = terrainData.heightmapResolution;
                Vector3 terrainScale = new Vector3(terrainData.size.x / (terrainWidth - 1), terrainData.size.y, terrainData.size.z / (terrainHeight - 1));

                float[,] terrainHeights = terrainData.GetHeights(0, 0, terrainWidth, terrainHeight);
                Vector3[] v = new Vector3[terrainWidth * terrainHeight];
                int[] verticesIndex = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];

                for (int y = 0; y < terrainHeight; y++)
                {
                    for (int x = 0; x < terrainWidth; x++)
                    {
                        v[y * terrainWidth + x] = Vector3.Scale(terrainScale, new Vector3(y, terrainHeights[x, y], x));
                    }
                }

                int index = 0;
                for (int y = 0; y < terrainHeight - 1; y++)
                {
                    for (int x = 0; x < terrainWidth - 1; x++)
                    {
                        verticesIndex[index++] = (y * terrainWidth) + x;
                        verticesIndex[index++] = (y * terrainWidth) + x + 1;
                        verticesIndex[index++] = ((y + 1) * terrainWidth) + x;

                        verticesIndex[index++] = ((y + 1) * terrainWidth) + x;
                        verticesIndex[index++] = (y * terrainWidth) + x + 1;
                        verticesIndex[index++] = ((y + 1) * terrainWidth) + x + 1;
                    }
                }
                float[] points = new float[3 * v.Length];

                var j = 0;
                for (int i = 0; i < v.Length; i++)
                {
                    var value = v[i];
                    points[j] = value.x;
                    points[j + 1] = value.y;
                    points[j + 2] = value.z;
                    j = j + 3;
                }

                var data = new Ode.Net.Collision.TriMeshData();
                var ids = verticesIndex;
                var indices = new uint[ids == null ? 0 : ids.Length];
                for (int i = 0; i < indices.Length; i++)
                    indices[i] = (uint)ids[i];

                data.BuildSingle(points, indices);
                var geom = new Ode.Net.Collision.TriMesh(space, data);

                //attach to body
                geom.Body = _body;

                //add geom in list
                _geoms.Add(geom);

                if (isFirstGeom)
                    Ode.Net.Mass.CreateTriMeshTotal(inertial.mass, geom as Ode.Net.Collision.TriMesh, out mass);
                isFirstGeom = false;

                // Add tree colliders
                for (int k = 0; k < terrainData.treeInstanceCount; k++)
                {
                    TreeInstance tree = terrainData.GetTreeInstance(k);
                    TreePrototype prototype = (TreePrototype)terrainData.treePrototypes.GetValue(tree.prototypeIndex);

                    foreach (var item in prototype.prefab.GetComponents<Collider>())
                    {
                        if (!item.enabled)
                            continue;

                        if (item.GetType() == typeof(BoxCollider))
                        {
                            var uTree = item as BoxCollider;
                            var centerTree = Vector3.Scale(tree.position, terrainData.size) + transform.position;
                            var uTreeSize = Vector3.Scale(uTree.size, uTree.transform.localScale);
                            var treeScaler = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                            var treeInstanceSize = Vector3.Scale(uTreeSize, treeScaler);
                            var sz = Vector3.Scale(scale, treeInstanceSize);

                            //Create Geom
                            var treeGeom = new Ode.Net.Collision.Box(space, sz.x, sz.y, sz.z);

                            //attach to body
                            treeGeom.Body = _body;

                            var fullCenterScale = Vector3.Scale(uTree.center, Vector3.Scale(uTree.transform.localScale, treeScaler));

                            // Set offset
                            treeGeom.OffsetPosition = (centerTree + fullCenterScale).ToODE();

                            //add geom in list
                            _geoms.Add(treeGeom);
                        }
                        else if (item.GetType() == typeof(SphereCollider))
                        {
                            var uTree = item as SphereCollider;
                            var centerTree = Vector3.Scale(tree.position, terrainData.size) + transform.position;

                            var uTreeSize = uTree.radius * uTree.transform.localScale;
                            var treeScaler = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                            var treeInstanceSize = Vector3.Scale(uTreeSize, treeScaler);
                            var sz = Mathf.Max(new float[] { treeInstanceSize.x, treeInstanceSize.y, treeInstanceSize.z });
                            var treeGeom = new Ode.Net.Collision.Sphere(space, sz);
                            var fullCenterScale = Vector3.Scale(uTree.center, Vector3.Scale(uTree.transform.localScale, treeScaler));

                            treeGeom.Body = _body;
                            treeGeom.OffsetPosition = (centerTree + fullCenterScale).ToODE();
                            _geoms.Add(treeGeom);
                        }
                        else if (item.GetType() == typeof(CapsuleCollider))
                        {
                            var uTree = item as CapsuleCollider;
                            var centerTree = Vector3.Scale(tree.position, terrainData.size) + transform.position;
                            var uTreeSize = new Vector3(uTree.transform.localScale.x * uTree.radius, uTree.transform.localScale.y * uTree.height, uTree.transform.localScale.z * uTree.radius);
                            var treeScaler = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                            var treeInstanceSize = Vector3.Scale(uTreeSize, treeScaler);
                            var radius = treeInstanceSize.x;
                            var height = treeInstanceSize.y - radius * 2;
                            var treeGeom = new Ode.Net.Collision.Capsule(space, radius, height);
                            var fullCenterScale = Vector3.Scale(uTree.center, Vector3.Scale(uTree.transform.localScale, treeScaler));
                            treeGeom.Body = _body;

                            treeGeom.OffsetPosition = (centerTree + fullCenterScale).ToODE();
                            switch (uTree.direction)
                            {
                                case 0:
                                    treeGeom.OffsetQuaternion = (treeGeom.OffsetQuaternion.ToUnity() * Quaternion.Euler(0, 90, 0)).ToODE();
                                    break;
                                case 1:
                                    treeGeom.OffsetQuaternion = (treeGeom.OffsetQuaternion.ToUnity() * Quaternion.Euler(90, 0, 0)).ToODE();
                                    break;
                            }
                            _geoms.Add(treeGeom);
                        }
                        else OdeWorld.Debug.LogWarning("Not supported tree collider type.");
                    }
                }

                return true;
            }

            if (collider.GetType() == typeof(MeshCollider))
            {
                var u = collider as MeshCollider;
                var mesh = u.sharedMesh;

                if (mesh)
                {
                    //get points
                    var v = mesh.vertices;
#if ODE_DOUBLE_PRECISION
                    double[] points = new double[3 * v.Length];
#else
                    float[] points = new float[3 * v.Length];
#endif
                    var j = 0;
                    if (u.gameObject == gameObject)
                    {
                        for (int i = 0; i < v.Length; i++)
                        {
                            var value = Vector3.Scale(scale, v[i]);
                            points[j] = value.x;
                            points[j + 1] = value.y;
                            points[j + 2] = value.z;
                            j = j + 3;
                        }
                    }
                    else
                    {
                        var ofs = Quaternion.Inverse(transform.rotation) * (u.transform.position - transform.position);
                        var ofsRot = Quaternion.Inverse(transform.rotation) * u.transform.rotation;
                        for (int i = 0; i < v.Length; i++)
                        {
                            var value = Vector3.Scale(scale, ofsRot * v[i]) + ofs;
                            points[j] = value.x;
                            points[j + 1] = value.y;
                            points[j + 2] = value.z;
                            j = j + 3;
                        }
                    }

                    var data = new Ode.Net.Collision.TriMeshData();
                    var ids = mesh.GetIndices(0);
                    var indices = new uint[ids == null ? 0 : ids.Length];
                    for (int i = 0; i < indices.Length; i++)
                        indices[i] = (uint)ids[i];
#if ODE_DOUBLE_PRECISION
                    data.BuildDouble(points, indices);
#else
                    data.BuildSingle(points, indices);
#endif
                    var geom = new Ode.Net.Collision.TriMesh(space, data);

                    //attach to body
                    geom.Body = _body;

                    //add geom in list
                    _geoms.Add(geom);

                    if (isFirstGeom)
                        Ode.Net.Mass.CreateTriMeshTotal(inertial.mass, geom as Ode.Net.Collision.TriMesh, out mass);
                    isFirstGeom = false;
                }
                return true;
            }

            return false;
        }

    }
}