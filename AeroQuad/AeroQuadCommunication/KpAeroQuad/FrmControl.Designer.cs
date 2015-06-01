namespace Scada.Comm.KP.AeroQuad
{
    partial class FrmControl
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
            this.btnSend_i = new System.Windows.Forms.Button();
            this.btnConvertToCsv = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnSend_i
            // 
            this.btnSend_i.Location = new System.Drawing.Point(12, 12);
            this.btnSend_i.Name = "btnSend_i";
            this.btnSend_i.Size = new System.Drawing.Size(260, 23);
            this.btnSend_i.TabIndex = 0;
            this.btnSend_i.Text = "Отправить i";
            this.btnSend_i.UseVisualStyleBackColor = true;
            this.btnSend_i.Click += new System.EventHandler(this.btnSend_i_Click);
            // 
            // btnConvertToCsv
            // 
            this.btnConvertToCsv.Location = new System.Drawing.Point(12, 41);
            this.btnConvertToCsv.Name = "btnConvertToCsv";
            this.btnConvertToCsv.Size = new System.Drawing.Size(260, 23);
            this.btnConvertToCsv.TabIndex = 1;
            this.btnConvertToCsv.Text = "Конвертировать в CSV";
            this.btnConvertToCsv.UseVisualStyleBackColor = true;
            this.btnConvertToCsv.Click += new System.EventHandler(this.btnConvertToCsv_Click);
            // 
            // FrmControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.btnConvertToCsv);
            this.Controls.Add(this.btnSend_i);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmControl";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Управление AeroQuad";
            this.Load += new System.EventHandler(this.FrmControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSend_i;
        private System.Windows.Forms.Button btnConvertToCsv;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}