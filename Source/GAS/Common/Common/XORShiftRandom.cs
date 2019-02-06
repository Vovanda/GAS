
using System.Runtime.CompilerServices;

namespace GAS.Common
{
    public struct XORShiftRandom : IRnd
    {
        public XORShiftRandom(IRnd lcg_rnd)
            : this(lcg_rnd.NextUInt(), lcg_rnd.NextUInt(), lcg_rnd.NextUInt(), lcg_rnd.NextUInt()) { }

        public XORShiftRandom(uint x, uint y, uint z, uint w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double NextDouble()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            return _w * MaxRatio;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float NextFloat()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            return _w * (float)MaxRatio;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint NextUInt()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            return _w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Next()
        {
            uint t = _x ^ (_x << 11);
            _x = _y; _y = _z; _z = _w;
            _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
            int result = (int)_w;
            return (result < 0) ? -result : result;
        }
        
        private uint _x, _y, _z, _w;

        public const double MaxRatio = 1.0 / uint.MaxValue;
    }
}
