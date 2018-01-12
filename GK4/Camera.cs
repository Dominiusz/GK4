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

        public Camera()
        {
            Position = new Vector(3, 0.5f, 0.5f);
            Target = new Vector(0, 0.5f, 0.5f);
            UpWorld = new Vector(0, 1, 0);
        }

        public Camera(Vector position, Vector target, Vector upWorld)
        {
            Position = position;
            Target = target;
            UpWorld = upWorld;
        }
    }
}