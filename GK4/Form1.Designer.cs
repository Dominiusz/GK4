namespace GK4
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nearLabel = new System.Windows.Forms.Label();
            this.farLabel = new System.Windows.Forms.Label();
            this.fieldOfViewLabel = new System.Windows.Forms.Label();
            this.aspectLabel = new System.Windows.Forms.Label();
            this.nearNUD = new System.Windows.Forms.NumericUpDown();
            this.farNUD = new System.Windows.Forms.NumericUpDown();
            this.fieldOfViewNUD = new System.Windows.Forms.NumericUpDown();
            this.aspectNUD = new System.Windows.Forms.NumericUpDown();
            this.shinessLabel = new System.Windows.Forms.Label();
            this.shinessNUD = new System.Windows.Forms.NumericUpDown();
            this.specularLabel = new System.Windows.Forms.Label();
            this.diffuseLabel = new System.Windows.Forms.Label();
            this.ambientLabel = new System.Windows.Forms.Label();
            this.specularTrackBar = new System.Windows.Forms.TrackBar();
            this.diffuseTrackBar = new System.Windows.Forms.TrackBar();
            this.ambientTrackBar = new System.Windows.Forms.TrackBar();
            this.newSceneButton = new System.Windows.Forms.Button();
            this.loadbutton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nearNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.farNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldOfViewNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shinessNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.specularTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ambientTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(183, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1013, 603);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(11, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 47);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(209, 97);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.nearLabel);
            this.panel1.Controls.Add(this.farLabel);
            this.panel1.Controls.Add(this.fieldOfViewLabel);
            this.panel1.Controls.Add(this.aspectLabel);
            this.panel1.Controls.Add(this.nearNUD);
            this.panel1.Controls.Add(this.farNUD);
            this.panel1.Controls.Add(this.fieldOfViewNUD);
            this.panel1.Controls.Add(this.aspectNUD);
            this.panel1.Controls.Add(this.shinessLabel);
            this.panel1.Controls.Add(this.shinessNUD);
            this.panel1.Controls.Add(this.specularLabel);
            this.panel1.Controls.Add(this.diffuseLabel);
            this.panel1.Controls.Add(this.ambientLabel);
            this.panel1.Controls.Add(this.specularTrackBar);
            this.panel1.Controls.Add(this.diffuseTrackBar);
            this.panel1.Controls.Add(this.ambientTrackBar);
            this.panel1.Controls.Add(this.newSceneButton);
            this.panel1.Controls.Add(this.loadbutton);
            this.panel1.Controls.Add(this.saveButton);
            this.panel1.Controls.Add(this.numericUpDown3);
            this.panel1.Controls.Add(this.numericUpDown2);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(807, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(531, 603);
            this.panel1.TabIndex = 4;
            // 
            // nearLabel
            // 
            this.nearLabel.AutoSize = true;
            this.nearLabel.Location = new System.Drawing.Point(325, 521);
            this.nearLabel.Name = "nearLabel";
            this.nearLabel.Size = new System.Drawing.Size(43, 17);
            this.nearLabel.TabIndex = 28;
            this.nearLabel.Text = "Near:";
            // 
            // farLabel
            // 
            this.farLabel.AutoSize = true;
            this.farLabel.Location = new System.Drawing.Point(325, 487);
            this.farLabel.Name = "farLabel";
            this.farLabel.Size = new System.Drawing.Size(33, 17);
            this.farLabel.TabIndex = 27;
            this.farLabel.Text = "Far:";
            // 
            // fieldOfViewLabel
            // 
            this.fieldOfViewLabel.AutoSize = true;
            this.fieldOfViewLabel.Location = new System.Drawing.Point(325, 453);
            this.fieldOfViewLabel.Name = "fieldOfViewLabel";
            this.fieldOfViewLabel.Size = new System.Drawing.Size(40, 17);
            this.fieldOfViewLabel.TabIndex = 26;
            this.fieldOfViewLabel.Text = "FOV:";
            // 
            // aspectLabel
            // 
            this.aspectLabel.AutoSize = true;
            this.aspectLabel.Location = new System.Drawing.Point(325, 418);
            this.aspectLabel.Name = "aspectLabel";
            this.aspectLabel.Size = new System.Drawing.Size(55, 17);
            this.aspectLabel.TabIndex = 25;
            this.aspectLabel.Text = "Aspect:";
            // 
            // nearNUD
            // 
            this.nearNUD.Location = new System.Drawing.Point(403, 519);
            this.nearNUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nearNUD.Name = "nearNUD";
            this.nearNUD.Size = new System.Drawing.Size(120, 22);
            this.nearNUD.TabIndex = 24;
            this.nearNUD.ValueChanged += new System.EventHandler(this.nearNUD_ValueChanged);
            // 
            // farNUD
            // 
            this.farNUD.Location = new System.Drawing.Point(403, 485);
            this.farNUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.farNUD.Name = "farNUD";
            this.farNUD.Size = new System.Drawing.Size(120, 22);
            this.farNUD.TabIndex = 23;
            this.farNUD.ValueChanged += new System.EventHandler(this.farNUD_ValueChanged);
            // 
            // fieldOfViewNUD
            // 
            this.fieldOfViewNUD.Location = new System.Drawing.Point(403, 451);
            this.fieldOfViewNUD.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.fieldOfViewNUD.Name = "fieldOfViewNUD";
            this.fieldOfViewNUD.Size = new System.Drawing.Size(120, 22);
            this.fieldOfViewNUD.TabIndex = 22;
            this.fieldOfViewNUD.ValueChanged += new System.EventHandler(this.fieldOfViewNUD_ValueChanged);
            // 
            // aspectNUD
            // 
            this.aspectNUD.DecimalPlaces = 1;
            this.aspectNUD.Location = new System.Drawing.Point(403, 413);
            this.aspectNUD.Name = "aspectNUD";
            this.aspectNUD.Size = new System.Drawing.Size(120, 22);
            this.aspectNUD.TabIndex = 21;
            this.aspectNUD.ValueChanged += new System.EventHandler(this.aspectNUD_ValueChanged);
            // 
            // shinessLabel
            // 
            this.shinessLabel.AutoSize = true;
            this.shinessLabel.Location = new System.Drawing.Point(19, 413);
            this.shinessLabel.Name = "shinessLabel";
            this.shinessLabel.Size = new System.Drawing.Size(62, 17);
            this.shinessLabel.TabIndex = 20;
            this.shinessLabel.Text = "Shiness:";
            // 
            // shinessNUD
            // 
            this.shinessNUD.Location = new System.Drawing.Point(122, 413);
            this.shinessNUD.Name = "shinessNUD";
            this.shinessNUD.Size = new System.Drawing.Size(191, 22);
            this.shinessNUD.TabIndex = 18;
            this.shinessNUD.ValueChanged += new System.EventHandler(this.shinessNUD_ValueChanged);
            // 
            // specularLabel
            // 
            this.specularLabel.AutoSize = true;
            this.specularLabel.Location = new System.Drawing.Point(19, 515);
            this.specularLabel.Name = "specularLabel";
            this.specularLabel.Size = new System.Drawing.Size(100, 17);
            this.specularLabel.TabIndex = 17;
            this.specularLabel.Text = "Specular: 0.80";
            // 
            // diffuseLabel
            // 
            this.diffuseLabel.AutoSize = true;
            this.diffuseLabel.Location = new System.Drawing.Point(19, 480);
            this.diffuseLabel.Name = "diffuseLabel";
            this.diffuseLabel.Size = new System.Drawing.Size(100, 17);
            this.diffuseLabel.TabIndex = 16;
            this.diffuseLabel.Text = "Diffuse:    0.80";
            // 
            // ambientLabel
            // 
            this.ambientLabel.AutoSize = true;
            this.ambientLabel.Location = new System.Drawing.Point(19, 446);
            this.ambientLabel.Name = "ambientLabel";
            this.ambientLabel.Size = new System.Drawing.Size(99, 17);
            this.ambientLabel.TabIndex = 15;
            this.ambientLabel.Text = "Ambient:  0.80";
            // 
            // specularTrackBar
            // 
            this.specularTrackBar.AutoSize = false;
            this.specularTrackBar.Location = new System.Drawing.Point(122, 514);
            this.specularTrackBar.Maximum = 100;
            this.specularTrackBar.Name = "specularTrackBar";
            this.specularTrackBar.Size = new System.Drawing.Size(191, 28);
            this.specularTrackBar.TabIndex = 14;
            this.specularTrackBar.Scroll += new System.EventHandler(this.specularTrackBar_Scroll);
            // 
            // diffuseTrackBar
            // 
            this.diffuseTrackBar.AutoSize = false;
            this.diffuseTrackBar.Location = new System.Drawing.Point(122, 480);
            this.diffuseTrackBar.Maximum = 100;
            this.diffuseTrackBar.Name = "diffuseTrackBar";
            this.diffuseTrackBar.Size = new System.Drawing.Size(191, 28);
            this.diffuseTrackBar.TabIndex = 13;
            this.diffuseTrackBar.Scroll += new System.EventHandler(this.diffuseTrackBar_Scroll);
            // 
            // ambientTrackBar
            // 
            this.ambientTrackBar.AutoSize = false;
            this.ambientTrackBar.Location = new System.Drawing.Point(122, 446);
            this.ambientTrackBar.Maximum = 100;
            this.ambientTrackBar.Name = "ambientTrackBar";
            this.ambientTrackBar.Size = new System.Drawing.Size(191, 28);
            this.ambientTrackBar.TabIndex = 12;
            this.ambientTrackBar.Scroll += new System.EventHandler(this.ambientTrackBar_Scroll);
            // 
            // newSceneButton
            // 
            this.newSceneButton.Location = new System.Drawing.Point(328, 555);
            this.newSceneButton.Name = "newSceneButton";
            this.newSceneButton.Size = new System.Drawing.Size(195, 35);
            this.newSceneButton.TabIndex = 9;
            this.newSceneButton.Text = "Nowa scena";
            this.newSceneButton.UseVisualStyleBackColor = true;
            this.newSceneButton.Click += new System.EventHandler(this.newSceneButton_Click);
            // 
            // loadbutton
            // 
            this.loadbutton.Location = new System.Drawing.Point(170, 555);
            this.loadbutton.Name = "loadbutton";
            this.loadbutton.Size = new System.Drawing.Size(143, 35);
            this.loadbutton.TabIndex = 8;
            this.loadbutton.Text = "Wczytaj";
            this.loadbutton.UseVisualStyleBackColor = true;
            this.loadbutton.Click += new System.EventHandler(this.loadbutton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(11, 555);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(143, 35);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Zapisz";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(76, 187);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(152, 136);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown2.TabIndex = 5;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(34, 97);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1338, 603);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "GK4";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nearNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.farNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldOfViewNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shinessNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.specularTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ambientTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button newSceneButton;
        private System.Windows.Forms.Button loadbutton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TrackBar specularTrackBar;
        private System.Windows.Forms.TrackBar diffuseTrackBar;
        private System.Windows.Forms.TrackBar ambientTrackBar;
        private System.Windows.Forms.Label specularLabel;
        private System.Windows.Forms.Label diffuseLabel;
        private System.Windows.Forms.Label ambientLabel;
        private System.Windows.Forms.Label shinessLabel;
        private System.Windows.Forms.NumericUpDown shinessNUD;
        private System.Windows.Forms.Label nearLabel;
        private System.Windows.Forms.Label farLabel;
        private System.Windows.Forms.Label fieldOfViewLabel;
        private System.Windows.Forms.Label aspectLabel;
        private System.Windows.Forms.NumericUpDown nearNUD;
        private System.Windows.Forms.NumericUpDown farNUD;
        private System.Windows.Forms.NumericUpDown fieldOfViewNUD;
        private System.Windows.Forms.NumericUpDown aspectNUD;
    }
}

