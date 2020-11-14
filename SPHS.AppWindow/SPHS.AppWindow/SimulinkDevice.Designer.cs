namespace SPHS.AppWindow
{
    partial class SimulinkDevice
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
            this.picCameraDevice = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCameraDevice)).BeginInit();
            this.SuspendLayout();
            // 
            // picCameraDevice
            // 
            this.picCameraDevice.Location = new System.Drawing.Point(12, 57);
            this.picCameraDevice.Name = "picCameraDevice";
            this.picCameraDevice.Size = new System.Drawing.Size(357, 307);
            this.picCameraDevice.TabIndex = 0;
            this.picCameraDevice.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(41, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Simulink Device Scan QRCode";
            // 
            // cbDevices
            // 
            this.cbDevices.FormattingEnabled = true;
            this.cbDevices.Location = new System.Drawing.Point(165, 382);
            this.cbDevices.Name = "cbDevices";
            this.cbDevices.Size = new System.Drawing.Size(204, 21);
            this.cbDevices.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 385);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select device simulink";
            // 
            // SimulinkDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 420);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDevices);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.picCameraDevice);
            this.Name = "SimulinkDevice";
            this.Text = "SimulinkDevice";
            ((System.ComponentModel.ISupportInitialize)(this.picCameraDevice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCameraDevice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.Label label1;
    }
}