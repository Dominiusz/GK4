using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    class Camera
    {
        public Vector Position { get; set; }
        public Vector Target { get; set; }
        public Vector UpWorld { get; set; }

        public Vector D => Position - Target;
        public Vector R => Vector.CrossProduct(D, UpWorld);
        public Vector U => Vector.CrossProduct(D, R);

        public Camera()
        {
            Position = new Vector(3, 0.5f, 0.5f,1);
            Target = new Vector(0, 0.5f, 0.5f,1);
            UpWorld = new Vector(0, 1, 0);
        }

        public Camera(Vector position, Vector target, Vector upWorld)
        {
            Position = position;
            Target = target;
            UpWorld = upWorld;
        }

        public Matrix GetViewMatrix()
        {
            Vector Dn = D.GetNormalizedVector();
            Vector Rn = R.GetNormalizedVector();
            Vector Un = U.GetNormalizedVector();

            Matrix a = new Matrix(4);

            for (int i = 0; i < 3; i++)
            {
                a[0, i] = Rn[i];
                a[1, i] = Un[i];
                a[2, i] = Dn[i];
            }
            a[3, 3] = 1;

            Matrix b = Matrix.GetIdentityMatrix(4);
            for (int i = 0; i < 3; i++)
            {
                b[i, 3] = -Position[i];
            }

            return Matrix.Multiply(a, b);
        }
    }
}