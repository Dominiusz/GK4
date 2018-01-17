using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        public Form1()
        {
            InitializeComponent();
            FillSceneBlack();
            Triangle t = new Triangle(1, 1, 1, 1, 1, 1, 1, 1, 1);

        }

        private void FillSceneBlack()
        {
            if (pictureBox1.Image == null)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Size.Width, Size.Height);
            }

            Bitmap B = (Bitmap)pictureBox1.Image;
            for (int i = 0; i < B.Width; i++)
            {
                for (int j = 0; j < B.Height; j++)
                {
                    B.SetPixel(i, j, Color.Black);
                }
            }
        }

        private void FillSceneBlack1()
        {
            if (pictureBox1.Image != null)
            {
                Bitmap B = (Bitmap)pictureBox1.Image;
                for (int i = 0; i < B.Width; i++)
                {
                    for (int j = 0; j < B.Height; j++)
                    {
                        B.SetPixel(i, j, Color.Black);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Camera cam = new Camera();
            cam.Position = new Vector(3, 3, 2);
            cam.Target = new Vector(0, 0.5f, 0);
            cam.UpWorld = new Vector(0, 1, 0);
            Cube cube = new Cube();
            Cone cone = new Cone(16);
            Cylinder cylinder = new Cylinder(40);


            Render(cone, cam);


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

                Graphics G = pictureBox1.CreateGraphics();
                G.FillPolygon(Brushes.Blue, list.ToArray());
                G.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs ea)
        {
            float far = 3.789f;
            float near = 1.123f;


            Triangle t1 = new Triangle(-1, 1, far, 1, 1, far, 1, -1, near);

            Vector position = new Vector(0, 1, 0);
            Vector target = new Vector(0, 0.5f, 0.5f);
            Vector upWorld = new Vector(0, 1, 0);
            Vector D = position - target;
            Vector R = Vector.CrossProduct(upWorld, D);
            Vector U = Vector.CrossProduct(D, R);

            D.Normalize();
            R.Normalize();
            U.Normalize();

            Matrix a = new Matrix(4);

            for (int i = 0; i < 3; i++)
            {
                a[0, i] = R[i];
                a[1, i] = U[i];
                a[2, i] = D[i];
            }
            a[3, 3] = 1;

            Matrix b = Matrix.GetIdentityMatrix(4);
            for (int i = 0; i < 3; i++)
            {
                b[i, 3] = -position[i];
            }

            Matrix View = Matrix.Multiply(a, b);

            float n = 1f;
            float f = 100f;
            float fov = 45;
            float aspect = 1;

            float e = 1 / (float)(Math.Tan(fov * Math.PI / 180 / 2));

            Matrix Proj = new Matrix(4);

            Proj[0, 0] = e;
            Proj[1, 1] = e / aspect;
            Proj[2, 2] = -(f + n) / (f - n);
            Proj[3, 2] = -1;
            Proj[2, 3] = (-2 * f * n) / (f - n);

            Vector vt11 = Matrix.Multiply(View, t1[0]);
            Vector vt12 = Matrix.Multiply(View, t1[1]);
            Vector vt13 = Matrix.Multiply(View, t1[2]);

            Vector pt11 = Matrix.Multiply(Proj, vt11);
            Vector pt12 = Matrix.Multiply(Proj, vt12);
            Vector pt13 = Matrix.Multiply(Proj, vt13);

            int W = pictureBox1.Width;
            int H = pictureBox1.Height;

            float t11x = pt11[0] / pt11[3];
            float t11y = pt11[1] / pt11[3];

            float t12x = pt12[0] / pt12[3];
            float t12y = pt12[1] / pt12[3];

            float t13x = pt13[0] / pt13[3];
            float t13y = pt13[1] / pt13[3];

            t11x = (t11x + 1) * W / 2;
            t11y = -(t11y - 1) * H / 2;

            t12x = (t12x + 1) * W / 2;
            t12y = -(t12y - 1) * H / 2;

            t13x = (t13x + 1) * W / 2;
            t13y = -(t13y - 1) * H / 2;

            List<PointF> list = new List<PointF>();
            list.Add(new PointF(t11x, t11y));
            list.Add(new PointF(t12x, t12y));
            list.Add(new PointF(t13x, t13y));

            Graphics G = pictureBox1.CreateGraphics();
            G.FillPolygon(Brushes.Blue, list.ToArray());

        }

        public void MyDrawLine(ref Bitmap B, Point p1, Point p2, Color color)
        {
            // zmienne pomocnicze
            int x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
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

            B.SetPixel(x,y,color);
           
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
                    B.SetPixel(x,y,color);
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
            Bitmap B = pictureBox1.Image   as Bitmap;
            
            for (int i = 0; i < 400; i++)
            {
                MyDrawLine(ref B,  new Point(i,i), new Point(550,i), Color.White);
            }
            for (int i = 0; i < 400; i++)
            {
                MyDrawLine(ref B, new Point(i, i), new Point(550, i), Color.Red);
            }
            for (int i = 0; i < 400; i++)
            {
                MyDrawLine(ref B, new Point(i, i), new Point(550, i), Color.Blue);
            }
            for (int i = 0; i < 400; i++)
            {
                MyDrawLine(ref B, new Point(i, i), new Point(550, i), Color.Green);
            }

            pictureBox1.Image = B;
        }
    }
}
