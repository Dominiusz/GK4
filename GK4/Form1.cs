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
using System.Xml;
using System.Xml.Serialization;

namespace GK4
{
    public partial class Form1 : Form
    {
        private Scene scene;

        private int XXX = 0;
        Light light = new Light();
        private float[,] ZBuffer;
        private Triangle T;
        private Camera cam = new Camera();

        public Form1()
        {
            InitializeComponent();
            FillSceneBlack();
            scene = new Scene();
            pictureBox1.Image = new Bitmap("C:\\Users\\450 G2\\Desktop\\Obrazki\\Bez tytułu — kopia.png");



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

            scene.Camera.Position = new Vector(0f, 2, 3, 1);
            scene.Camera.Target = new Vector(0, 0.5f, 0, 1);
            scene.Camera.UpWorld = new Vector(0, 1, 0);
            Cube cube = new Cube();
            Cone cone = new Cone(16);
            Cylinder cylinder = new Cylinder(20);
            scene.Light.Colour=Color.Crimson;

            Rendering R = new Rendering(ZBuffer);

            scene.Figures.Add(cone);
            Bitmap PBbtmap = pictureBox1.Image as Bitmap;

            R.RenderGouraud(scene, ref PBbtmap);
            pictureBox1.Image = PBbtmap;

            //RenderGouraud(cone, cam,ref PBbtmap);
            //  cone.Ytranslation = 1;
            // RenderGouraud(cone, cam);
            //Render(cone, cam);




        }


        private void RenderFlat(Figure figure, Camera _cam)
        {
            //int xxx = 0;
            Matrix View = _cam.GetViewMatrix();
            Matrix Proj = null; //GetProjectionMatrix(n, f, fov, aspect);
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

                float distance = 0; //CalculateDistance3D(T[0], cam.Position);
                Vector toLightVersor = (light.Position - T[0]).Normalize();
                Vector toObserver = (cam.Position - T[0]).Normalize();

                Color
                    triangleColor =
                        Color.Aqua; //CalculateColor(toLightVersor, T.normals[0].Normalize(), toObserver, distance);

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



        private void button2_Click(object sender, EventArgs ea)
        {
            // FillSceneBlack();
            // Random rnd = new Random();
            // Triangle t = new Triangle(200, 250, 999, 100, 100, 999, 50, 450, 999);


            Cube Tri = new Cube();
            Tri.triangles = new Triangle[1];
            Tri.triangles[0] =
                new Triangle(new Vector(0, 0, 0, 1), new Vector(0.7f, 0, 0, 1), new Vector(0, 0.7f, 0, 1));
            Tri.triangles[0].normals = new Vector[3];
            Tri.triangles[0].normals[0] =
                Tri.triangles[0].normals[1] = Tri.triangles[0].normals[2] = new Vector(0, 0, 1, 0);

            //RenderGouraud(Tri,cam);

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

            int y1 = (int) y1f;
            int y2 = (int) y2f;
            int y3 = (int) y3f;
            int x1 = (int) x1f;
            int x2 = (int) x2f;
            int x3 = (int) x3f;

            int x4 = (int) (x1f + (y2f - y1f) / (y3f - y1f) * (x3f - x1f));
            int y4 = (int) y2f;
            float q = 0; //CalculateDistance2D(x1, y1, x4, y4) / CalculateDistance2D(x1, y1, x3, y3);
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

        public void FillBottomFlatTriangle(ref Bitmap B, Color C, int x1, int y1, float z1f, int x2, int y2, float z2f,
            int x3, int y3, float z3f)
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

            double dy1 = (double) (x2 - x1) / (y2 - y1);
            double dy2 = (double) (x3 - x1) / (y3 - y1);

            double curr1 = x1;
            double curr2 = x1;

            float currZ1; //= z1f;
            float currZ2; //= z2f;
            float Z;

            for (int i = y1; i <= y2; i++)
            {
                float q = 00000000000; //CalculateDistance2D(curr1, i, x1, y1) / CalculateDistance2D(x1, y1, x2, y2);
                currZ1 = z1f * (1 - q) + z2f * q;
                q = 00000000000; //CalculateDistance2D(curr2, i, x3, y3) / CalculateDistance2D(x1, y1, x3, y3);
                currZ2 = z1f * (1 - q) + z3f * q;

                for (int j = (int) curr1; j <= (int) curr2; j++)
                {
                    if (j < -1000)
                    {
                        //      if (Math.Abs(curr1) > 3000)
                        //          return;
                        break;

                    }
                    q = 000000000; //CalculateDistance2D(j, i, curr1, i) / CalculateDistance2D(curr1, i, curr2, i);
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

        public void FillTopFlatTriangle(ref Bitmap B, Color C, int x1, int y1, float z1f, int x2, int y2, float z2f,
            int x3, int y3, float z3f)
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

            double dy1 = (double) (x3 - x1) / (y3 - y1);
            double dy2 = (double) (x3 - x2) / (y3 - y2);

            double curr1 = x3;
            double curr2 = x3;

            float currZ1;
            float currZ2;
            float Z;

            /*          for (int i = y3; i > y1; i--)
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
                      }*/
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





        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Height > 0 && pictureBox1.Width > 0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                FillSceneBlack();
            }
            // button1_Click(null,null);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cam.Position[0] = (float) numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            cam.Position[1] = (float) numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            cam.Position[2] = (float) numericUpDown3.Value;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files|*.xml";
            saveFileDialog1.Title = "Zapisz scenę";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {

                FileStream fs =(FileStream)saveFileDialog1.OpenFile();

                XmlSerializer serializer = new XmlSerializer(typeof(Scene));

                serializer.Serialize(fs,scene);
                fs.Close();
            }

        }

        private void loadbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML files|*.xml";
            openFileDialog1.Title = "Wczytaj scenę";

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                XmlSerializer seralzer = new XmlSerializer(typeof(Scene));

                scene = seralzer.Deserialize(sr)as Scene;
              
                sr.Close();
            }
        }

        private void newSceneButton_Click(object sender, EventArgs e)
        {
            Bitmap B = pictureBox1.Image as Bitmap;
            Rendering r = new Rendering(ZBuffer);
            r.RenderGouraud(scene,ref B);
            pictureBox1.Image = B;
        }
    }
}
