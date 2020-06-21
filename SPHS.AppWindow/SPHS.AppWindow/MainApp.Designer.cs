namespace SPHS.AppWindow
{
    partial class MainApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pic_vehicle_in = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNumberPlate_in = new System.Windows.Forms.TextBox();
            this.picNumberPlate_in = new System.Windows.Forms.PictureBox();
            this.pic_vehicle_out = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picNumberPlate_out = new System.Windows.Forms.PictureBox();
            this.txtNumberPlate_out = new System.Windows.Forms.TextBox();
            this.btnLoadImageIn = new System.Windows.Forms.Button();
            this.btnLoadImageOut = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_vehicle_in)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNumberPlate_in)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_vehicle_out)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNumberPlate_out)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.pic_vehicle_in);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 601);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.pic_vehicle_out);
            this.panel2.Location = new System.Drawing.Point(383, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(365, 601);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(754, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(473, 601);
            this.panel3.TabIndex = 2;
            // 
            // pic_vehicle_in
            // 
            this.pic_vehicle_in.BackColor = System.Drawing.Color.White;
            this.pic_vehicle_in.Location = new System.Drawing.Point(3, 3);
            this.pic_vehicle_in.Name = "pic_vehicle_in";
            this.pic_vehicle_in.Size = new System.Drawing.Size(359, 325);
            this.pic_vehicle_in.TabIndex = 0;
            this.pic_vehicle_in.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadImageIn);
            this.groupBox1.Controls.Add(this.picNumberPlate_in);
            this.groupBox1.Controls.Add(this.txtNumberPlate_in);
            this.groupBox1.Location = new System.Drawing.Point(3, 334);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 264);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recogize In";
            // 
            // txtNumberPlate_in
            // 
            this.txtNumberPlate_in.Location = new System.Drawing.Point(6, 19);
            this.txtNumberPlate_in.Multiline = true;
            this.txtNumberPlate_in.Name = "txtNumberPlate_in";
            this.txtNumberPlate_in.Size = new System.Drawing.Size(133, 84);
            this.txtNumberPlate_in.TabIndex = 0;
            // 
            // picNumberPlate_in
            // 
            this.picNumberPlate_in.BackColor = System.Drawing.Color.White;
            this.picNumberPlate_in.Location = new System.Drawing.Point(145, 19);
            this.picNumberPlate_in.Name = "picNumberPlate_in";
            this.picNumberPlate_in.Size = new System.Drawing.Size(208, 84);
            this.picNumberPlate_in.TabIndex = 1;
            this.picNumberPlate_in.TabStop = false;
            // 
            // pic_vehicle_out
            // 
            this.pic_vehicle_out.BackColor = System.Drawing.Color.White;
            this.pic_vehicle_out.Location = new System.Drawing.Point(3, 3);
            this.pic_vehicle_out.Name = "pic_vehicle_out";
            this.pic_vehicle_out.Size = new System.Drawing.Size(359, 325);
            this.pic_vehicle_out.TabIndex = 1;
            this.pic_vehicle_out.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLoadImageOut);
            this.groupBox2.Controls.Add(this.picNumberPlate_out);
            this.groupBox2.Controls.Add(this.txtNumberPlate_out);
            this.groupBox2.Location = new System.Drawing.Point(3, 334);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 264);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recogize Out";
            // 
            // picNumberPlate_out
            // 
            this.picNumberPlate_out.BackColor = System.Drawing.Color.White;
            this.picNumberPlate_out.Location = new System.Drawing.Point(145, 19);
            this.picNumberPlate_out.Name = "picNumberPlate_out";
            this.picNumberPlate_out.Size = new System.Drawing.Size(208, 84);
            this.picNumberPlate_out.TabIndex = 1;
            this.picNumberPlate_out.TabStop = false;
            // 
            // txtNumberPlate_out
            // 
            this.txtNumberPlate_out.Location = new System.Drawing.Point(6, 19);
            this.txtNumberPlate_out.Multiline = true;
            this.txtNumberPlate_out.Name = "txtNumberPlate_out";
            this.txtNumberPlate_out.Size = new System.Drawing.Size(133, 84);
            this.txtNumberPlate_out.TabIndex = 0;
            // 
            // btnLoadImageIn
            // 
            this.btnLoadImageIn.Location = new System.Drawing.Point(6, 109);
            this.btnLoadImageIn.Name = "btnLoadImageIn";
            this.btnLoadImageIn.Size = new System.Drawing.Size(133, 31);
            this.btnLoadImageIn.TabIndex = 2;
            this.btnLoadImageIn.Text = "Load Image In";
            this.btnLoadImageIn.UseVisualStyleBackColor = true;
            this.btnLoadImageIn.Click += new System.EventHandler(this.btnLoadImageIn_Click);
            // 
            // btnLoadImageOut
            // 
            this.btnLoadImageOut.Location = new System.Drawing.Point(6, 109);
            this.btnLoadImageOut.Name = "btnLoadImageOut";
            this.btnLoadImageOut.Size = new System.Drawing.Size(133, 31);
            this.btnLoadImageOut.TabIndex = 3;
            this.btnLoadImageOut.Text = "Load Image Out";
            this.btnLoadImageOut.UseVisualStyleBackColor = true;
            this.btnLoadImageOut.Click += new System.EventHandler(this.btnLoadImageOut_Click);
            // 
            // MainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 625);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMART PARKING HOUSE SYSTEM";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_vehicle_in)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNumberPlate_in)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_vehicle_out)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNumberPlate_out)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pic_vehicle_in;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox picNumberPlate_in;
        private System.Windows.Forms.TextBox txtNumberPlate_in;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox picNumberPlate_out;
        private System.Windows.Forms.TextBox txtNumberPlate_out;
        private System.Windows.Forms.PictureBox pic_vehicle_out;
        private System.Windows.Forms.Button btnLoadImageIn;
        private System.Windows.Forms.Button btnLoadImageOut;
    }
}

