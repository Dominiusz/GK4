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
        public Form1()
        {
            InitializeComponent();
            FillSceneBlack();
            Triangle t = new Triangle(1, 1, 1, 1, 1, 1, 1, 1, 1);
        }

        private void FillSceneBlack()
        {
            if (pictureBox1.Image == null)
                pictureBox1.Image = new Bitmap(pictureBox1.Size.Width, Size.Height);

            Graphics G = pictureBox1.CreateGraphics();
            G.Clear(Color.Black);
            G.Dispose();

           // using (Graphics gfx = Graphics.FromImage(pictureBox1.Image))
           // using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
           // {
           //     gfx.Clear(Color.Black);
           // }

        }

        private void FillSceneBlack1()
        {
            if (pictureBox1.Image != null)
            {
                Bitmap B = (Bitmap) pictureBox1.Image;
                for (int i = 0; i < B.Width; i++)
                {
                    for (int j = 0; j < B.Height; j++)
                    {
                        B.SetPixel(i,j,Color.Black);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Camera cam = new Camera();
            cam.Position = new Vector(3, 3, 0.5f);
            Cube cube = new Cube();

            Render(cube,cam);
        }

        private void Render(Cube cube,Camera cam)
        {
            Vector position = cam.Position;
            Vector target = cam.Target;
            Vector upWorld = cam.UpWorld;
            Vector D = position - target;
            Vector R = Vector.CrossProduct( upWorld,D);
            Vector U = Vector.CrossProduct( D,R);

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

            foreach (var T in cube.triangles)
            {
                Vector vt11 = Matrix.Multiply(View, T[0]);
                Vector vt12 = Matrix.Multiply(View, T[1]);
                Vector vt13 = Matrix.Multiply(View, T[2]);

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
                G.Dispose();


               //using (Graphics gfx = Graphics.FromImage(pictureBox1.Image))
               //using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 255)))
               //{
               //    gfx.FillPolygon(brush,list.ToArray());
               //}
            }


        }

        private void button2_Click(object sender, EventArgs ea)
        {
            float far = 3.789f;
            float near = 1.123f;


            Triangle t1 = new Triangle(-1, 1, far, 1, 1, far, 1, -1, near);

            Vector position = new Vector(0,1,0);
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

        private void pictureBox1_ClientSizeChanged(object sender, EventArgs e)
        {
           // FillSceneBlack();
        }


    }
}
