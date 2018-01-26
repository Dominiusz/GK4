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
    public class Scene
    {
        public List<Figure> Figures { get; set; }
        public Light Light { get; set; }
        public Camera Camera { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }
        public float FieldOfView { get; set; }
        public float Aspect { get; set; }
        [XmlIgnore]
        public Color Background { get; set; }
        [XmlElement("Colour")]
        public int BackgroundInt
        {
            get { return Background.ToArgb(); }
            set { Background = Color.FromArgb(value); }
        }
        public Scene()
        {
            Near = 1f;
            Far = 100f;
            FieldOfView = 45;
            Aspect = 1;
            Camera = new Camera();
            Light = new Light();
            Figures = new List<Figure>();
            Background = Color.Black;
        }
        public Matrix GetProjectionMatrix()
        {
            float e = 1 / (float)Math.Tan(FieldOfView * Math.PI / 180 / 2);
            Matrix Proj = new Matrix(4);

            Proj[0, 0] = e / Aspect;
            Proj[1, 1] = e;
            Proj[2, 2] = (Far + Near) / (Far - Near);
            Proj[3, 2] = 1;
            Proj[2, 3] = (-2 * Far * Near) / (Far - Near);

            return Proj;
        }
    }
}
