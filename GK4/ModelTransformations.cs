using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK4
{
    public class ModelTransformations
    {
        public static Matrix GetTranslationMatrix(float x, float y, float z)
        {
            Matrix ret = Matrix.GetIdentityMatrix(4);
            ret[0, 3] = x;
            ret[1, 3] = y;
            ret[2, 3] = z;
            return ret;
        }

        public static Matrix GetScaleMatrix(float x, float y, float z)
        {
            Matrix ret = Matrix.GetIdentityMatrix(4);
            ret[0, 0] = x;
            ret[1, 1] = y;
            ret[2, 2] = z;
            return ret;
        }

        public static Matrix GetXTurnMatrix(float angle)
        {
            Matrix ret = Matrix.GetIdentityMatrix(4);
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            ret[1, 1] = cos;
            ret[2, 2] = cos;
            ret[2, 1] = sin;
            ret[1, 2] = -sin;
            return ret;
        }

        public static Matrix GetYTurnMatrix(float angle)
        {
            Matrix ret = Matrix.GetIdentityMatrix(4);
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            ret[0, 0] = cos;
            ret[2, 2] = cos;
            ret[0, 2] = sin;
            ret[2, 0] = -sin;
            return ret;
        }

        public static Matrix GetZTurnMatrix(float angle)
        {
            Matrix ret = Matrix.GetIdentityMatrix(4);
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            ret[0, 0] = cos;
            ret[1, 1] = cos;
            ret[1, 0] = sin;
            ret[0, 1] = -sin;
            return ret;
        }
    }
}
