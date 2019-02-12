using System;

namespace GAS.Common
{
    //RNG
    public class ParkMillerLCG
    {
        public ParkMillerLCG() :this(Environment.TickCount){}

        public ParkMillerLCG(int seed)
        {
            if (seed == _m)
            {
                _state = (uint)(seed - 1);
            }
            else if (seed < 0)
            {
                _state = (uint)(seed + _m);
            }
            else
            {
                _state = (uint)seed;
            }
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
            return Next() * _im;
        }

        public int Next(int a, int b)
        {
            return a + (int)((b - a) * NextFloat());
        }

        public float NextFloat(float a, float b)
        {
            return a + (b - a) * NextFloat();
        }
        
        public int NextBoolInt()
        {
            return Next() % 2;
        }

        public bool NextBool()
        {
            return (NextBoolInt() == 0) ? false : true;
        }

        private uint _state;        
        private const uint _g = 48271;
        private const uint _m = int.MaxValue;
        private const float _im = 1.0f/ int.MaxValue;
    }
}
