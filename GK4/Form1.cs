using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private float[,] ZBuffer;
        private Triangle T;
        Camera cam= new Camera();

        public Form1()
        {
            InitializeComponent();
            FillSceneBlack();
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
           // cam.Position = new Vector(3, 2.5f, 1.5f);
           pictureBox1.CreateGraphics().Clear(Color.Black);
            cam.Target = new Vector(0, 0.5f, 0);
            cam.UpWorld = new Vector(0, 1, 0);
            Cube cube = new Cube();
            Cone cone = new Cone(16);
            Cylinder cylinder = new Cylinder(40);

            Render(cube, cam);


        }

        private Matrix GetProjectionMatrix(float _near, float _far, float _fov, float _aspect)
        {
            float e = 1 / (float)(Math.Tan(_fov * Math.PI / 180 / 2));

            Matrix Proj = new Matrix(4);

            Proj[0, 0] = e;
            Proj[1, 1] = e / _aspect;
            Proj[2, 2] = (_far + _near) / (_far - _near);
            Proj[3, 2] = 1;
            Proj[2, 3] = (-2 * _far * _near) / (_far - _near);

            return Proj;
        }

        private void Render(Figure figure, Camera cam)
        {

            Matrix View = cam.GetViewMatrix();
            Matrix Proj = GetProjectionMatrix(n, f, fov, aspect);
            Matrix PVM = Matrix.Multiply(Proj, View);
            Bitmap B = pictureBox1.Image as Bitmap;
            foreach (var T in figure.triangles)
            {
                Vector v1 = Matrix.Multiply(PVM, T[0]);
                Vector v2 = Matrix.Multiply(PVM, T[1]);
                Vector v3 = Matrix.Multiply(PVM, T[2]);

                //Vector n1 = Matrix.Multiply(PVM, T.normals[0]);

                // if (Vector.DotProduct(v1 - cam.Position, n1) < 0)
                //     continue;

                //Vector N = Vector.CrossProduct((v2-v1),(v3-v1));
                //if(Vector.DotProduct(v1,N)<0)
                //    continue;

                int W = pictureBox1.Width;
                int H = pictureBox1.Height;

                float v1x = v1[0] / v1[3];
                float v1y = v1[1] / v1[3];
                float v2x = v2[0] / v2[3];
                float v2y = v2[1] / v2[3];
                float v3x = v3[0] / v3[3];
                float v3y = v3[1] / v3[3];

                v1x = (v1x + 1) * W / 2;
                v1y = -(v1y - 1) * H / 2;
                v2x = (v2x + 1) * W / 2;
                v2y = -(v2y - 1) * H / 2;
                v3x = (v3x + 1) * W / 2;
                v3y = -(v3y - 1) * H / 2;

                List<PointF> list = new List<PointF>();
                list.Add(new PointF(v1x, v1y));
                list.Add(new PointF(v2x, v2y));
                list.Add(new PointF(v3x, v3y));
                
               // Graphics G = pictureBox1.CreateGraphics();
               // G.FillPolygon(Brushes.Blue, list.ToArray());
               // G.Dispose();
                
              //  Pen p = new Pen(Color.Blue, 1);
             //   Graphics G = pictureBox1.CreateGraphics();
              //  G.DrawPolygon(p, list.ToArray());
              //  G.Dispose();

                FillTriangle(ref B, new Triangle(v1x, v1y, 0, v2x, v2y, 0, v3x, v3y, 0), Color.Aqua);
           //     MyDrawLine(ref B, (int)v1x, (int)v1y, (int)v2x, (int)v2y, Color.BlueViolet);
           //     MyDrawLine(ref B, (int)v2x, (int)v2y, (int)v3x, (int)v3y, Color.BlueViolet);
           //     MyDrawLine(ref B, (int)v3x, (int)v3y, (int)v1x, (int)v1y, Color.BlueViolet);               
            }
            pictureBox1.Image = B;
        }

        private void button2_Click(object sender, EventArgs ea)
        {
            // FillSceneBlack();
            Random rnd = new Random();
            Triangle t = new Triangle(200, 250, 999, 100, 100, 999, 50, 450, 999);
            Bitmap b = pictureBox1.Image as Bitmap;

            //for (int i = 0; i < 10; i++)
            {
                t = new Triangle(rnd.Next(400), rnd.Next(400), rnd.Next(400), rnd.Next(400), rnd.Next(400), rnd.Next(400), rnd.Next(400), rnd.Next(400), rnd.Next(400));
                //t = new Triangle(181, 24, 999, 252, 173, 999, 210, 288, 999);
                FillTriangle(ref b, t, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            }


            pictureBox1.Image = b;
            T = t;

        }

        public void FillTriangle(ref Bitmap B, Triangle T, Color C)
        {
            float x1f, y1f, x2f, y2f, x3f, y3f;

            List<Vector> sorted = new List<Vector>();
            sorted.Add(T[0]);
            sorted.Add(T[1]);
            sorted.Add(T[2]);
            sorted.Sort((v1, v2) => v1[1].CompareTo(v2[1]));

            x1f = sorted[0][0];
            y1f = sorted[0][1];
            x2f = sorted[1][0];
            y2f = sorted[1][1];
            x3f = sorted[2][0];
            y3f = sorted[2][1];         

            int y1 = (int)y1f;
            int y2 = (int)y2f;
            int y3 = (int)y3f;
            int x1 = (int)x1f;
            int x2 = (int)x2f;
            int x3 = (int)x3f;

            int x4 = (int)(x1f + (y2f - y1f) / (y3f - y1f) * (x3f - x1f));
            int y4 = (int)y2f;

            if (y2 == y3)
            {
                FillBottomFlatTriangle(ref B, C, x1, y1, x2, y2, x3, y3);
            }
            else if (y1 == y2)
            {
                FillTopFlatTriangle(ref B, C, x1, y1, x2, y2, x3, y3);
            }
            else
            {

                FillBottomFlatTriangle(ref B, C, x1, y1, x2, y2, x4, y4);
                FillTopFlatTriangle(ref B, C, x2, y2, x4, y4, x3, y3);
            }
        }

        public void FillBottomFlatTriangle(ref Bitmap B, Color C, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            if (x2 > x3)
            {
                int tmp = x2;
                x2 = x3;
                x3 = tmp;
                tmp = y2;
                y2 = y3;
                y3 = tmp;
            }


            double dy1 = (double)(x2 - x1) / (y2 - y1);
            double dy2 = (double)(x3 - x1) / (y3 - y1);

            double curr1 = x1;
            double curr2 = x1;

            for (int i = y1; i <= y2; i++)
            {
              //  for (int j = (int)curr1; j <= (int)curr2; j++)
              //  {
                   // if (j < -1000)
                   //     break;
                  //  if (j > -1 && i > -1 && j < B.Width && i < B.Height)
                   //     B.SetPixel(j, i, C);
                    MyDrawLine(ref B, (int)curr1, i, (int)curr2, i, C);
              //  }
                curr1 += dy1;
                curr2 += dy2;
            }
        }

        public void FillTopFlatTriangle(ref Bitmap B, Color C, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            if (x1 > x2)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }


            double dy1 = (double)(x3 - x1) / (y3 - y1);
            double dy2 = (double)(x3 - x2) / (y3 - y2);

            double curr1 = x3;
            double curr2 = x3;

            for (int i = y3; i > y1; i--)
            {
              //  for (int j = (int)curr1; j <= (int)curr2; j++)
                {
                  //  if (j < -1000)
                  //      break;
                  //  if (j > -1 && i > -1 && j < B.Width && i < B.Height)
              //          B.SetPixel(j, i, C);
                     MyDrawLine(ref B,(int)curr1,i,(int)curr2,i,C);
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

        private void SwapBitmaps(ref Bitmap b1, ref Bitmap b2)
        {
            Bitmap tmp = b1;
            b1 = b2;
            b2 = tmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
;
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
