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
            this.label2 = new System.Windows.Forms.Label();
            this.cbCompanies = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbStatusAction = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCardId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCameraDevice)).BeginInit();
            this.SuspendLayout();
            // 
            // picCameraDevice
            // 
            this.picCameraDevice.Location = new System.Drawing.Point(12, 57);
            this.picCameraDevice.Name = "picCameraDevice";
            this.picCameraDevice.Size = new System.Drawing.Size(625, 307);
            this.picCameraDevice.TabIndex = 0;
            this.picCameraDevice.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(177, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Simulink Device Scan QRCode";
            // 
            // cbDevices
            // 
            this.cbDevices.FormattingEnabled = true;
            this.cbDevices.Location = new System.Drawing.Point(181, 450);
            this.cbDevices.Name = "cbDevices";
            this.cbDevices.Size = new System.Drawing.Size(188, 21);
            this.cbDevices.TabIndex = 7;
            this.cbDevices.SelectedIndexChanged += new System.EventHandler(this.cbDevices_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 453);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select device simulink";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 416);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Select company simulink";
            // 
            // cbCompanies
            // 
            this.cbCompanies.FormattingEnabled = true;
            this.cbCompanies.Location = new System.Drawing.Point(181, 413);
            this.cbCompanies.Name = "cbCompanies";
            this.cbCompanies.Size = new System.Drawing.Size(188, 21);
            this.cbCompanies.TabIndex = 9;
            this.cbCompanies.SelectedIndexChanged += new System.EventHandler(this.cbCompanies_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 487);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "Status Connect Device";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // lbStatusAction
            // 
            this.lbStatusAction.AutoSize = true;
            this.lbStatusAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatusAction.Location = new System.Drawing.Point(178, 487);
            this.lbStatusAction.Name = "lbStatusAction";
            this.lbStatusAction.Size = new System.Drawing.Size(71, 15);
            this.lbStatusAction.TabIndex = 12;
            this.lbStatusAction.Text = "NO ACTION";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Card Id simulink";
            // 
            // txtCardId
            // 
            this.txtCardId.Location = new System.Drawing.Point(181, 377);
            this.txtCardId.Name = "txtCardId";
            this.txtCardId.Size = new System.Drawing.Size(187, 20);
            this.txtCardId.TabIndex = 14;
            this.txtCardId.TextChanged += new System.EventHandler(this.txtCardId_TextChanged);
            // 
            // SimulinkDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 516);
            this.Controls.Add(this.txtCardId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbStatusAction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCompanies);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDevices);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.picCameraDevice);
            this.Name = "SimulinkDevice";
            this.Text = "SimulinkDevice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SimulinkDevice_FormClosed);
            this.Load += new System.EventHandler(this.SimulinkDevice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCameraDevice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCameraDevice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCompanies;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbStatusAction;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCardId;
    }
}