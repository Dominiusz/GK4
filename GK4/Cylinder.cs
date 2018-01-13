using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GK4
{
    class Cylinder : Figure
    {
        public Cylinder(int pieces)
        {
            if (pieces < 4)
                throw new ArgumentException();

            double Pi = Math.PI;

            pieces /= 4;

            Vector center = new Vector(0, 0, 0, 1);
            Vector top_center = new Vector(0, 0, 1, 1);
            Vector up = new Vector(0, 0, 1, 0);
            Vector down = new Vector(0, 0, -1, 0);
            List<Vector> Base = new List<Vector>();
            List<Vector> Top = new List<Vector>();
            double deg = Pi / 2 / pieces;
            double curr = 0;

            while (Math.Abs(curr - Pi / 2) > 0.0001)
            {
                Base.Add(new Vector((float)Math.Cos(curr), (float)Math.Sin(curr), 0, 1));
                Top.Add(new Vector((float)Math.Cos(curr), (float)Math.Sin(curr), 1, 1));
                curr += deg;
            }

            int length = Base.Count;

            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(-Base[i][1], Base[i][0], 0, 1));//-y,x
                Top.Add(new Vector(-Base[i][1], Base[i][0], 1, 1));//-y,x
            }
            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(-Base[i][0], -Base[i][1], 0, 1)); //-x,-y
                Top.Add(new Vector(-Base[i][0], -Base[i][1], 1, 1));//-y,x
            }
            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(Base[i][1], -Base[i][0], 0, 1)); //y,-x
                Top.Add(new Vector(Base[i][1], -Base[i][0], 1, 1)); //y,-x
            }

            triangles = new Triangle[pieces * 16];

            int Base_count = Base.Count;

            for (int i = 0; i < Base_count - 1; i++)
            {
                triangles[i] = new Triangle(Base[i], Base[i + 1], center, down);
            }
            triangles[Base.Count - 1] = new Triangle(Base[Base.Count - 1], Base[0], center, down);

            for (int i = 0; i < Top.Count - 1; i++)
            {
                triangles[i + Base_count] = new Triangle(Top[i], Top[i + 1], top_center, up);
            }
            triangles[Base_count * 2 - 1] = new Triangle(Top[Top.Count - 1], Top[0],top_center, up);


            for (int i = 0; i < Base_count - 1; i++)
            {
                Vector normal1 = new Vector(Top[i][0], Top[i][1], 0, 0);
                Vector normal2 = new Vector(Base[i + 1][0], Base[i + 1][1], 0, 0);
                Vector normal3 = new Vector(Base[i][0], Base[i][1], 0, 0);

                Vector normal4 = new Vector(Top[i][0], Top[i][1], 0, 0);
                Vector normal5 = new Vector(Top[i + 1][0], Top[i + 1][1], 0, 0);
                Vector normal6 = new Vector(Base[i + 1][0], Base[i + 1][1], 0, 0);

                normal1.Normalize();
                normal2.Normalize();
                normal3.Normalize();
                normal4.Normalize();
                normal5.Normalize();
                normal6.Normalize();
               
                triangles[Base_count * 2 + i] = new Triangle(Top[i], Base[i + 1], Base[i], normal1, normal2, normal3);
                triangles[Base_count * 3 + i] = new Triangle(Top[i], Top[i + 1], Base[i + 1], normal4, normal5, normal6);
            }

            Vector n1 = new Vector(Top[0][0], Top[0][1], 0, 0);
            Vector n2 = new Vector(Base[Base_count - 1][0], Base[Base_count - 1][1], 0, 0);
            Vector n3 = new Vector(Base[0][0], Base[0][1], 0, 0);

            Vector n4 = new Vector(Top[0][0], Top[0][1], 0, 0);
            Vector n5 = new Vector(Top[Base_count - 1][0], Top[Base_count - 1][1], 0, 0);
            Vector n6 = new Vector(Base[Base_count - 1][0], Base[Base_count - 1][1], 0, 0);

            n1.Normalize();
            n2.Normalize();
            n3.Normalize();
            n4.Normalize();
            n5.Normalize();
            n6.Normalize();

            triangles[Base_count * 3- 1] = new Triangle(Top[0], Base[Base_count - 1], Base[0], n1, n2, n3);
            triangles[Base_count * 4-1 ] = new Triangle(Top[0], Top[Base_count-1], Base[Base_count-1], n4, n5, n6);


        }
    }
}
