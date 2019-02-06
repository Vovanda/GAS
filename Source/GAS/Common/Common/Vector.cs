using System;
using System.Runtime.CompilerServices;

namespace GAS.Common
{
    public struct Vector
    {
        #region Public Static Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator +(Vector vector)
        {
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator -(Vector vector)
        {
            Vector new_vector = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = -vector[i];
            }
            return new_vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator +(Vector first, Vector second)
        {
            for (int i=0; i< first.Length; i++)
            {
                first[i] += second[i];
            }
            return first;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator +(Vector vector, float value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] += value;
            }
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator +(float value, Vector vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] += value;
            }
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator -(Vector first, Vector second)
        {
            Vector new_vector = new Vector(first.Length);
            for (int i = 0; i < first.Length; i++)
            {
                new_vector[i] = first[i] - second[i];
            }
            return new_vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator -(Vector vector, float value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] -= value;
            }
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator -(float value, Vector vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] -= value;
            }
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator *(Vector vector, float value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] *= value;
            }
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector operator *(float value, Vector vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] *= value;
            }
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector(int[] arr) => new Vector(arr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector(float[] arr) => new Vector(arr);

        #endregion

        #region constructors

        public Vector(int length)
        {
            _data = new float[length];
            Length = length;
        }
        public Vector(int[] data) : this(data.Length)
        {
            Array.Copy(data, _data, data.Length);
        }

        public Vector(float[] data) : this(data.Length)
        {
            Array.Copy(data, _data, data.Length);
        }

        public Vector(Vector vector) : this(vector._data.Length) 
        {
            Array.Copy(vector._data, _data, vector._data.Length);
        }

        #endregion

        public float this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _data[i];               
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set 
            {
                _data[i] = value;            
            }
        }

        public readonly int Length;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(_data.Length * 2);
            sb.Append("(" + _data[0].ToString());
            for (int i = 1; i < Length; i++)
            {
                sb.Append("; " + _data[i].ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }

        private readonly float[] _data;
    }
}
