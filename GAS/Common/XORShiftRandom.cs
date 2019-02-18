using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAS.Common
{
    public class XORShiftRandom : IUniform
    {
        public XORShiftRandom(IUniform lcg_rnd)
            : this(lcg_rnd.NextUInt(), lcg_rnd.NextUInt(), lcg_rnd.NextUInt(), lcg_rnd.NextUInt()) { }

        public XORShiftRandom(uint x, uint y, uint z, uint w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public double NextDouble()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            return _w * _max_ratio;
        }

        public float NextFloat()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            return _w * (float)_max_ratio;
        }

        public uint NextUInt()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            return _w;
        }

        public int Next()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            int result = (int)_w;
            return (result < 0) ? -result : result;
        }
        
        private uint _x, _y, _z, _w;

        public double MaxRatio => _max_ratio;

        private const double _max_ratio = 1.0 / uint.MaxValue;
    }
}
