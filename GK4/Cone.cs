using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    class Cone:Figure
    {
        public Cone(int pieces)
        {
            if (pieces < 4)
                throw new ArgumentException();
            double Pi = Math.PI;

            pieces /= 4;

            Vector center = new Vector(0, 0, 0,1);
            Vector top = new Vector(0, 0, 1,1);
            Vector up = new Vector(0,0,1,0);
            Vector down = new Vector(0, 0, -1,0);
            List<Vector> Base = new List<Vector>();
            double deg = Pi / 2 / pieces;
            double curr = 0;

            while (Math.Abs(curr - Pi / 2) > 0.0001)
            {
                Base.Add(new Vector( (float)Math.Cos(curr), (float)Math.Sin(curr), 0,1));
                curr += deg;
            }

            int length = Base.Count;

            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(-Base[i][1], Base[i][0], 0,1));//-y,x
            }
            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(-Base[i][0], -Base[i][1], 0,1)); //-x,-y
            }
            for (int i = 0; i < length; i++)
            {
                Base.Add(new Vector(Base[i][1], -Base[i][0], 0,1)); //y,-x
            }

            triangles = new Triangle[pieces * 8];

            for (int i = 0; i < Base.Count - 1; i++)
            {            
                triangles[i] = new Triangle(Base[i], Base[i + 1], center, down);
            }
            triangles[Base.Count-1]= new Triangle(Base[Base.Count-1],Base[0],center,down);

            for (int i = 0; i < Base.Count-1; i++)
            {  
                Vector normal1 = new Vector(Base[i+1][0],Base[i+1][1],1);
                Vector normal2 = new Vector(Base[i][0], Base[i][1], 1);
                normal1.Normalize();
                normal2.Normalize();
                triangles[i+4*pieces]= new Triangle(top,Base[i+1],Base[i],up,normal1,normal2);
            }
            triangles[8 * pieces-1] = new Triangle(top, Base[Base.Count-1], Base[0]);

        }

    }
}
