using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK4
{
    public partial class Form1 : Form
    {
        float n = 1f;
        float f = 100f;
        float fov = 45;
        float aspect = 1;
        private float ambient = 0.5f;
        private float diffuse = 0.5f;
        private float specular = 0.5f;
        private float shiness = 32;
        Light light = new Light();
        private int XXX = 0;

        private float[,] ZBuffer;
        private Triangle T;
        private Camera cam = new Camera();

        public Form1()
        {
            InitializeComponent();
            FillSceneBlack();
            light.Position = new Vector(0.1f, 0.1f, 0.5f, 1);

        }

        private void FillSceneBlack()
        {
            if (pictureBox1.Image == null)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Size.Width, Size.Height);
            }
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, ImageFormat.Bmp);
            byte[] bitmapData = ms.GetBuffer();

            const int BITMAP_HEADER_OFFSET = 54;
            Color colorValue = Color.Black;

            for (int i = 0; i < bitmapData.Length - BITMAP_HEADER_OFFSET; i += 4)
            {
                bitmapData[BITMAP_HEADER_OFFSET + i] = colorValue.R;
                bitmapData[BITMAP_HEADER_OFFSET + i + 1] = colorValue.G;
                bitmapData[BITMAP_HEADER_OFFSET + i + 2] = colorValue.B;
                bitmapData[BITMAP_HEADER_OFFSET + i + 3] = colorValue.A;
            }
            pictureBox1.Image = new Bitmap(ms);

            ZBuffer = new float[pictureBox1.Width, pictureBox1.Height];
            for (int i = 0; i < ZBuffer.GetLength(0); i++)
            {
                for (int j = 0; j < ZBuffer.GetLength(1); j++)
                {
                    ZBuffer[i, j] = float.MinValue;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // cam = new Camera();
            cam.Position = new Vector(0f, 2, 1, 1);
           // pictureBox1.CreateGraphics().Clear(Color.Black);
            cam.Target = new Vector(0, 0.5f, 0, 1);
            cam.UpWorld = new Vector(0, 1, 0);
            Cube cube = new Cube();
            Cone cone = new Cone(16);
            Cylinder cylinder = new Cylinder(36);


            RenderGouraud(cylinder, cam);
            //Render(cone, cam);
          //  XXX++;



        }

        private Matrix GetProjectionMatrix(float _near, float _far, float _fov, float _aspect)
        {
            float e = 1 / (float)Math.Tan(_fov * Math.PI / 180 / 2);

            Matrix Proj = new Matrix(4);

            Proj[0, 0] = e / _aspect;
            Proj[1, 1] = e;
            Proj[2, 2] = (_far + _near) / (_far - _near);
            Proj[3, 2] = 1;
            Proj[2, 3] = (-2 * _far * _near) / (_far - _near);

            return Proj;
        }

        private void RenderGouraud(Figure figure, Camera _cam)
        {
          //  int xxx = 0;

            Matrix Model = GetModelMatrix(figure);
            Matrix View = _cam.GetViewMatrix();
            Matrix Proj = GetProjectionMatrix(n, f, fov, aspect);
            Bitmap B = pictureBox1.Image as Bitmap;
            int W = pictureBox1.Width;
            int H = pictureBox1.Height;

            Matrix PV = Matrix.Multiply(Proj, View);
            //if (Model != null)
            //    PVM = Matrix.Multiply(PVM, Model);

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

                Color col1 = CalculateColor(toLightVersor1, n1, toObserver1, dist1);
                Color col2 = CalculateColor(toLightVersor2, n2, toObserver2, dist2);
                Color col3 = CalculateColor(toLightVersor3, n3, toObserver3, dist3);

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

                FillTriangle(ref B, new Vector(v1x, v1y, v1z, v1[3]), col1, new Vector(v2x, v2y, v2z, v2[3]), col2, new Vector(v3x, v3y, v3z, v3[3]), col3);
            //    if(xxx==XXX)
            //        break;
            //    xxx++;
            }
            pictureBox1.Image = B;
        }

        private void FillTriangle(ref Bitmap b, Vector vector1, Color col1, Vector vector2, Color col2, Vector vector3, Color col3)
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
            Color col4 = InterpolateColor(col1, col3, wc1, wc3, q);


            if (y2 == y3)
            {
                FillBottomFlatTriangle(ref b, x1, y1, z1f, wc1, col1, x2, y2, z2f, wc2, col2, x3, y3, z3f, wc3, col3);
            }
            else if (y1 == y2)
            {
                FillTopFlatTriangle(ref b, x1, y1, z1f, wc1, col1, x2, y2, z2f, wc2, col2, x3, y3, z3f, wc3, col3);
            }
            else
            {

                FillBottomFlatTriangle(ref b, x1, y1, z1f, wc1, col1, x2, y2, z2f, wc2, col2, x4, y4, z4f, wc4, col4);
                FillTopFlatTriangle   (ref b, x2, y2, z2f, wc2, col2, x4, y4, z4f, wc4, col4, x3, y3, z3f, wc3, col3);
            }




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

        private void FillBottomFlatTriangle(ref Bitmap b, int x1, int y1, float z1f, float wc1, Color col1, int x2, int y2, float z2f, float wc2, Color col2, int x3, int y3, float z3f, float wc3, Color col3)
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
                        //      if (Math.Abs(curr1) > 3000)
                        //          return;
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

        private void FillTopFlatTriangle(ref Bitmap b, int x1, int y1, float z1f, float wc1, Color col1, int x2, int y2, float z2f, float wc2, Color col2, int x3, int y3, float z3f, float wc3, Color col3)
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

        private void RenderFlat(Figure figure, Camera _cam)
        {
            //int xxx = 0;
            Matrix View = _cam.GetViewMatrix();
            Matrix Proj = GetProjectionMatrix(n, f, fov, aspect);
            Matrix PVM = Matrix.Multiply(Proj, View);
            Bitmap B = pictureBox1.Image as Bitmap;


            //var M = Matrix.Multiply(ModelTransformations.GetTranslationMatrix(0, 0.5f, 0),ModelTransformations.GetXTurnMatrix(20));
            // PVM = Matrix.Multiply(PVM, M);


            foreach (var T in figure.triangles)
            {
                Vector v1 = Matrix.Multiply(PVM, T[0]);
                Vector v2 = Matrix.Multiply(PVM, T[1]);
                Vector v3 = Matrix.Multiply(PVM, T[2]);

                Vector n1 = Matrix.Multiply(PVM, T.normals[0]);
                Vector n2 = Matrix.Multiply(PVM, T.normals[1]);
                Vector n3 = Matrix.Multiply(PVM, T.normals[2]);

                n1.Normalize();
                n2.Normalize();
                n3.Normalize();

                float distance = CalculateDistance3D(T[0], cam.Position);
                Vector toLightVersor = (light.Position - T[0]).Normalize();
                Vector toObserver = (cam.Position - T[0]).Normalize();

                Color triangleColor = CalculateColor(toLightVersor, T.normals[0].Normalize(), toObserver, distance);

                // if (Vector.DotProduct(v1 - cam.Position, n1) < 0)
                //     continue;

                //Vector N = Vector.CrossProduct((v2-v1),(v3-v1));
                //if(Vector.DotProduct(v1,N)<0)
                //    continue;

                int W = pictureBox1.Width;
                int H = pictureBox1.Height;

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

                /*       List<PointF> list = new List<PointF>();
                       list.Add(new PointF(v1x, v1y));
                       list.Add(new PointF(v2x, v2y));
                       list.Add(new PointF(v3x, v3y));
                */
                // Graphics G = pictureBox1.CreateGraphics();
                // G.FillPolygon(Brushes.Blue, list.ToArray());
                // G.Dispose();

                //  Pen p = new Pen(Color.Blue, 1);
                //   Graphics G = pictureBox1.CreateGraphics();
                //  G.DrawPolygon(p, list.ToArray());
                //  G.Dispose();

                FillTriangle(ref B, new Triangle(v1x, v1y, v1z, v2x, v2y, v2z, v3x, v3y, v3z), triangleColor);
                //     MyDrawLine(ref B, (int)v1x, (int)v1y, (int)v2x, (int)v2y, Color.BlueViolet);
                //     MyDrawLine(ref B, (int)v2x, (int)v2y, (int)v3x, (int)v3y, Color.BlueViolet);
                //     MyDrawLine(ref B, (int)v3x, (int)v3y, (int)v1x, (int)v1y, Color.BlueViolet);      
                //    if (xxx == XXX)
                //    {
                //        XXX++;
                //        break;
                //    }
                //    xxx++;
            }
            pictureBox1.Image = B;
        }

        private Color CalculateColor(Vector toLightVersor, Vector normal, Vector toObserver, float distance)
        {
            int R = light.Colour.R;
            int G = light.Colour.G;
            int B = light.Colour.B;

            float L1N = Vector.DotProduct(toLightVersor, normal);
            float RV = Vector.DotProduct(2 * L1N * normal - toLightVersor, toObserver);
            float att = GetAttenuation(distance);

            R = (int)(ambient * R + ((diffuse * L1N) * R + specular * (float)Math.Pow(RV, shiness) * R) * att);
            G = (int)(ambient * G + ((diffuse * L1N) * G + specular * (float)Math.Pow(RV, shiness) * G) * att);
            B = (int)(ambient * B + ((diffuse * L1N) * B + specular * (float)Math.Pow(RV, shiness) * B) * att);

            R = Math.Min(Math.Max(0, R), 255);
            G = Math.Min(Math.Max(0, G), 255);
            B = Math.Min(Math.Max(0, B), 255);

            return Color.FromArgb(R, G, B);
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

        private void button2_Click(object sender, EventArgs ea)
        {
            // FillSceneBlack();
           // Random rnd = new Random();
           // Triangle t = new Triangle(200, 250, 999, 100, 100, 999, 50, 450, 999);
          

            Cube Tri = new Cube();
            Tri.triangles=new Triangle[1];
            Tri.triangles[0]= new Triangle(new Vector(0,0,0,1),new Vector(0.7f,0,0,1),new Vector(0,0.7f,0,1));
            Tri.triangles[0].normals=new Vector[3];
            Tri.triangles[0].normals[0] =
                Tri.triangles[0].normals[1] = Tri.triangles[0].normals[2] = new Vector(0, 0, 1, 0);

            RenderGouraud(Tri,cam);

        }

        public void FillTriangle(ref Bitmap B, Triangle T, Color C)
        {
            float x1f, y1f, x2f, y2f, x3f, y3f, z1f, z2f, z3f;

            List<Vector> sorted = new List<Vector>();
            sorted.Add(T[0]);
            sorted.Add(T[1]);
            sorted.Add(T[2]);
            sorted.Sort((v1, v2) => v1[1].CompareTo(v2[1]));

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
                FillBottomFlatTriangle(ref B, C, x1, y1, z1f, x2, y2, z2f, x3, y3, z3f);
            }
            else if (y1 == y2)
            {
                FillTopFlatTriangle(ref B, C, x1, y1, z1f, x2, y2, z2f, x3, y3, z3f);
            }
            else
            {

                FillBottomFlatTriangle(ref B, C, x1, y1, z1f, x2, y2, z2f, x4, y4, z4f);
                FillTopFlatTriangle(ref B, C, x2, y2, z2f, x4, y4, z4f, x3, y3, z3f);
            }
        }

        public void FillBottomFlatTriangle(ref Bitmap B, Color C, int x1, int y1, float z1f, int x2, int y2, float z2f, int x3, int y3, float z3f)
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
                        //      if (Math.Abs(curr1) > 3000)
                        //          return;
                        break;

                    }
                    q = CalculateDistance2D(j, i, curr1, i) / CalculateDistance2D(curr1, i, curr2, i);
                    Z = currZ1 * (1 - q) + currZ2 * q;
                    if (j > -1 && i > -1 && j < B.Width && i < B.Height && Z > ZBuffer[j, i])
                    {
                        ZBuffer[j, i] = Z;
                        B.SetPixel(j, i, C);
                    } //MyDrawLine(ref B, (int)curr1, i, (int)curr2, i, C);
                }
                curr1 += dy1;
                curr2 += dy2;
            }
        }

        public void FillTopFlatTriangle(ref Bitmap B, Color C, int x1, int y1, float z1f, int x2, int y2, float z2f, int x3, int y3, float z3f)
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
                        //   if (Math.Abs(curr1) > 3000)
                        //       return;
                        break;
                    }
                    if (j > -1 && i > -1 && j < B.Width && i < B.Height && Z > ZBuffer[j, i])
                    {
                        ZBuffer[j, i] = Z;
                        B.SetPixel(j, i, C);
                    } //  MyDrawLine(ref B,(int)curr1,i,(int)curr2,i,C);
                }
                curr1 -= dy1;
                curr2 -= dy2;
            }
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

        private float CalculateDistance2D(int x1, int y1, int x2, int y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private float CalculateDistance2D(double x1, double y1, double x2, double y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private void SwapBitmaps(ref Bitmap b1, ref Bitmap b2)
        {
            Bitmap tmp = b1;
            b1 = b2;
            b2 = tmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Matrix A = new Matrix(3);
            Matrix B = new Matrix(3);

            for (int i = 0; i < 9; i++)
            {
                A[i / 3, i % 3] = i * 5;
                B[i / 3, i % 3] = i * 3;
            }

            B[2, 2] = 373;

            MessageBox.Show(Matrix.Multiply(A, B).ToString());
            MessageBox.Show(Matrix.Multiply(B, A).ToString());


        }

        private Matrix GetModelMatrix(Figure fig)
        {
            Matrix ret = null;
            if (fig.Xtranslation != 0 && fig.Ytranslation != 0f && fig.Ztranslation != 0)
            {
                ret = ModelTransformations.GetTranslationMatrix(fig.Xtranslation, fig.Ytranslation, fig.Ztranslation);
            }
            if (fig.Xscale != 0 && fig.Yscale != 0 && fig.Zscale != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetScaleMatrix(fig.Xscale, fig.Yscale, fig.Zscale));
                else
                    ret = ModelTransformations.GetScaleMatrix(fig.Xscale, fig.Yscale, fig.Zscale);
            }
            if (fig.Xturn != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetXTurnMatrix(fig.Xturn));
                else
                    ret = ModelTransformations.GetXTurnMatrix(fig.Xturn);
            }
            if (fig.Yturn != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetYTurnMatrix(fig.Yturn));
                else
                    ret = ModelTransformations.GetYTurnMatrix(fig.Yturn);
            }
            if (fig.Zturn != 0)
            {
                if (ret != null)
                    ret = Matrix.Multiply(ret, ModelTransformations.GetZTurnMatrix(fig.Zturn));
                else
                    ret = ModelTransformations.GetZTurnMatrix(fig.Zturn);
            }
            return ret;
        }

        private float GetAttenuation(float distance)
        {
            return 1f / (1 + 0.09f * distance + 0.032f * distance * distance);
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Height > 0 && pictureBox1.Width > 0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                FillSceneBlack();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cam.Position[0] = (float)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            cam.Position[1] = (float)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            cam.Position[2] = (float)numericUpDown3.Value;
        }
    }
}
