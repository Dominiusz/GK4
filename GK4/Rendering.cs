using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK4
{
    class Rendering
    {
        private float[,] ZBuffer;

        public Rendering(float[,] zbuffer)
        {
            ZBuffer = zbuffer;
        }

        public void RenderFlat(Scene scene, ref Bitmap B)
        {
            Matrix View = scene.Camera.GetViewMatrix();
            Matrix Proj = scene.GetProjectionMatrix();
            Camera cam = scene.Camera;
            Light light = scene.Light;

            int W = B.Width;
            int H = B.Height;

            Matrix PV = Matrix.Multiply(Proj, View);

            foreach (Figure figure in scene.Figures)
            {
                Color figureColor = figure.Colour;
                Matrix Model = figure.GetModelMatrix();

                foreach (Triangle T in figure.triangles)
                {
                    Vector v1, v2, v3, n1, n2, n3;

                    if (Model != null)
                    {
                        v1 = Matrix.Multiply(Model, T[0]);
                        v2 = Matrix.Multiply(Model, T[1]);
                        v3 = Matrix.Multiply(Model, T[2]);
                        n1 = Matrix.Multiply(Model, T.normals[0]);
                        n2 = Matrix.Multiply(Model, T.normals[1]);
                        n3 = Matrix.Multiply(Model, T.normals[2]);
                    }
                    else
                    {
                        v1 = T[0];
                        v2 = T[1];
                        v3 = T[2];
                        n1 = T.normals[0];
                        n2 = T.normals[1];
                        n3 = T.normals[2];
                    }
                    n1.Normalize();
                    n2.Normalize();
                    n3.Normalize();

                    float dist1 = CalculateDistance3D(v1, cam.Position);
                    float dist2 = CalculateDistance3D(v2, cam.Position);
                    float dist3 = CalculateDistance3D(v3, cam.Position);
                    Vector toLightVersor1 = (light.Position - v1).Normalize();
                    Vector toLightVersor2 = (light.Position - v2).Normalize();
                    Vector toLightVersor3 = (light.Position - v3).Normalize();
                    Vector toObserver1 = (cam.Position - v1).Normalize();
                    Vector toObserver2 = (cam.Position - v2).Normalize();
                    Vector toObserver3 = (cam.Position - v3).Normalize();

                    Color col1 = CalculateColor(toLightVersor1, n1, toObserver1, dist1, light, figureColor);
                    Color col2 = CalculateColor(toLightVersor2, n2, toObserver2, dist2, light, figureColor);
                    Color col3 = CalculateColor(toLightVersor3, n3, toObserver3, dist3, light, figureColor);

                    v1 = Matrix.Multiply(PV, v1);
                    v2 = Matrix.Multiply(PV, v2);
                    v3 = Matrix.Multiply(PV, v3);

                    float v1x = v1[0] / v1[3];
                    float v1y = v1[1] / v1[3];
                    float v1z = v1[2] / v1[3];
                    float v2x = v2[0] / v2[3];
                    float v2y = v2[1] / v2[3];
                    float v2z = v2[2] / v2[3];
                    float v3x = v3[0] / v3[3];
                    float v3y = v3[1] / v3[3];
                    float v3z = v3[2] / v3[3];

                    v1x = (v1x + 1) * W / 2;
                    v1y = -(v1y - 1) * H / 2;
                    v1z = (v1z + 1) / 2;
                    v2x = (v2x + 1) * W / 2;
                    v2y = -(v2y - 1) * H / 2;
                    v2z = (v2z + 1) / 2;
                    v3x = (v3x + 1) * W / 2;
                    v3y = -(v3y - 1) * H / 2;
                    v3z = (v3z + 1) / 2;

                    Color triangleColor = Color.FromArgb((col1.R + col2.R + col3.R) / 3, (col1.G + col2.G + col3.G) / 3,
                        (col1.B + col2.B + col3.B) / 3);

                    FillTriangleFlat(ref B, new Vector(v1x, v1y, v1z), new Vector(v2x, v2y, v2z), new Vector(v3x, v3y, v3z), triangleColor);


                }
            }
        }

        private void FillTriangleFlat(ref Bitmap b, Vector vector1, Vector vector2, Vector vector3, Color triangleColor)
        {
            List<Vector> sorted = new List<Vector>();
            sorted.Add(vector1);
            sorted.Add(vector2);
            sorted.Add(vector3);
            sorted.Sort((v1, v2) => v1[1].CompareTo(v2[1]));

            float x1f, y1f, x2f, y2f, x3f, y3f, z1f, z2f, z3f;

            x1f = sorted[0][0];
            y1f = sorted[0][1];
            z1f = sorted[0][2];

            x2f = sorted[1][0];
            y2f = sorted[1][1];
            z2f = sorted[1][2];

            x3f = sorted[2][0];
            y3f = sorted[2][1];
            z3f = sorted[2][2];

            int y1 = (int)y1f;
            int y2 = (int)y2f;
            int y3 = (int)y3f;
            int x1 = (int)x1f;
            int x2 = (int)x2f;
            int x3 = (int)x3f;

            int x4 = (int)(x1f + (y2f - y1f) / (y3f - y1f) * (x3f - x1f));
            int y4 = (int)y2f;
            float q = CalculateDistance2D(x1, y1, x4, y4) / CalculateDistance2D(x1, y1, x3, y3);
            float z4f = z1f * (1 - q) + z3f * q;

            if (y2 == y3)
            {
                FillBottomFlatTriangleFlat(ref b, triangleColor, x1, y1, z1f, x2, y2, z2f, x3, y3, z3f);
            }
            else if (y1 == y2)
            {
                FillTopFlatTriangleFlat(ref b, triangleColor, x1, y1, z1f, x2, y2, z2f, x3, y3, z3f);
            }
            else
            {

                FillBottomFlatTriangleFlat(ref b, triangleColor, x1, y1, z1f, x2, y2, z2f, x4, y4, z4f);
                FillTopFlatTriangleFlat(ref b, triangleColor, x2, y2, z2f, x4, y4, z4f, x3, y3, z3f);
            }

        }

        private void FillTopFlatTriangleFlat(ref Bitmap b, Color triangleColor, int x1, int y1, float z1f, int x2, int y2, float z2f, int x3, int y3, float z3f)
        {
            if (x1 > x2)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
                float tmpf = z1f;
                z1f = z2f;
                z2f = tmpf;
            }

            int width = b.Width;
            int height = b.Height;

            double dy1 = (double)(x3 - x1) / (y3 - y1);
            double dy2 = (double)(x3 - x2) / (y3 - y2);

            double curr1 = x3;
            double curr2 = x3;

            float currZ1;
            float currZ2;
            float Z;

            for (int i = y3; i > y1; i--)
            {
                float q = CalculateDistance2D(curr1, i, x1, y1) / CalculateDistance2D(x1, y1, x3, y3);
                currZ1 = z3f * (1 - q) + z1f * q;
                q = CalculateDistance2D(curr2, i, x3, y3) / CalculateDistance2D(x2, y2, x3, y3);
                currZ2 = z3f * (1 - q) + z2f * q;

                for (int j = (int)curr1; j <= (int)curr2; j++)
                {
                    q = CalculateDistance2D(j, i, curr1, i) / CalculateDistance2D(curr1, i, curr2, i);
                    Z = currZ1 * (1 - q) + currZ2 * q;
                    if (j < -1000)
                    {
                        break;
                    }
                    if (j > -1 && i > -1 && j < width && i < height && Z > ZBuffer[j, i])
                    {
                        ZBuffer[j, i] = Z;
                        b.SetPixel(j, i, triangleColor);
                    }
                }
                curr1 -= dy1;
                curr2 -= dy2;
            }
        }

        private void FillBottomFlatTriangleFlat(ref Bitmap b, Color triangleColor, int x1, int y1, float z1f, int x2, int y2, float z2f, int x3, int y3, float z3f)
        {
            if (x2 > x3)
            {
                int tmp = x2;
                x2 = x3;
                x3 = tmp;
                tmp = y2;
                y2 = y3;
                y3 = tmp;
                float tmpf = z2f;
                z2f = z3f;
                z3f = tmpf;
            }

            double dy1 = (double)(x2 - x1) / (y2 - y1);
            double dy2 = (double)(x3 - x1) / (y3 - y1);

            double curr1 = x1;
            double curr2 = x1;

            float currZ1; //= z1f;
            float currZ2; //= z2f;
            float Z;

            int width = b.Width;
            int height = b.Height;

            for (int i = y1; i <= y2; i++)
            {
                float q = CalculateDistance2D(curr1, i, x1, y1) / CalculateDistance2D(x1, y1, x2, y2);
                currZ1 = z1f * (1 - q) + z2f * q;
                q = CalculateDistance2D(curr2, i, x3, y3) / CalculateDistance2D(x1, y1, x3, y3);
                currZ2 = z1f * (1 - q) + z3f * q;

                for (int j = (int)curr1; j <= (int)curr2; j++)
                {
                    if (j < -1000)
                    {
                        break;
                    }
                    q = CalculateDistance2D(j, i, curr1, i) / CalculateDistance2D(curr1, i, curr2, i);
                    Z = currZ1 * (1 - q) + currZ2 * q;
                    if (j > -1 && i > -1 && j < width && i < height && Z > ZBuffer[j, i])
                    {
                        ZBuffer[j, i] = Z;
                        b.SetPixel(j, i, triangleColor);
                    }
                }
                curr1 += dy1;
                curr2 += dy2;
            }
        }

        public void RenderGouraud(Scene scene, ref Bitmap B)
        {

            Matrix View = scene.Camera.GetViewMatrix();
            Matrix Proj = scene.GetProjectionMatrix();
            Camera cam = scene.Camera;
            Light light = scene.Light;

            int W = B.Width;
            int H = B.Height;

            Matrix PV = Matrix.Multiply(Proj, View);

            foreach (Figure figure in scene.Figures)
            {
                Color figureColor = figure.Colour;
                Matrix Model = figure.GetModelMatrix();

                foreach (Triangle T in figure.triangles)
                {
                    Vector v1, v2, v3, n1, n2, n3;

                    if (Model != null)
                    {
                        v1 = Matrix.Multiply(Model, T[0]);
                        v2 = Matrix.Multiply(Model, T[1]);
                        v3 = Matrix.Multiply(Model, T[2]);
                        n1 = Matrix.Multiply(Model, T.normals[0]);
                        n2 = Matrix.Multiply(Model, T.normals[1]);
                        n3 = Matrix.Multiply(Model, T.normals[2]);
                    }
                    else
                    {
                        v1 = T[0];
                        v2 = T[1];
                        v3 = T[2];
                        n1 = T.normals[0];
                        n2 = T.normals[1];
                        n3 = T.normals[2];
                    }
                    n1.Normalize();
                    n2.Normalize();
                    n3.Normalize();

                    float dist1 = CalculateDistance3D(v1, cam.Position);
                    float dist2 = CalculateDistance3D(v2, cam.Position);
                    float dist3 = CalculateDistance3D(v3, cam.Position);
                    Vector toLightVersor1 = (light.Position - v1).Normalize();
                    Vector toLightVersor2 = (light.Position - v2).Normalize();
                    Vector toLightVersor3 = (light.Position - v3).Normalize();
                    Vector toObserver1 = (cam.Position - v1).Normalize();
                    Vector toObserver2 = (cam.Position - v2).Normalize();
                    Vector toObserver3 = (cam.Position - v3).Normalize();

                    Color col1 = CalculateColor(toLightVersor1, n1, toObserver1, dist1, light, figureColor);
                    Color col2 = CalculateColor(toLightVersor2, n2, toObserver2, dist2, light, figureColor);
                    Color col3 = CalculateColor(toLightVersor3, n3, toObserver3, dist3, light, figureColor);

                    v1 = Matrix.Multiply(PV, v1);
                    v2 = Matrix.Multiply(PV, v2);
                    v3 = Matrix.Multiply(PV, v3);

                    float v1x = v1[0] / v1[3];
                    float v1y = v1[1] / v1[3];
                    float v1z = v1[2] / v1[3];
                    float v2x = v2[0] / v2[3];
                    float v2y = v2[1] / v2[3];
                    float v2z = v2[2] / v2[3];
                    float v3x = v3[0] / v3[3];
                    float v3y = v3[1] / v3[3];
                    float v3z = v3[2] / v3[3];

                    v1x = (v1x + 1) * W / 2;
                    v1y = -(v1y - 1) * H / 2;
                    v1z = (v1z + 1) / 2;
                    v2x = (v2x + 1) * W / 2;
                    v2y = -(v2y - 1) * H / 2;
                    v2z = (v2z + 1) / 2;
                    v3x = (v3x + 1) * W / 2;
                    v3y = -(v3y - 1) * H / 2;
                    v3z = (v3z + 1) / 2;

                    FillTriangleGouraud(ref B, new Vector(v1x, v1y, v1z, v1[3]), col1, new Vector(v2x, v2y, v2z, v2[3]), col2,
                        new Vector(v3x, v3y, v3z, v3[3]), col3);

                }
            }
        }

        private void FillTriangleGouraud(ref Bitmap b, Vector vector1, Color col1, Vector vector2, Color col2, Vector vector3, Color col3)
        {
            var sorted = new List<Tuple<Vector, Color>>();
            sorted.Add(new Tuple<Vector, Color>(vector1, col1));
            sorted.Add(new Tuple<Vector, Color>(vector2, col2));
            sorted.Add(new Tuple<Vector, Color>(vector3, col3));
            sorted.Sort((v1, v2) => v1.Item1[1].CompareTo(v2.Item1[1]));

            float x1f, y1f, x2f, y2f, x3f, y3f, z1f, z2f, z3f, wc1, wc2, wc3;
            Color c1, c2, c3;

            x1f = sorted[0].Item1[0];
            y1f = sorted[0].Item1[1];
            z1f = sorted[0].Item1[2];
            wc1 = sorted[0].Item1[3];
            c1 = sorted[0].Item2;

            x2f = sorted[1].Item1[0];
            y2f = sorted[1].Item1[1];
            z2f = sorted[1].Item1[2];
            wc2 = sorted[1].Item1[3];
            c2 = sorted[1].Item2;

            x3f = sorted[2].Item1[0];
            y3f = sorted[2].Item1[1];
            z3f = sorted[2].Item1[2];
            wc3 = sorted[2].Item1[3];
            c3 = sorted[2].Item2;

            int y1 = (int)y1f;
            int y2 = (int)y2f;
            int y3 = (int)y3f;
            int x1 = (int)x1f;
            int x2 = (int)x2f;
            int x3 = (int)x3f;

            int x4 = (int)(x1f + (y2f - y1f) / (y3f - y1f) * (x3f - x1f));
            int y4 = (int)y2f;
            float q = CalculateDistance2D(x1, y1, x4, y4) / CalculateDistance2D(x1, y1, x3, y3);
            float z4f = z1f * (1 - q) + z3f * q;
            float wc4 = wc1 * (1 - q) + wc3 * q;
            Color c4 = InterpolateColor(c1, c3, wc1, wc3, q);


            if (y2 == y3)
            {
                FillBottomFlatTriangleGouraud(ref b, x1, y1, z1f, wc1, c1, x2, y2, z2f, wc2, c2, x3, y3, z3f, wc3, c3);
            }
            else if (y1 == y2)
            {
                FillTopFlatTriangleGouraud(ref b, x1, y1, z1f, wc1, c1, x2, y2, z2f, wc2, c2, x3, y3, z3f, wc3, c3);
            }
            else
            {

                FillBottomFlatTriangleGouraud(ref b, x1, y1, z1f, wc1, c1, x2, y2, z2f, wc2, c2, x4, y4, z4f, wc4, c4);
                FillTopFlatTriangleGouraud(ref b, x2, y2, z2f, wc2, c2, x4, y4, z4f, wc4, c4, x3, y3, z3f, wc3, c3);
            }
        }

        private void FillBottomFlatTriangleGouraud(ref Bitmap b, int x1, int y1, float z1f, float wc1, Color col1, int x2, int y2, float z2f, float wc2, Color col2, int x3, int y3, float z3f, float wc3, Color col3)
        {
            if (x2 > x3)
            {
                int tmp = x2;
                x2 = x3;
                x3 = tmp;
                tmp = y2;
                y2 = y3;
                y3 = tmp;
                float tmpf = z2f;
                z2f = z3f;
                z3f = tmpf;
                tmpf = wc2;
                wc2 = wc3;
                wc3 = tmpf;
                Color tmpc = col2;
                col2 = col3;
                col3 = tmpc;
            }

            int width = b.Width;
            int height = b.Height;

            double dy1 = (double)(x2 - x1) / (y2 - y1);
            double dy2 = (double)(x3 - x1) / (y3 - y1);

            double curr1 = x1;
            double curr2 = x1;

            float currZ1; //= z1f;
            float currZ2; //= z2f;
            float Z;

            float currWC1;
            float currWC2;

            Color C1;
            Color C2;
            Color C;

            for (int i = y1; i <= y2; i++)
            {
                float q = CalculateDistance2D((int)curr1, i, x1, y1) / CalculateDistance2D(x1, y1, x2, y2);
                currZ1 = z1f * (1 - q) + z2f * q;
                currWC1 = wc1 * (1 - q) + wc2 * q;
                C1 = InterpolateColor(col1, col2, wc1, wc2, q);

                q = CalculateDistance2D((int)curr2, i, x3, y3) / CalculateDistance2D(x1, y1, x3, y3);
                currZ2 = z1f * (1 - q) + z3f * q;
                currWC2 = wc1 * (1 - q) + wc3 * q;
                C2 = InterpolateColor(col1, col3, wc1, wc3, q);

                for (int j = (int)curr1; j <= (int)curr2; j++)
                {
                    if (j < -1000)
                    {
                        //if (Math.Abs(curr1) > 3000)
                        //    return;
                        break;

                    }
                    q = CalculateDistance2D(j, i, (int)curr1, i) / CalculateDistance2D((int)curr1, i, (int)curr2, i);
                    Z = currZ1 * (1 - q) + currZ2 * q;

                    C = InterpolateColor(C1, C2, currWC1, currWC2, q);

                    if (j > -1 && i > -1 && j < width && i < height && Z > ZBuffer[j, i])
                    {
                        ZBuffer[j, i] = Z;
                        b.SetPixel(j, i, C);
                    } //MyDrawLine(ref B, (int)curr1, i, (int)curr2, i, C);
                }
                curr1 += dy1;
                curr2 += dy2;
            }
        }

        private void FillTopFlatTriangleGouraud(ref Bitmap b, int x1, int y1, float z1f, float wc1, Color col1, int x2, int y2, float z2f, float wc2, Color col2, int x3, int y3, float z3f, float wc3, Color col3)
        {
            if (x1 > x2)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
                float tmpf = z1f;
                z1f = z2f;
                z2f = tmpf;
                tmpf = wc1;
                wc1 = wc2;
                wc2 = tmpf;
                Color tmpc = col1;
                col1 = col2;
                col2 = tmpc;
            }
            int width = b.Width;
            int height = b.Height;

            double dy1 = (double)(x3 - x1) / (y3 - y1);
            double dy2 = (double)(x3 - x2) / (y3 - y2);

            double curr1 = x3;
            double curr2 = x3;

            float currZ1;
            float currZ2;
            float Z;

            float currWC1;
            float currWC2;

            Color C1;
            Color C2;
            Color C;

            for (int i = y3; i > y1; i--)
            {
                float q = CalculateDistance2D((int)curr1, i, x1, y1) / CalculateDistance2D(x1, y1, x3, y3);
                currZ1 = z3f * (1 - q) + z1f * q;
                currWC1 = wc3 * (1 - q) + wc1 * q;
                C1 = InterpolateColor(col3, col1, wc3, wc1, q);

                q = CalculateDistance2D((int)curr2, i, x3, y3) / CalculateDistance2D(x2, y2, x3, y3);
                currZ2 = z3f * (1 - q) + z2f * q;
                currWC2 = wc3 * (1 - q) + wc2 * q;
                C2 = InterpolateColor(col3, col2, wc3, wc2, q);

                for (int j = (int)curr1; j <= (int)curr2; j++)
                {
                    q = CalculateDistance2D(j, i, (int)curr1, i) / CalculateDistance2D((int)curr1, i, (int)curr2, i);
                    Z = currZ1 * (1 - q) + currZ2 * q;
                    C = InterpolateColor(C1, C2, currWC1, currWC2, q);

                    if (j < -1000)
                    {
                        //   if (Math.Abs(curr1) > 3000)
                        //       return;
                        break;
                    }
                    if (j > -1 && i > -1 && j < width && i < height && Z > ZBuffer[j, i])
                    {
                        ZBuffer[j, i] = Z;
                        b.SetPixel(j, i, C);
                    } //  MyDrawLine(ref B,(int)curr1,i,(int)curr2,i,C);
                }
                curr1 -= dy1;
                curr2 -= dy2;
            }
        }

        private Color CalculateColor(Vector toLightVersor, Vector normal, Vector toObserver, float distance, Light light, Color figureColor)
        {
            float R = light.Colour.R / 255f;
            float G = light.Colour.G / 255f;
            float B = light.Colour.B / 255f;

            float Rfig = figureColor.R / 255f;
            float Gfig = figureColor.G / 255f;
            float Bfig = figureColor.B / 255f;

            float ambient = light.Ambient;
            float diffuse = light.Diffuse;
            float specular = light.Specular;
            float shiness = light.Shiness;

            float L1N = Vector.DotProduct(toLightVersor, normal);
            float RV = Vector.DotProduct(2 * L1N * normal - toLightVersor, toObserver);
            float att = GetAttenuation(distance);

            R = ambient * R * Rfig + ((diffuse * L1N) * R * Rfig + specular * (float)Math.Pow(RV, shiness) * R * Rfig) * att;
            G = ambient * G * Gfig + ((diffuse * L1N) * G * Gfig + specular * (float)Math.Pow(RV, shiness) * G * Gfig) * att;
            B = ambient * B * Bfig + ((diffuse * L1N) * B * Bfig + specular * (float)Math.Pow(RV, shiness) * B * Bfig) * att;

            R = Math.Min(Math.Max(0, R), 1);
            G = Math.Min(Math.Max(0, G), 1);
            B = Math.Min(Math.Max(0, B), 1);

            return Color.FromArgb((int)(255 * R), (int)(255 * G), (int)(255 * B));
        }

        private float CalculateDistance3D(Vector v1, Vector v2)
        {
            float ret = 0f;
            for (int i = 0; i < 3; i++)
            {
                ret += (v1[i] - v2[i]) * (v1[i] - v2[i]);
            }
            return (float)Math.Sqrt(ret);
        }

        private float CalculateDistance2D(int x1, int y1, int x2, int y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private float CalculateDistance2D(double x1, double y1, double x2, double y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private float GetAttenuation(float distance)
        {
            return 1f / (1 + 0.09f * distance + 0.032f * distance * distance);
        }

        private Color InterpolateColor(Color col1, Color col2, float wc1, float wc2, float q)
        {
            int R = (int)((col1.R * (1 - q) / wc1 + col2.R * q / wc2) / ((1 - q) / wc1 + q / wc2));
            int G = (int)((col1.G * (1 - q) / wc1 + col2.G * q / wc2) / ((1 - q) / wc1 + q / wc2));
            int B = (int)((col1.B * (1 - q) / wc1 + col2.B * q / wc2) / ((1 - q) / wc1 + q / wc2));

            R = Math.Min(Math.Max(0, R), 255);
            G = Math.Min(Math.Max(0, G), 255);
            B = Math.Min(Math.Max(0, B), 255);

            return Color.FromArgb(R, G, B);
        }
    
        public void MyDrawLine(ref Bitmap B, int _x1, int _y1, int _x2, int _y2, Color color)
        {
            // zmienne pomocnicze
            int x1 = _x1, y1 = _y1, x2 = _x2, y2 = _y2;
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;

            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel
            if (x > -1 && y > -1 && x < B.Width && y < B.Height)
                B.SetPixel(0, y, color);

            // oś wiodąca OX
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while (x != x2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    if (x > -1 && y > -1 && x < B.Width && y < B.Height)
                        B.SetPixel(x, y, color);
                }
            }
            // oś wiodąca OY
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while (y != y2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    if (x > -1 && y > -1 && x < B.Width && y < B.Height)
                        B.SetPixel(x, y, color);
                }
            }
        }
    }
}
