using System;
using System.Collections.Generic;
using UnityEngine;
using Ode.Net;
using Ode.Net.Joints;
using Ode.Net.Collision;
using System.Threading;
using System.Diagnostics;

namespace UnityODE
{
    [Serializable]
    public class SpringDamper
    {
        [Tooltip("Stiffness coefficient [N/m]. Characterizes the depth of interpenetration between bodies, depending on the applied force.")]
        public float spring = 0;

        [Tooltip("Damping coefficient [N/(m/s)]. Characterizes the force pushing one body out of another per unit time, depending on the penetration depth.")]
        public float damping = 0;

        public void CalcEprCfm(out float erp, out float cfm)
        {
            float k = OdeWorld.Settings.stepTime * spring + damping;
            if (Mathf.Epsilon > k)
            {
                erp = OdeWorld.Settings.erp;
                cfm = OdeWorld.Settings.cfm;
            }
            else
            {
                erp = (OdeWorld.Settings.stepTime * spring) / k;
                cfm = 1 / k;
            }
        }
    }

    [AddComponentMenu("ODE/World")]
    public class OdeWorld : MonoBehaviour
    {
        public enum OdeSolver { standart, quick }
        public enum OdeDebugType { none, remote, file }

        [Serializable]
        public class WorldStatistics
        {
            public int sps;
            public int bodies;
            public int joints;
            public int geoms;
            public int contacts;
            public float time;
            public float timeScaler;
        }

        #region Inspector
        [System.Serializable]
        public class WorldSettings
        {
            [Tooltip("Manual simulation step. For next step just call Step().")]
            public bool manualStep = false;

            [Range(1, 1000), Tooltip("Number of simulations per second.")]
            public int targetSPS = 500;
            [Range(0.0001f, 0.1f)]
            public float stepTime = 0.002f;
            [Range(1, 100), Tooltip("Simulation step subdivision. Simulation accuracy will improve, but desynchronization between real and virtual time may occur.")]
            public byte stepRate = 1;
            public OdeSolver solver = OdeSolver.quick;
            [Range(5, 100)]
            public int quickIterations = 20;
            [Range(0, 1), Tooltip("Error reduction parameter (ERP). If ERP=0, no corrective force will be applied. If ERP=1, an attempt will be made to correct all joint errors in the next simulation step.")]
            public float erp = 0.2f;
            [Range(0, 0.0001f)]
            public float cfm = 1e-5f;
            public UnityEngine.Vector3 gravity = new UnityEngine.Vector3(0, -9.8f, 0);

            [Header("Inited property")]
            public bool autoDisable = false;
            [Range(0, 100)]
            public int maxContacts = 10;
            public float maxAngularSpeed = 5000;
            public float contactMaxCorrectingVelocity = 1;
        }

        [SerializeField, OdeReadOnlyOnPlay]
        private WorldSettings _worldSettings = new WorldSettings();

        [Header("Debug")]
        public OdeDebugType debugType;
        public bool showContacts = false;
        #endregion

        #region Public
        public static World CurrentWorld { get { return _world; } }
        public static Ode.Net.Collision.Space CurrentSpace { get { return _space; } }
        public static object Sync { get { return _sync; } }

