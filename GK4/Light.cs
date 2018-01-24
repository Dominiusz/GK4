using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GK4
{
    [Serializable]
    public class Light
    {
        public Vector Position { get; set; }
        [XmlIgnore]
        public Color Colour { get; set; }
        [XmlElement("Colour")]
        public int ColorInt
        {
            get { return Colour.ToArgb(); }
            set { Colour = Color.FromArgb(value); }
        }
        public float Ambient { get; set; }
        public float Diffuse { get; set; }
        public float Specular { get; set; }
        public float Shiness { get; set; }

        public Light()
        {
            Position = new Vector(1, 2, 1, 1);
            Colour = Color.FromArgb(255, 255, 255);
            Ambient = 0.8f;
            Diffuse = 1f;
            Specular = 0.5f;
            Shiness = 32;

        }

        public Light(Vector position, Color colour)
        {
            Position = position;
            Colour = colour;
            Ambient = 0.8f;
            Diffuse = 1f;
            Specular = 0.5f;
            Shiness = 32;
        }

        public Light(Vector position, Color colour, float ambient, float diffuse, float specular, float shiness)
        {
            Position = position;
            Colour = colour;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shiness = shiness;
        }
    }
}
