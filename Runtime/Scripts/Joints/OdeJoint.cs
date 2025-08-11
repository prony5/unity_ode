using UnityEngine;

namespace UnityODE
{
    [RequireComponent(typeof(OdeBody))]
    public abstract class OdeJoint : MonoBehaviour
    {
        #region Inspector
        [Header("Main properties")]
        [Tooltip("Имя узла. Некоторые скрипты, будут игнорировать узел без имени.")]
        public new string name = null;
        [OdeReadOnlyOnPlay]
        public OdeBody connectedBody;
        [OdeReadOnlyOnPlay]
        public bool connectedBodyCollision = false;
        #endregion

        [System.Serializable]
        public class Limits
        {
            public bool visible = true;

            [Tooltip("Нижняя граница ограничения положения [градус или метр].")]
            public float min;
            [Tooltip("Верхняя граница ограничения положения [градус или метр].")]
            public float max;
            // [Tooltip("Ограничение скорости [градус/сек или метр/сек].")]
            // public float velocity;
            // [Tooltip("Ограничение силы [Ньютон или Ньютон*метр].")]
            // public float force;
            // [Range(0, 1)]
            // public float FudgeFactor = 1;
            // [Range(0, 1)]
            // public float Bounce;

            public Ode.Net.Joints.JointLimitMotor Motor { get; set; }
        }

        public bool Inited { get { return _inited; } }
        public bool HasConnectedBody { get { return _hasConnectedBody; } }
        public bool UseFeedback { get { return _useFeedback; } set { if (!_inited) _useFeedback = value; } }
        public Ode.Net.Joints.Joint Joint { get { return _joint; } }
        public OdeBody Body { get { return _body; } }
        public Ode.Net.Joints.JointFeedback Feedback { get { return _jointFeedback; } }

        protected abstract Ode.Net.Joints.Joint Init();

        private bool _inited = false;
        private bool _hasConnectedBody = false;
        [SerializeField, OdeReadOnlyOnPlay]
        private bool _useFeedback = false;
        private Ode.Net.Joints.Joint _joint;
        private OdeBody _body;
        private Ode.Net.Joints.JointFeedback _jointFeedback;

        protected virtual void BeforeDestroy()
        {
            OdeWorld.Statistics.joints -= 1;
        }

        protected virtual void AfterInit()
        {
            if (connectedBodyCollision == false)
                if (connectedBody != null && OdeBody.CanCollide(Body, connectedBody))
                    Body.collision.ignored.Add(connectedBody);

            OdeWorld.Debug.LogInfo("Joint->Attach [" + gameObject.name + "]");

            _joint.Attach(Body.Body, HasConnectedBody ? connectedBody.Body : null);
            Body.Attach(this);
            if (HasConnectedBody) connectedBody.Attach(this);

            OdeWorld.Statistics.joints += 1;
        }

        void OnEnable()
        {
            OdeWorld.Debug.LogInfo(string.Format("Joint->Enable->Start [{0}]<{1}>", gameObject.name, OdeWorld.Statistics.joints + 1));

            _inited = false;
            _body = GetComponent<OdeBody>();
            _hasConnectedBody = connectedBody != null;

            if (HasConnectedBody)
            {
                if (connectedBody == _body)
                {
                    OdeWorld.Debug.LogError("Joint can't connect the body to itself.");
                    enabled = false;
                    return;
                }
                if (connectedBody.Inited == false)
                {
                    OdeWorld.Debug.LogWarning(string.Format("Joint [{0}] does not created, because connected body [{1}] not inited!", gameObject.name, connectedBody.name));
                    enabled = false;
                    return;
                }
            }

            lock (OdeWorld.Sync)
            {
                _joint = Init();
                _inited = _joint != null;
                if (_inited)
                {
                    if (_useFeedback)
                    {
                        _jointFeedback = new Ode.Net.Joints.JointFeedback();
                        _joint.Feedback = _jointFeedback;
                    }
                    AfterInit();
                }
            }

            OdeWorld.Debug.LogInfo("Joint->Enable->Done [" + gameObject.name + "]");
        }

        void OnDisable()
        {
            OdeWorld.Debug.LogInfo(string.Format("Joint->Disable->Start [{0}]<{1}>", gameObject.name, OdeWorld.Statistics.joints));

            lock (OdeWorld.Sync)
            {
                if (!_inited)
                    return;
                _inited = false;

                BeforeDestroy();

                if (_jointFeedback != null)
                {
                    _jointFeedback.Dispose();
                    _jointFeedback = null;
                }
                _joint.Dispose();
            }

            OdeWorld.Debug.LogInfo("Joint->Disable->Done [" + gameObject.name + "]");
        }
    }
}