using System;

namespace GAS.Common
{
    public class Vector
    {
        #region expressions-operators
        
        public static Vector operator +(Vector first, Vector second)
        {
            Vector new_vector = new Vector(first.Length);

            for (int i=0; i< first.Length; i++)
            {
                new_vector[i] = first[i] + second[i];
            }
            return new_vector;
        }
        
        public static Vector operator +(Vector vector, float value)
        {
            Vector new_vector = new Vector(vector.Length);

            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = vector[i] + value;
            }
            return new_vector;
        }

        public static Vector operator +(float value, Vector vector)
        {
            Vector new_vector = new Vector(vector.Length);

            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = vector[i] + value;
            }

            return new_vector;
        }

        public static Vector operator +(Vector vector)
        {
            return new Vector(vector);
        }

        public static Vector operator -(Vector first, Vector second)
        {
            Vector new_vector = new Vector(first.Length);
            for (int i = 0; i < first.Length; i++)
            {
                new_vector[i] = first[i] - second[i];
            }
            return new_vector;
        }

        public static Vector operator -(Vector vector, float value)
        {
            Vector new_vector = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = vector[i] - value;
            }
            return new_vector;
        }

        public static Vector operator -(float value, Vector vector)
        {
            Vector new_vector = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = vector[i] - value;
            }
            return new_vector;
        }

        public static Vector operator -(Vector vector)
        {
            Vector new_vector = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = -vector[i];
            }
            return new_vector;
        }

        public static Vector operator *(Vector vector, float value)
        {
            Vector new_vector = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = vector[i] * value;
            }
            return new_vector;
        }

        public static Vector operator *(float value, Vector vector)
        {
            Vector new_vector = new Vector(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                new_vector[i] = vector[i] * value;
            }
            return new_vector;
        }
                
        #endregion
        
        public Vector(Vector vector) : this(vector._data)  {   }

        public Vector(double[] data) : this(data.Length)
        {
            Array.Copy(data, _data, data.Length);
        }

        public Vector(int[] data) : this(data.Length)
        {
            Array.Copy(data, _data, data.Length);
        }

        public Vector(float[] data) : this(data.Length)
        {
            Array.Copy(data, _data, data.Length);
        }

        public Vector(int length)
        {
            _data = new float[length];
        }
                
        public float this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public int Length
        {
            get { return _data.Length; }
        }

        public override string ToString()
        {
            string result = "(" + this[0].ToString();
            for(int i=1; i< Length; i++)
            {
                result = result + "; " + this[i];
            }
            return result + ")";
        }

        private readonly float[] _data;
    }
}
