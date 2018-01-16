using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    class Cone : Figure
    {
        public Cone(int pieces)
        {
            if (pieces < 4)
                throw new ArgumentException();
            double Pi = Math.PI;

            pieces /= 4;

            Vector center = new Vector(0, 0, 0, 1);
            Vector top = new Vector(0, 1, 0, 1);
            Vector up = new Vector(0, 1, 0, 0);
            Vector down = new Vector(0, -1, 0, 0);
            List<Vector> Base = new List<Vector>();
            double deg = Pi / 2 / pieces;
            double curr = 0;

            while (Math.Abs(curr - Pi / 2) > 0.0001)
            {
                Base.Add(new Vector((float)Math.Cos(curr), 0, (float)Math.Sin(curr), 1));
                curr += deg;
            }

            int length = Base.Count;

            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(-Base[i][2], 0, Base[i][0], 1));//-y,x
            }
            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(-Base[i][0], 0, -Base[i][2], 1)); //-x,-y
            }
            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(Base[i][2], 0, -Base[i][0], 1)); //y,-x
            }

            triangles = new Triangle[pieces * 8];

            for (int i = 0; i < Base.Count - 1; i++)
            {
                triangles[i] = new Triangle(Base[i], Base[i + 1], center, down);
            }
            triangles[Base.Count - 1] = new Triangle(Base[Base.Count - 1], Base[0], center, down);

            for (int i = 0; i < Base.Count - 1; i++)
            {
                Vector normal1 = new Vector(Base[i + 1][0], 1, Base[i + 1][1], 0);
                Vector normal2 = new Vector(Base[i][0], 1, Base[i][1], 0);
                Vector normalUp = new Vector((Base[i][0] + Base[i + 1][0]) / 2, 1, (Base[i][1] + Base[i + 1][1]) / 2, 0);

                normal1.Normalize();
                normal2.Normalize();
                normalUp.Normalize();
                triangles[i + 4 * pieces] = new Triangle(top, Base[i + 1], Base[i], normalUp, normal1, normal2);
            }

            Vector n1 = new Vector(Base[0][0], 1, Base[0][1], 0);
            Vector n2 = new Vector(Base[Base.Count - 1][0], 1, Base[Base.Count - 1][1], 0);
            Vector nUp = new Vector((Base[0][0] + Base[Base.Count - 1][0]) / 2, 1, (Base[0][1] + Base[Base.Count - 1][1]) / 2, 0);
            n1.Normalize();
            n2.Normalize();
            nUp.Normalize();

            triangles[8 * pieces - 1] = new Triangle(top, Base[0], Base[Base.Count - 1], nUp, n1, n2);

        }

    }
}
