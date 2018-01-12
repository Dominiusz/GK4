using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
    class Light
    {
        public Vector Position { get; set; }
        public Color Colour { get; set; }

        public Light()
        {
            Position = new Vector(0, 0, 2);
            Colour = Color.FromArgb(255, 255, 255);
        }

        public Light(Vector position, Color colour)
        {
            Position = position;
            Colour = colour;
        }
    }
}
