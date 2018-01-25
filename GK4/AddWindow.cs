using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK4
{
    public partial class AddWindow : Form
    {
        public Figure Figure;
        private Color color = Color.White;

        public AddWindow()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                color = colorDialog1.Color;
                label1.BackColor = colorDialog1.Color;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = comboBox1.SelectedIndex != 0;

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            CreateFigure();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Figure = null;
            this.Close();
        }

        private void CreateFigure()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        Figure = new Cube();
                        Figure.Colour = color;
                    }
                    break;
                case 1:
                    {
                        Figure = new Cylinder((int)numericUpDown1.Value);
                        Figure.Colour = color;
                    }
                    break;
                case 2:
                    {
                        Figure = new Cone((int)numericUpDown1.Value);
                        Figure.Colour = color;
                    }
                    break;
            }
        }

    }
}
