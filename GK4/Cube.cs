using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    class Cube:Figure
    {
        //public Triangle[] triangles;

        public Cube()
        {
            triangles = new Triangle[12];

       
            
            triangles[0] = new Triangle(0, 0, 0, 0, 0, 1, 1, 0, 0);
            triangles[0].normals = new Vector[] { new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0) };

            triangles[1] = new Triangle(1, 0, 0, 0, 0, 1, 1, 0, 1);
            triangles[1].normals = new Vector[] { new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0), new Vector(0, -1, 0, 0) };

            triangles[2] = new Triangle(0, 1, 0, 1, 1, 0, 0, 1, 1);
            triangles[2].normals = new Vector[] { new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0) };

            triangles[3] = new Triangle(1, 1, 0, 1, 1, 1, 0, 1, 1);
            triangles[3].normals = new Vector[] { new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0), new Vector(0, 1, 0, 0) };

            triangles[4] = new Triangle(0, 0, 0, 0, 1, 0, 0, 0, 1);
            triangles[4].normals = new Vector[] { new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0) };

            triangles[5] = new Triangle(0, 1, 1, 0, 0, 1, 0, 1, 0);
            triangles[5].normals = new Vector[] { new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0), new Vector(-1, 0, 0, 0) };

            triangles[6] = new Triangle(1, 0, 0, 1, 0, 1, 1, 1, 0);
            triangles[6].normals = new Vector[] { new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0) };

            triangles[7] = new Triangle(1, 1, 1, 1, 1, 0, 1, 0, 1);
            triangles[7].normals = new Vector[] { new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0), new Vector(1, 0, 0, 0) };

            triangles[8] = new Triangle(0, 0, 0, 1, 1, 0, 0, 1, 0);
            triangles[8].normals = new Vector[] { new Vector(0, 0, 0, -1), new Vector(0, 0, 0, -1), new Vector(0, 0, 0, -1) };

            triangles[9] = new Triangle(0, 0, 0, 1, 0, 0, 1, 1, 0);
            triangles[9].normals = new Vector[] { new Vector(0, 0, 0, -1), new Vector(0, 0, 0, -1), new Vector(0, 0, 0, -1) };

            triangles[10] = new Triangle(1, 0, 1, 0, 0, 1, 1, 1, 1);
            triangles[10].normals = new Vector[] { new Vector(0, 0, 0, 1), new Vector(0, 0, 0, 1), new Vector(0, 0, 0, 1) };

            triangles[11] = new Triangle(0, 0, 1, 0, 1, 1, 1, 1, 1);
            triangles[11].normals = new Vector[] { new Vector(0, 0, 0, 1), new Vector(0, 0, 0, 1), new Vector(0, 0, 0, 1) };
           

           /* Vector[] vertices = {
                new Vector (0, 0, 0,1),
                new Vector (1, 0, 0,1),
                new Vector (1, 1, 0,1),
                new Vector (0, 1, 0,1),
                new Vector (0, 1, 1,1),
                new Vector (1, 1, 1,1),
                new Vector (1, 0, 1,1),
                new Vector (0, 0, 1,1)
            };



            Triangle[] t = {
              new Triangle(vertices[0], vertices[2], vertices[1]), //face front
              new Triangle(vertices[0], vertices[3], vertices[2]),
              new Triangle(vertices[2], vertices[3], vertices[4]), //face top
              new Triangle(vertices[2], vertices[4], vertices[5]),
              new Triangle(vertices[1], vertices[2], vertices[5]), //face right
              new Triangle(vertices[1], vertices[5], vertices[6]),
              new Triangle(vertices[0], vertices[7], vertices[4]), //face left
              new Triangle(vertices[0], vertices[4], vertices[3]),
              new Triangle(vertices[5], vertices[4], vertices[7]), //face back
              new Triangle(vertices[5], vertices[7], vertices[6]),
              new Triangle(vertices[0], vertices[6], vertices[7]), //face bottom
              new Triangle(vertices[0], vertices[1], vertices[6])
            }
            ;
            triangles = t;
            */
        }
    }
}
