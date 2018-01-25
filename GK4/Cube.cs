using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    [Serializable]
    public class Cube : Figure
    {
        //public Triangle[] triangles;

        public Cube()
        {
            Xtranslation = 0;
            Xscale = 1;
            Xturn = 0;
            Ytranslation = 0;
            Yscale = 1;
            Yturn = 0;
            Ztranslation = 0;
            Zscale = 1;
            Zturn = 0;

            Colour = Color.White;

            triangles = new Triangle[12];

            triangles[0] = new Triangle(0, 0, 0, 1, 0, 0, 0, 0, 1);
            triangles[0].normals = new Vector[] { new Vector(0, 0, -1, 0), new Vector(0, 0, -1, 0), new Vector(0, 0, -1, 0) };

            triangles[1] = new Triangle(1, 0, 0, 1, 0, 1, 0, 0, 1);
            triangles[1].normals = new Vector[] { new Vector(0, 0, -1, 0), new Vector(0, 0, -1, 0), new Vector(0, 0, -1, 0) };

            triangles[2] = new Triangle(0, 1, 0, 0, 1, 1, 1, 1, 0);
            triangles[2].normals = new Vector[] { new Vector(0, 0, 1, 0), new Vector(0, 0, 1, 0), new Vector(0, 0, 1, 0) };

            triangles[3] = new Triangle(1, 1, 0, 0, 1, 1, 1, 1, 1);
            triangles[3].normals = new Vector[] { new Vector(0, 0, 1, 0), new Vector(0, 0, 1, 0), new Vector(0, 0, 1, 0) };

            triangles[4] = new Triangle(0, 0, 0, 0, 0, 1, 0, 1, 0);
            triangles[4].normals = new Vector[] { new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0) };

            triangles[5] = new Triangle(0, 1, 1, 0, 1, 0, 0, 0, 1);
            triangles[5].normals = new Vector[] { new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0) };

            triangles[6] = new Triangle(1, 0, 0, 1, 1, 0, 1, 0, 1);
            triangles[6].normals = new Vector[] { new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0) };

            triangles[7] = new Triangle(1, 1, 1, 1, 0, 1, 1, 1, 0);
            triangles[7].normals = new Vector[] { new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0) };

            triangles[8] = new Triangle(0, 0, 0, 0, 1, 0, 1, 1, 0);
            triangles[8].normals = new Vector[] { new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0) };

            triangles[9] = new Triangle(0, 0, 0, 1, 1, 0, 1, 0, 0);
            triangles[9].normals = new Vector[] { new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0) };

            triangles[10] = new Triangle(1, 0, 1, 1, 1, 1, 0, 0, 1);
            triangles[10].normals = new Vector[] { new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0) };

            triangles[11] = new Triangle(0, 0, 1, 1, 1, 1, 0, 1, 1);
            triangles[11].normals = new Vector[] { new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0) };
        }

        public override string ToString()
        {
            return "Cube";
        }
    }
}
