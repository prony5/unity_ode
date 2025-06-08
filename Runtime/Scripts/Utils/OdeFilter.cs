namespace UnityODE
{
    public class OdeFilter
    {
        public float tau
        {
            get { return _tau; }
            set
            {
                if (value <= 1 || value == tau)
                    return;
                _tau = value;
                _delta1 = 1 / (_dt * _tau);
                _delta2 = (_delta1 * _delta1);
                _delta1 = _delta1 * _dt;
                _delta2 = _delta2 * _dt;
            }
        }
        private float _tau;

        public double Calc(double value, out double delta, int index = 0)
        {
            var x = value - _lastValues[index];

            _lastValues[index] += _lastDelta[index] * _dt + _delta1 * x;
            _lastDelta[index] += _delta2 * x;

            delta = _lastDelta[index];
            return _lastValues[index];
        }

        public double Calc(double value, int index = 0)
        {
            var x = value - _lastValues[index];

            _lastValues[index] += _lastDelta[index] * _dt + _delta1 * x;
            _lastDelta[index] += _delta2 * x;

            return _lastValues[index];
        }

        private double _dt;
        private double[] _lastValues;
        private double[] _lastDelta;
        private double _delta1, _delta2;

        public OdeFilter(int size, float dt)
        {
            _dt = dt;
            tau = 1;

            _lastValues = new double[size];
            _lastDelta = new double[size];
        }


    }
}
