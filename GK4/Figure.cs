using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GK4
{   [XmlInclude(typeof(Cube))]
    [XmlInclude(typeof(Cone))]
    [XmlInclude(typeof(Cylinder))]
    [Serializable]
    public abstract class Figure
    {
        public Triangle[] triangles;
        public float Xtranslation { get; set; }
        public float Ytranslation { get; set; }
        public float Ztranslation { get; set; }
        public float Xscale { get; set; }
        public float Yscale { get; set; }
        public float Zscale { get; set; }
        public float Xturn { get; set; }
        public float Yturn { get; set; }
        public float Zturn { get; set; }
        [XmlIgnore]
        public Color Colour { get; set; }

        [XmlElement("Colour")]
        public int ColorInt
        {
            get { return Colour.ToArgb(); }
            set { Colour = Color.FromArgb(value); }
        }


        public Matrix GetModelMatrix()
        {
            Matrix ret = null;
            if (Xtranslation != 0 || Ytranslation != 0f || Ztranslation != 0)
            {
                ret = ModelTransformations.GetTranslationMatrix(Xtranslation, Ytranslation, Ztranslation);
            }
            if (Xscale != 0 || Yscale != 0 || Zscale != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetScaleMatrix(Xscale, Yscale, Zscale));
                else
                    ret = ModelTransformations.GetScaleMatrix(Xscale, Yscale, Zscale);
            }
            if (Xturn != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetXTurnMatrix(Xturn));
                else
                    ret = ModelTransformations.GetXTurnMatrix(Xturn);
            }
            if (Yturn != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetYTurnMatrix(Yturn));
                else
                    ret = ModelTransformations.GetYTurnMatrix(Yturn);
            }
            if (Zturn != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetZTurnMatrix(Zturn));
                else
                    ret = ModelTransformations.GetZTurnMatrix(Zturn);
            }
            return ret;
        }
    }
}
