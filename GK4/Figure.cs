using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK4
{
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

    }
}
