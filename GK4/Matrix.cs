using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK4
{
   public class Matrix
    {
        private float[,] array;
        public int Rows => array.GetLength(0);
        public int Columns => array.GetLength(1);

        public Matrix(float[,] _array)
        {
            array = _array.Clone() as float[,];
        }

        public Matrix(int r, int c)
        {
            if(r<1||c<1)
                throw new ArgumentException();
            array = new float[r, c];
        }

        public Matrix(int n)
        {
            if (n < 1)
                throw new ArgumentException();
            array = new float[n, n];
        }

        public float this[int r, int c] { get { return array[r, c]; } set { array[r, c] = value; } }

        public static Matrix GetIdentityMatrix(int n)
        {
            if (n < 1)
                throw new ArgumentException();
            Matrix ret = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                ret.array[i, i] = 1;
            }
            return ret;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                throw new ArgumentException();
            Matrix ret = new Matrix(new float[m1.Rows, m1.Columns]);

            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    ret[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return ret;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                throw new ArgumentException();
            Matrix ret = new Matrix(new float[m1.Rows, m1.Columns]);

            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    ret[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return ret;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                sb.Append("| ");
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(array[i, j] + " ");
                }
                sb.Append("|");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Rows)
                throw new ArgumentException();
            Matrix ret = new Matrix(m1.Rows, m2.Columns);

            for (int i = 0; i < ret.array.GetLength(0); i++)
            {
                for (int j = 0; j < ret.array.GetLength(1); j++)
                {
                    for (int k = 0; k < m1.array.GetLength(1); k++)
                        ret.array[i, j] = ret.array[i, j] + m1.array[i, k] * m2.array[k, j];
                }
            }
            return ret;
        }

        public static Matrix Multiply(Vector v1, Vector v2)
        {
            if (v1.IsVertical == v2.IsVertical || v1.Rank != v2.Rank)
                throw new ArgumentException();

            if (!v1.IsVertical)
            {
                Matrix ret = new Matrix(1);
                for (int i = 0; i < v1.Rank; i++)
                {
                    ret[0, 0] += v1[i] * v2[i];
                }
                return ret;
            }
            else
            {
                Matrix ret = new Matrix(v1.Rank);
                for (int i = 0; i < v1.Rank; i++)
                {
                    for (int j = 0; j < v1.Rank; j++)
                    {
                        ret[i, j] = v1[i] * v2[j];
                    }
                }
                return ret;
            }

        }


        public static Vector Multiply(Matrix m, Vector v)
        {
            if (m.Columns != v.Rank || !v.IsVertical)
                throw new ArgumentException();
            Vector ret = new Vector(m.Rows);

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    ret[i] += m.array[i, j] * v[j];
                }
            }
            return ret;
        }

        public static Vector Multiply(Vector v, Matrix m)
        {
            if (m.Rows != v.Rank || v.IsVertical)
                throw new ArgumentException();
            Vector ret = new Vector(m.Columns, false);

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    ret[j] += m.array[i, j] * v[i];
                }
            }
            return ret;
        }

        public Vector ToVector()
        {
            if (Rows != 1 && Columns != 1)
                throw new ArgumentException();

            if (Rows == 1)
            {
                float[] tmp = new float[Columns];
                for (int i = 0; i < Columns; i++)
                {
                    tmp[i] = array[0, i];
                }
                return new Vector(tmp, false);
            }
            else
            {
                float[] tmp = new float[Rows];
                for (int i = 0; i < Rows; i++)
                {
                    tmp[i] = array[i, 0];
                }
                return new Vector(tmp);
            }
        }

        public Matrix Transpose()
        {
            float[,] new_array = new float[Columns, Rows];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    new_array[j, i] = array[i, j];
                }
            }
            array = new_array;
            return this;
        }

        public float ToNumber()
        {
            if (Rows != 1 || Columns != 1)
                throw new ArgumentException();
            return array[0, 0];
        }
    }
}
