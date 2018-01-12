using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    class Cylinder
    {
        public Triangle[] triangles;

        public Cylinder(int pieces)
        {
            if(pieces<4)
                throw new ArgumentException();


        }

    }
}
