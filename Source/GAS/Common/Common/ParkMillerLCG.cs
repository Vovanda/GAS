using System.Runtime.CompilerServices;

namespace GAS.Common
{
    public struct ParkMillerLCG : IRnd
    {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            return _state * MaxRatio;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            return _state * (float)MaxRatio;
        }
               
        public const double MaxRatio = 1.0 / int.MaxValue;

        private uint _state;
        private const uint _g = 48271;
        private const uint _m = int.MaxValue;
    }
}
