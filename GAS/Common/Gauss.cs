using System;

namespace GAS.Common
{
    public class Gauss
    {
        public Gauss(IUniform rnd) : this(rnd, 0.0, 1.0) { }

        public Gauss(IUniform rnd, double mean, double dev)
        {
            _rnd = rnd;
            _mean = mean;
            _dev = dev;
        }

        public double Next()
        {
            if (_readyGetGaussSecondState)
            {
                _readyGetGaussSecondState = false;
                return _gaussSecondState * _dev + _mean;
            }
            else
            {
                double u, v, s;
                do
                {
                    u = 2.0 * _rnd.NextDouble() - 1.0;
                    v = 2.0 * _rnd.NextDouble() - 1.0;
                    s = u * u + v * v;
                } while (s > 1.0 || s == 0.0);

                var r = Math.Sqrt(-2.0 * Math.Log(s) / s);
                _gaussSecondState = r * u;
                _readyGetGaussSecondState = true;
                return r * v * _dev + _mean;
            }
        }

        private IUniform _rnd;
        readonly double _mean;
        readonly double _dev;
        bool _readyGetGaussSecondState = false;
        double _gaussSecondState = 0.0;
    }
}
