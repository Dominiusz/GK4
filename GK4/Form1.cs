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
        private Figure selectedFigure;


        private float[,] ZBuffer;


        public Form1()
        {
            InitializeComponent();
            FillScene(Color.Black);
            scene = new Scene();
            rendering = new Rendering(ref ZBuffer);

            //pictureBox1.Image = new Bitmap("C:\\Users\\450 G2\\Desktop\\Obrazki\\Bez tytułu — kopia.png");

            SetDefaultValues();
            Cylinder cylinder = new Cylinder(20);
            scene.Figures.Add(cylinder);
            comboBox1.DataSource = null;
            comboBox1.DataSource = scene.Figures;

        }

        private void SetEdit(Figure figure)
        {
            xTranslationNUD.Value = (decimal)figure.Xtranslation;
            yTranslationNUD.Value = (decimal)figure.Ytranslation;
            zTranslationNUD.Value = (decimal)figure.Ztranslation;
            xRotationNUD.Value = (decimal)figure.Xturn;
            yRotationNUD.Value = (decimal)figure.Yturn;
            zRotationNUD.Value = (decimal)figure.Zturn;
            xScaleNUD.Value = (decimal)figure.Xscale;
            yScaleNUD.Value = (decimal)figure.Yscale;
            zScaleNUD.Value = (decimal)figure.Zscale;
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
            comboBox1.DataSource = null;
            comboBox1.DataSource = scene.Figures;
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
            positionZNUD.Value = 3M;
            lightXNUD.Increment = 0.1M;
            lightXNUD.Value = 1M;
            lightYNUD.Increment = 0.1M;
            lightYNUD.Value = 2M;
            lightZNUD.Increment = 0.1M;
            lightZNUD.Value = 1M;
            xScaleNUD.Increment = 0.01M;
            yScaleNUD.Increment = 0.01M;
            zScaleNUD.Increment = 0.01M;
            xTranslationNUD.Increment = 0.01M;
            yTranslationNUD.Increment = 0.01M;
            zTranslationNUD.Increment = 0.01M;
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

            if (rendering != null)
                rendering.ZBuffer = ZBuffer;
        }

        private void Repaint()
        {
            FillScene(scene.Background);
            Bitmap PBbtmap = pictureBox1.Image as Bitmap;

            if (noFillRB.Checked)
                rendering.RenderOutlines(scene, ref PBbtmap);
            else if (flatFillRB.Checked)
            {
                rendering.RenderFlat(scene, ref PBbtmap);
            }
            else
            {
                rendering.RenderGouraud(scene, ref PBbtmap);
            }

            pictureBox1.Image = PBbtmap;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Height > 0 && pictureBox1.Width > 0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                if (scene != null)
                    FillScene(scene.Background);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files|*.xml";
            saveFileDialog1.Title = "Save scene";
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
            openFileDialog1.Title = "Load scene";

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
            SetDefaultValues();
            scene = new Scene();
            FillScene(Color.Black);
            lightColorLabel.BackColor = Color.White;
            backgroundColorLabel.BackColor = Color.Black;
            comboBox1.DataSource = null;
            comboBox1.DataSource = scene.Figures;
            selectedFigure = null;
            RunEvents();
        }

        private void ambientTrackBar_Scroll(object sender, EventArgs e)
        {
            float val = ambientTrackBar.Value / 100f;
            scene.Light.Ambient = val;
            ambientLabel.Text = "Ambient:  " + String.Format("{0:0.00}", val);
            Repaint();
        }

        private void diffuseTrackBar_Scroll(object sender, EventArgs e)
        {
            float val = diffuseTrackBar.Value / 100f;
            scene.Light.Diffuse = val;
            diffuseLabel.Text = "Diffuse:    " + String.Format("{0:0.00}", val);
            Repaint();
        }

        private void specularTrackBar_Scroll(object sender, EventArgs e)
        {
            float val = specularTrackBar.Value / 100f;
            scene.Light.Specular = val;
            specularLabel.Text = "Specular: " + String.Format("{0:0.00}", val);
            Repaint();
        }

        private void shinessNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Shiness = (float)shinessNUD.Value;
            Repaint();
        }

        private void aspectNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Aspect = (float)aspectNUD.Value;
            Repaint();
        }

        private void fieldOfViewNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.FieldOfView = (float)fieldOfViewNUD.Value;
            Repaint();
        }

        private void farNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Far = (float)farNUD.Value;
            Repaint();
        }

        private void nearNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Near = (float)nearNUD.Value;
            Repaint();
        }

        private void backgroundColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                scene.Background = colorDialog1.Color;
                backgroundColorLabel.BackColor = colorDialog1.Color;
                Repaint();
            }
        }

        private void lightColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                scene.Light.Colour = colorDialog1.Color;
                lightColorLabel.BackColor = colorDialog1.Color;
                Repaint();
            }
        }

        private void targetXNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Target[0] = (float)targetXNUD.Value;
            Repaint();
        }

        private void targetYNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Target[1] = (float)targetYNUD.Value;
            Repaint();
        }

        private void targetZNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Target[2] = (float)targetZNUD.Value;
            Repaint();
        }

        private void positionXNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Position[0] = (float)positionXNUD.Value;
            Repaint();
        }

        private void positionYNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Position[1] = (float)positionYNUD.Value;
            Repaint();
        }

        private void positionZNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Camera.Position[2] = (float)positionZNUD.Value;
            Repaint();
        }

        private void lightXNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Position[0] = (float)lightXNUD.Value;
            Repaint();
        }

        private void lightYNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Position[1] = (float)lightYNUD.Value;
            Repaint();
        }

        private void lightZNUD_ValueChanged(object sender, EventArgs e)
        {
            scene.Light.Position[2] = (float)lightZNUD.Value;
            Repaint();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddWindow window = new AddWindow();
            window.ShowDialog();
            Figure result = window.Figure;
            if (result == null)
                return;
            else
            {
                scene.Figures.Add(result);
                comboBox1.DataSource = null;
                comboBox1.DataSource = scene.Figures;
                Repaint();
            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
                scene.Figures.RemoveAt(comboBox1.SelectedIndex);
            comboBox1.DataSource = null;
            comboBox1.DataSource = scene.Figures;
            if (comboBox1.Items.Count != 0)
            {
                selectedFigure = scene.Figures[0];
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                selectedFigure = null;
            }
            Repaint();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                selectedFigure = scene.Figures[comboBox1.SelectedIndex];
                SetEdit(scene.Figures[comboBox1.SelectedIndex]);
            }
        }

        private void xTranslationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Xtranslation = (float)xTranslationNUD.Value;
            Repaint();
        }

        private void yTranslationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Ytranslation = (float)yTranslationNUD.Value;
            Repaint();
        }

        private void zTranslationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Ztranslation = (float)zTranslationNUD.Value;
            Repaint();
        }

        private void xRotationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Xturn = (float)xRotationNUD.Value;
            Repaint();
        }

        private void yRotationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Yturn = (float)yRotationNUD.Value;
            Repaint();
        }

        private void zRotationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Zturn = (float)zRotationNUD.Value;
            Repaint();
        }

        private void xScaleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Xscale = (float)xScaleNUD.Value;
            Repaint();
        }

        private void yScaleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Yscale = (float)yScaleNUD.Value;
            Repaint();
        }

        private void zScaleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (selectedFigure != null)
                selectedFigure.Zscale = (float)zScaleNUD.Value;
            Repaint();
        }

        private void noFillRB_CheckedChanged(object sender, EventArgs e)
        {
            Repaint();
        }

        private void flatFillRB_CheckedChanged(object sender, EventArgs e)
        {
            Repaint();
        }

        private void gouraudFillRB_CheckedChanged(object sender, EventArgs e)
        {
            Repaint();
        }
    }
}
