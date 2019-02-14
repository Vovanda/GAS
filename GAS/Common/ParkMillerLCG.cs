using System;

namespace GAS.Common
{
    //RNG
    public class ParkMillerLCG : IRandom
    {
        public ParkMillerLCG() : this((uint)Environment.TickCount) { }

        public ParkMillerLCG(uint seed)
        {
            if (seed != _m - 1)
            {
                _state = seed + 1;
            }
            else
            {
                _state = seed;
            }
        }

        public uint NextUInt()
        {
            uint div = _state / (_m / _g);
            uint rem = _state % (_m / _g);
            uint a = rem * _g;
            uint b = div * (_m % _g);
            _state = (a > b) ? (a - b) : (a + (_m - b));

            if (_state == _m)
            {
                _state--;
            };

            return _state;
        }
               
        public double NextDouble()
        {
            uint div = _state / (_m / _g);
            uint rem = _state % (_m / _g);
            uint a = rem * _g;
            uint b = div * (_m % _g);
            _state = (a > b) ? (a - b) : (a + (_m - b));

            if (_state == _m)
            {
                _state--;
            };

            return _state * _max_ratio;
        }

        public int Next()
        {
            uint div = _state / (_m / _g);
            uint rem = _state % (_m / _g);
            uint a = rem * _g;
            uint b = div * (_m % _g);
            _state = (a > b) ? (a - b) : (a + (_m - b));

            if (_state == _m)
            {
                _state--;
            };

            return (int)_state;
        }

        public float NextFloat()
        {
            uint div = _state / (_m / _g);
            uint rem = _state % (_m / _g);
            uint a = rem * _g;
            uint b = div * (_m % _g);
            _state = (a > b) ? (a - b) : (a + (_m - b));

            if (_state == _m)
            {
                _state--;
            };

            return (float)(_state * _max_ratio);
        }

        public double MaxRatio => _max_ratio;

        private uint _state;        
        private const uint _g = 48271;
        private const uint _m = int.MaxValue;
        private const double _max_ratio = 1.0 / int.MaxValue;
        private uint _seed;


    }
}
