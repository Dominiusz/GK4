using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    [Serializable]
    public class Triangle
    {
        public Vector[] normals;
        public Vector[] coordinates;
        public Triangle()
        {
            ;
        }

        public Triangle(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {
            coordinates = new Vector[3];
            coordinates[0] = new Vector(x1, y1, z1, 1);
            coordinates[1] = new Vector(x2, y2, z2, 1);
            coordinates[2] = new Vector(x3, y3, z3, 1);
        }

        public Triangle(Vector v1, Vector v2, Vector v3)
        {
            coordinates = new Vector[3];
            coordinates[0] = new Vector(v1);
            coordinates[1] = new Vector(v2);
            coordinates[2] = new Vector(v3);
        }

        public Triangle(Vector v1, Vector v2, Vector v3, Vector normal)
        {
            coordinates = new Vector[3];
            coordinates[0] = new Vector(v1);
            coordinates[1] = new Vector(v2);
            coordinates[2] = new Vector(v3);
            normals = new Vector[3];
            normals[0] = new Vector(normal);
            normals[1] = new Vector(normal);
            normals[2] = new Vector(normal);
        }

        public Triangle(Vector v1, Vector v2, Vector v3, Vector n1, Vector n2, Vector n3)
        {
            coordinates = new Vector[3];
            coordinates[0] = new Vector(v1);
            coordinates[1] = new Vector(v2);
            coordinates[2] = new Vector(v3);
            normals = new Vector[3];
            normals[0] = new Vector(n1);
            normals[1] = new Vector(n2);
            normals[2] = new Vector(n3);
        }

        public Vector this[int ind] { get { return coordinates[ind]; } set { coordinates[ind] = value; } }
    }
}
