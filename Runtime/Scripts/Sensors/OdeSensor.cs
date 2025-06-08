using UnityEngine;

namespace UnityODE
{
    public abstract class OdeSensor : MonoBehaviour
    {
        [Tooltip("Имя сенсора. Некоторые скрипты, будут игнорировать узел без имени.")]
        new public string name = null;
        public float[] values { get { return _values; } }
        public string[] valuesID { get { return _valuesID; } }

        protected float[] _values = new float[0];
        protected string[] _valuesID = new string[0];

        public string ToString(string separator = " ")
        {
            var retVal = "";
            foreach (var item in values)
                retVal += item.ToString() + separator;
            var i = retVal.LastIndexOf(separator);
            if (i >= 0)
                retVal = retVal.Remove(i);

            return retVal;
        }

        public string ToStringID(string separator = " ")
        {
            var retVal = "";
            foreach (var item in valuesID)
                retVal += item + separator;
            var i = retVal.LastIndexOf(separator);
            if (i >= 0)
                retVal = retVal.Remove(i);

            return retVal;
        }
    }
}
