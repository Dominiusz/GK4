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
        private Rendering rendering;

        private float[,] ZBuffer;


        public Form1()
        {
            InitializeComponent();
            FillScene(Color.Black);
            scene = new Scene();
            //pictureBox1.Image = new Bitmap("C:\\Users\\450 G2\\Desktop\\Obrazki\\Bez tytułu — kopia.png");

            SetDefaultValues();


        }

        private void SetFromSaved()
        {
            ambientTrackBar.Value = (int)(scene.Light.Ambient * 100);
            diffuseTrackBar.Value = (int)(scene.Light.Diffuse * 100);
            specularTrackBar.Value = (int)(scene.Light.Specular * 100);
            shinessNUD.Value = (decimal)scene.Light.Shiness;
            aspectNUD.Value = (decimal)scene.Aspect;
            farNUD.Value = (decimal)scene.Far;
            nearNUD.Value = (decimal)scene.Near;
            fieldOfViewNUD.Value = (decimal)scene.FieldOfView;
            targetXNUD.Value = (decimal)scene.Camera.Target[0];
            targetYNUD.Value = (decimal)scene.Camera.Target[1];
            targetZNUD.Value = (decimal)scene.Camera.Target[2];
            positionXNUD.Value = (decimal)scene.Camera.Position[0];
            positionYNUD.Value = (decimal)scene.Camera.Position[1];
            positionZNUD.Value = (decimal)scene.Camera.Position[2];
            lightXNUD.Value = (decimal)scene.Light.Position[0];
            lightYNUD.Value = (decimal)scene.Light.Position[1];
            lightZNUD.Value = (decimal)scene.Light.Position[2];

            lightColorLabel.BackColor = scene.Light.Colour;
            backgroundColorLabel.BackColor = scene.Background;
            RunEvents();
        }

        private void RunEvents()
        {
            ambientTrackBar_Scroll(null, null);
            diffuseTrackBar_Scroll(null, null);
            specularTrackBar_Scroll(null, null);
            shinessNUD_ValueChanged(null, null);
            farNUD_ValueChanged(null, null);
            nearNUD_ValueChanged(null, null);
            fieldOfViewNUD_ValueChanged(null, null);
            targetXNUD_ValueChanged(null, null);
            targetYNUD_ValueChanged(null, null);
            targetZNUD_ValueChanged(null, null);
            positionXNUD_ValueChanged(null, null);
            positionYNUD_ValueChanged(null, null);
            positionZNUD_ValueChanged(null, null);
            lightXNUD_ValueChanged(null, null);
            lightYNUD_ValueChanged(null, null);
            lightZNUD_ValueChanged(null, null);

        }

        private void SetDefaultValues()
        {
            ambientTrackBar.Value = 80;
            diffuseTrackBar.Value = 80;
            specularTrackBar.Value = 80;
            shinessNUD.Value = 32;
            aspectNUD.Increment = 0.1M;
            aspectNUD.Value = 1;
            farNUD.Value = 100;
            nearNUD.Value = 1;
            fieldOfViewNUD.Value = 45;
            targetXNUD.Increment = 0.1M;
            targetXNUD.Value = 0M;
            targetYNUD.Increment = 0.1M;
            targetYNUD.Value = new decimal(0.5);
            targetZNUD.Increment = 0.1M;
            targetZNUD.Value = 0M;
            positionXNUD.Increment = 0.1M;
            positionXNUD.Value = 0M;
            positionYNUD.Increment = 0.1M;
            positionYNUD.Value = 1M;
            positionZNUD.Increment = 0.1M;
            positionZNUD.Value = 2M;
            lightXNUD.Increment = 0.1M;
            lightXNUD.Value = 1M;
            lightYNUD.Increment = 0.1M;
            lightYNUD.Value = 2M;
            lightZNUD.Increment = 0.1M;
            lightZNUD.Value = 1M;

        }

        private void FillScene(Color color)
        {
            if (pictureBox1.Image == null)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Size.Width, Size.Height);
            }
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, ImageFormat.Bmp);
            byte[] bitmapData = ms.GetBuffer();

            const int BITMAP_HEADER_OFFSET = 54;
            Color colorValue = color;

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

            //  scene.Camera.Position = new Vector(0f, 2, 3, 1);
            //  scene.Camera.Target = new Vector(0, 0.5f, 0, 1);
            //  scene.Camera.UpWorld = new Vector(0, 1, 0);
            Cube cube = new Cube();
            Cone cone = new Cone(16);
            Cylinder cylinder = new Cylinder(20);
            //scene.Light.Colour = Color.Crimson;
            // scene.Light.Position = new Vector(0, 1, 1, 1);

            Rendering R = new Rendering(ZBuffer);

    //        scene.Figures.Add(cone);
            Bitmap PBbtmap = pictureBox1.Image as Bitmap;

            R.RenderGouraud(scene, ref PBbtmap);
            pictureBox1.Image = PBbtmap;

            //RenderGouraud(cone, cam,ref PBbtmap);
            //  cone.Ytranslation = 1;
            // RenderGouraud(cone, cam);
            //Render(cone, cam);
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
                FillScene(Color.Black);
            }
            // button1_Click(null,null);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files|*.xml";
            saveFileDialog1.Title = "Zapisz scenę";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {

                FileStream fs = (FileStream)saveFileDialog1.OpenFile();

                XmlSerializer serializer = new XmlSerializer(typeof(Scene));

                serializer.Serialize(fs, scene);
                fs.Close();
            }

        }

        private void loadbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML files|*.xml";
            openFileDialog1.Title = "Wczytaj scenę";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                XmlSerializer seralzer = new XmlSerializer(typeof(Scene));

                scene = seralzer.Deserialize(sr) as Scene;

                sr.Close();
                SetFromSaved();
            }
        }



        private void newSceneButton_Click(object sender, EventArgs e)
        {
            Bitmap B = pictureBox1.Image as Bitmap;
            Rendering r = new Rendering(ZBuffer);
            r.RenderGouraud(scene, ref B);
            pictureBox1.Image = B;
        }

        private void ambientTrackBar_Scroll(object sender, EventArgs e)
        {
            float val = ambientTrackBar.Value / 100f;
            scene.Light.Ambient = val;
            ambientLabel.Text = "Ambient:  " + String.Format("{0:0.00}", val);
        }

        private void diffuseTrackBar_Scroll(object sender, EventArgs e)
        {
            float val = diffuseTrackBar.Value / 100f;
            scene.Light.Diffuse = val;
            diffuseLabel.Text = "Diffuse:    " + String.Format("{0:0.00}", val);
        }

        private void specularTrackBar_Scroll(object sender, EventArgs e)
        {
            float val = specularTrackBar.Value / 100f;
            scene.Light.Specular = val;
            specularLabel.Text = "Specular: " + String.Format("{0:0.00}", val);
        }

        private void shinessNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Shiness = (float)shinessNUD.Value;
        }

        private void aspectNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Aspect = (float)aspectNUD.Value;
        }

        private void fieldOfViewNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.FieldOfView = (float)fieldOfViewNUD.Value;
        }

        private void farNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Far = (float)farNUD.Value;
        }

        private void nearNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Near = (float)nearNUD.Value;
        }

        private void backgroundColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                scene.Background = colorDialog1.Color;
                backgroundColorLabel.BackColor = colorDialog1.Color;
            }
        }

        private void lightColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                scene.Light.Colour = colorDialog1.Color;
                lightColorLabel.BackColor = colorDialog1.Color;
            }
        }

        private void targetXNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Target[0] = (float)targetXNUD.Value;
        }

        private void targetYNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Target[1] = (float)targetYNUD.Value;
        }

        private void targetZNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Target[2] = (float)targetZNUD.Value;
        }

        private void positionXNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Position[0] = (float)positionXNUD.Value;
        }

        private void positionYNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Position[1] = (float)positionYNUD.Value;
        }

        private void positionZNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Position[2] = (float)positionZNUD.Value;
        }

        private void lightXNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Position[0] = (float)lightXNUD.Value;
        }

        private void lightYNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Position[1] = (float)lightYNUD.Value;
        }

        private void lightZNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Position[2] = (float)lightZNUD.Value;
        }
    }
}
