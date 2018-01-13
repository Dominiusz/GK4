﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

//using System.Windows.Forms;

namespace GK4
{
    public class Vector
    {
        private readonly float[] table;

        public int Rank => table.Length;

        public bool IsVertical { get; private set; }

        public float Length { get { return (float)Math.Sqrt(table.Sum(a => a * a)); } }

        public float this[int ind] { get { return table[ind]; } set { table[ind] = value; } }

        public Vector(List<float> list, bool _IsVertical = true)
        {
            table = list.ToArray();
            IsVertical = _IsVertical;
        }

        public Vector(params float[] tab)
        {
            table = new float[tab.Length];
            tab.CopyTo(table, 0);
            IsVertical = true;
        }

        public Vector(IEnumerable<float> set, bool _IsVertical = true)
        {
            List<float> tmp = new List<float>();
            foreach (var number in set)
            {
                tmp.Add(number);
            }

            table = tmp.ToArray();
            IsVertical = _IsVertical;
        }

        public Vector(int rank, bool _ISVertical = true)
        {
            if (rank <= 0)
                throw new ArgumentException();

            table = new float[rank];
            IsVertical = _ISVertical;
        }

        public Vector(float[] _table, bool _IsVertical = true)
        {
            table = new float[_table.Length];
            _table.CopyTo(table, 0);
            IsVertical = _IsVertical;
        }

        public Vector(Vector v)
        {
            table = new float[v.Rank];
            for (int i = 0; i < v.Rank; i++)
            {
                table[i] = v.table[i];
            }
            IsVertical = v.IsVertical;
        }

        public Vector Transpose()
        {
            IsVertical = !IsVertical;
            return this;
        }

        public Vector GetNormalizedVector()
        {
            Vector ret = new Vector(this);
            float length = ret.Length;
            if (length < 0.00001)
                throw new ArgumentException();
            for (int i = 0; i < ret.Rank; i++)
            {
                ret.table[i] = ret.table[i] / length;
            }
            return ret;
        }

        public Vector Normalize()
        {
            if (Length < 0.0000001)
                throw new ArgumentException();
            float length = Length;
            for (int i = 0; i < Rank; i++)
            {
                table[i] = table[i] / length;
            }
            return this;
        }

        public static Vector CrossProduct(Vector v1, Vector v2)
        {
            if (v1.Rank != 3 || v2.Rank != 3)
                throw new ArgumentException();
            Vector ret = new Vector(3);

            ret.table[0] = v1.table[1] * v2.table[2] - v1.table[2] * v2.table[1];
            ret.table[1] = v1.table[2] * v2.table[0] - v1.table[0] * v2.table[2];
            ret.table[2] = v1.table[0] * v2.table[1] - v1.table[1] * v2.table[0];

            return ret;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Rank != v2.Rank || v1.IsVertical != v2.IsVertical)
                throw new ArgumentException();
            Vector ret = new Vector(v1.Rank, v1.IsVertical);
            for (int i = 0; i < v1.Rank; i++)
            {
                ret.table[i] = v1.table[i] + v2.table[i];
            }
            return ret;
        }


        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Rank != v2.Rank || v1.IsVertical != v2.IsVertical)
                throw new ArgumentException();
            Vector ret = new Vector(v1.Rank, v1.IsVertical);
            for (int i = 0; i < v1.Rank; i++)
            {
                ret.table[i] = v1.table[i] - v2.table[i];
            }
            return ret;
        }

        public float ToNumber()
        {
            if (Rank != 1)
                throw new ArgumentException();
            return table[0];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!IsVertical)
            {
                sb.Append("[ ");
                for (int i = 0; i < Rank; i++)
                {
                    sb.Append(table[i] + " ");
                }
                sb.Append("]");
            }
            else
            {
                sb.AppendLine("[");
                for (int i = 0; i < Rank; i++)
                {
                    sb.AppendLine(table[i].ToString());
                }
                sb.Append("]");
            }
            return sb.ToString();
        }
    }
}