        public static WorldSettings Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new WorldSettings();
                return _settings;
            }
            set
            {
                _settings = value;
                UpdateWorldSettings(false);
            }
        }
        public static WorldStatistics Statistics { get { return _statistics; } }


        private static EventWaitHandle _stepEvent;

        private HighPrecisionTimer _stepTimer;

        public static void Step()
        {
            _stepEvent.Set();
        }

        public static bool Pause
        {
            get { return _pause; }
            set
            {
                if (value != _pause)
                    _debug.LogInfo("World->Pause->Set(" + value.ToString() + ")");
                _pause = value;
            }
        }
        private static bool _pause;

        public static bool Destroyed { get { return _destroyed; } }
        public static bool Inited { get { return _inited; } }

        public static event BeforeStep OnBeforeStep = delegate { };
        public static event AfterStep OnAfterStep = delegate { };

        public delegate void BeforeStep();
        public delegate void AfterStep();

        #endregion

        private static World _world;
        private static WorldSettings _settings;
        private static Ode.Net.Collision.Space _space;

        private static object initSync = new object();
        private static object _sync = new object();

        private static bool _inited = false;
        private static bool _destroyed = false;

        private static WorldStatistics _statistics;
        private static EventWaitHandle _eventTerminated;
        private static JointGroup _contactGroup;
        private static ContactGeom[] contacts;

        // private double stepScaler = 0;

        #region Debug
        public static OdeDebug Debug { get { return _debug; } }
        private static OdeDebug _debug = null;

        private object syncDebug = new object();

        private struct DebugContact
        {
            public ContactGeom contact;
            public DateTime ts;

            public DebugContact(ContactGeom value)
            {
                contact = value;
                ts = DateTime.Now;
            }

            public DebugContact(ContactGeom value, DateTime time)
            {
                contact = value;
                ts = time;
            }
        }
        private List<DebugContact> debugContacts = new List<DebugContact>();
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        void Awake()
        {
            lock (initSync)
            {
                if (_debug == null)
                {
                    switch (debugType)
                    {
                        case OdeDebugType.remote:
                            _debug = new OdeDebugRemote();
                            break;

                        case OdeDebugType.file:
                            _debug = new OdeDebugFile();
                            break;

                        default:
                            _debug = new OdeDebugNone();
                            break;
                    }
                }

                Debug.LogInfo(string.Format("World->Awake->Start. Inited({0})", _inited));

                if (_world == null)
                    _inited = false;
                else
                {
                    Debug.LogError("World->Awake->There should be only one ODE world on the scene.");
                    Destroy(this);
                    return;
                }

                Engine.Init();
                _world = new World();
                _space = new HashSpace();
                _contactGroup = new JointGroup();
                _statistics = new WorldStatistics();
                _settings = _worldSettings;
                UpdateWorldSettings(true);

                _eventTerminated = new EventWaitHandle(false, EventResetMode.AutoReset);
                _stepEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

                _inited = true;
                _destroyed = false;
                Pause = false;

                _stepTimer = new HighPrecisionTimer(
                    TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond * _settings.stepTime)),
                    () => { if (!_settings.manualStep) Step(); }
                );

                Debug.LogInfo("World->Awake->Done");
            }
        }

        private static void UpdateWorldSettings(bool init)
        {
            lock (_sync)
            {
                _world.QuickStepNumIterations = _settings.quickIterations;
                _world.Erp = _settings.erp;
                _world.Cfm = _settings.cfm;
                _world.Gravity = _settings.gravity.ToODE();

                if (init)
                {
                    contacts = new ContactGeom[_settings.maxContacts];
                    _world.AutoDisable = _settings.autoDisable;

                    // _world.StepIslandsProcessingMaxThreadCount = 20;
                    /*
                          // _world.AutoDisableAverageSamplesCount = 10;
                    */
                    _world.LinearDamping = 0.001f;
                     _world.AngularDamping = 0.05f;
                    
                    _world.MaxAngularSpeed = _settings.maxAngularSpeed * Mathf.Deg2Rad;
                    _world.ContactMaxCorrectingVelocity = _settings.contactMaxCorrectingVelocity;
                    _world.ContactSurfaceLayer = 0.001f;
                }
            }
        }

#if UNITY_EDITOR
        void OnEnable()
        {
            UnityEditor.EditorApplication.pauseStateChanged += PauseStateChanged;
        }

        void PauseStateChanged(UnityEditor.PauseState state)
        {
            Pause = state == UnityEditor.PauseState.Paused;
        }

        void OnDisable()
        {
            UnityEditor.EditorApplication.pauseStateChanged -= PauseStateChanged;
        }
#endif

        void Start()
        {
            new Thread(Simulation).Start();
        }

        void Update()
        {
            if (showContacts)
            {
                lock (syncDebug)
                {
                    foreach (var item in debugContacts)
                        UnityEngine.Debug.DrawRay(item.contact.Position.ToUnity(), item.contact.Normal.ToUnity(), Color.cyan, Time.deltaTime, false);
                }
            }
        }

        void OnApplicationQuit()
        {
            Pause = true;
        }

        void OnDestroy()
        {
            Pause = true;
            lock (initSync)
            {
                Debug.LogInfo(string.Format("World->Destroy->Start. Inited({0})", _inited));

                if (_inited == false)
                    return;

                _inited = false;
                _stepEvent.Set();
                try
                {
                    _stepTimer.Dispose();
                    if (!_eventTerminated.WaitOne(10000))
                        Debug.LogWarning("World->Destroy: Terminated by timeout");
                }
                finally
                {
                    Debug.LogInfo("Destroy->DisableMonoBehaviour");

                    foreach (var script in FindObjectsOfType<OdeJoint>())
                        script.enabled = false;
                    foreach (var script in FindObjectsOfType<OdeBody>())
                        script.enabled = false;

                    Debug.LogInfo("World->Destroy->Space");
                    _space.Dispose();
                    Debug.LogInfo("World->Destroy->World");
                    _world.Dispose();
                    Debug.LogInfo("World->Destroy->CleanUp");
                    Engine.CleanupAllDataForThread();
                    Debug.LogInfo("World->Destroy->Close");
                    Engine.Close();

                    _world = null;
                }
                _destroyed = true;

                Debug.LogInfo("World->Destroy->Done");
            }

            if (_debug != null)
            {
                _debug.Dispose();
                _debug = null;
            }
        }

        void Simulation()
        {
            try
            {
                Debug.LogInfo("World->Simulation->Start");
                try
                {
                    _statistics.time = 0;
                    var spsCounter = 0;
                    var spsWatch = new Stopwatch();
                    spsWatch.Start();

                    TimeSpan lastElapsed = new TimeSpan();
                    var stepWatch = new Stopwatch();
                    stepWatch.Start();
                    while (_inited)
                    {
                        if (spsWatch.ElapsedMilliseconds >= 1000)
                        {
                            _statistics.sps = spsCounter;
                            spsCounter = 0;
                            spsWatch.Reset();
                            spsWatch.Start();
                        }

                        if (Pause)
                        {
                            Thread.Sleep(1);
                            lastElapsed = stepWatch.Elapsed;
                            continue;
                        }

                        if (!_stepEvent.WaitOne(1))
                            continue;

                        float realStepTime = 1 / (float)_settings.targetSPS;
                        int stepDelay = Mathf.FloorToInt(1000 / _settings.targetSPS) + 1;
                        float stepScalerMax = realStepTime * 100;
                        _statistics.timeScaler = realStepTime / _settings.stepTime;

                        lock (_sync)
                        {
                            OnBeforeStep();

                            _statistics.contacts = 0;
                            _space.Collide(NearCallback);

                            float rateScaler = 1f / _settings.stepRate;
                            for (int i = 0; i < _settings.stepRate; i++)
                            {
                                if (_settings.solver == OdeSolver.standart)
                                    _world.Step(_settings.stepTime * rateScaler);
                                else
                                    _world.QuickStep(_settings.stepTime * rateScaler);

                                spsCounter++;
                            }

                            _statistics.time += _settings.stepTime;

                            OnAfterStep();
                            _contactGroup.Clear();
                        }

                        lastElapsed = stepWatch.Elapsed;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(string.Format("World->Simulation: \"{0}\"", ex.Message));
                }
            }
            finally
            {
                _eventTerminated.Set();
                _statistics.sps = 0;
                Debug.LogInfo("World->Simulation->Done");
            }
        }

        void NearCallback(Geom geom1, Geom geom2)
        {
            // Get the dynamics body for each geom
            int collide = 0;
            var b1 = geom1.Body;
            var b2 = geom2.Body;

            if (b1 != null && b2 != null && (b1.Kinematic == false || b2.Kinematic == false))
            {
                var ob1 = b1.Tag as OdeBody;
                var ob2 = b2.Tag as OdeBody;

                //Check ignore
                if (ob1.collision.active && ob2.collision.active && OdeBody.CanCollide(ob1, ob2))
                {
                    var ci = new ContactInfo();
                    collide = Geom.Collide(geom1, geom2, contacts);
                    for (int i = 0; i < collide; i++)
                    {
                        _statistics.contacts += 1;

                        ci.Geometry = contacts[i];

                        var mu = Math.Min(ob1.friction.mu, ob2.friction.mu);
                        var bounce = Math.Max(ob1.friction.bounce, ob2.friction.bounce);
                        var softCfm = Math.Max(ob1.friction.CFM, ob2.friction.CFM);
                        var softErp = Math.Max(ob1.friction.ERP, ob2.friction.ERP);

                        ci.Surface.Mode = (bounce > 0 ? ContactModes.Bounce : 0) | (softCfm > 0 ? ContactModes.SoftCfm : 0) | (softErp > 0 ? ContactModes.SoftErp : 0);
                        ci.Surface.Mu = mu;
                        ci.Surface.Bounce = bounce;
                        ci.Surface.SoftCfm = softCfm;
                        ci.Surface.SoftErp = softErp;

                        var c = new Contact(_world, ci, _contactGroup);
                        c.Attach(b1, b2);
                    }
                }
            }

            if (showContacts)
            {
                lock (syncDebug)
                {
                    var time = DateTime.Now;
                    int i = 0;
                    while (i < debugContacts.Count)
                    {
                        if ((int)((TimeSpan)(time - debugContacts[i].ts)).Milliseconds >= _settings.stepTime * 1000)
                            debugContacts.RemoveRange(i, 1);
                        else
                            i++;
                    }

                    for (i = 0; i < collide; i++)
                        debugContacts.Add(new DebugContact(contacts[i], time));
                }
            }
        }


    }
}