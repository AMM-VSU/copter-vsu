namespace FindMarkerUI
{
    partial class FrmMain
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
            this.lblSrcFileName = new System.Windows.Forms.Label();
            this.txtSrcFileName = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblAction = new System.Windows.Forms.Label();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.btnActionParams = new System.Windows.Forms.Button();
            this.btnExecAction = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lblImage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSrcFileName
            // 
            this.lblSrcFileName.AutoSize = true;
            this.lblSrcFileName.Location = new System.Drawing.Point(9, 9);
            this.lblSrcFileName.Name = "lblSrcFileName";
            this.lblSrcFileName.Size = new System.Drawing.Size(117, 13);
            this.lblSrcFileName.TabIndex = 0;
            this.lblSrcFileName.Text = "Source image file name";
            // 
            // txtSrcFileName
            // 
            this.txtSrcFileName.Location = new System.Drawing.Point(12, 25);
            this.txtSrcFileName.Name = "txtSrcFileName";
            this.txtSrcFileName.Size = new System.Drawing.Size(260, 20);
            this.txtSrcFileName.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 51);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(12, 77);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(110, 13);
            this.lblAction.TabIndex = 3;
            this.lblAction.Text = "Select the next action";
            // 
            // cbAction
            // 
            this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Location = new System.Drawing.Point(12, 93);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(154, 21);
            this.cbAction.TabIndex = 4;
            // 
            // btnActionParams
            // 
            this.btnActionParams.Location = new System.Drawing.Point(172, 92);
            this.btnActionParams.Name = "btnActionParams";
            this.btnActionParams.Size = new System.Drawing.Size(100, 23);
            this.btnActionParams.TabIndex = 5;
            this.btnActionParams.Text = "Parameters...";
            this.btnActionParams.UseVisualStyleBackColor = true;
            // 
            // btnExecAction
            // 
            this.btnExecAction.Location = new System.Drawing.Point(12, 120);
            this.btnExecAction.Name = "btnExecAction";
            this.btnExecAction.Size = new System.Drawing.Size(100, 23);
            this.btnExecAction.TabIndex = 6;
            this.btnExecAction.Text = "Execute";
            this.btnExecAction.UseVisualStyleBackColor = true;
            this.btnExecAction.Click += new System.EventHandler(this.btnExecAction_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 146);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(25, 13);
            this.lblLog.TabIndex = 7;
            this.lblLog.Text = "Log";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(12, 162);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(260, 187);
            this.txtLog.TabIndex = 8;
            // 
            // pbImage
            // 
            this.pbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(278, 25);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(344, 324);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 9;
            this.pbImage.TabStop = false;
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(275, 9);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(36, 13);
            this.lblImage.TabIndex = 10;
            this.lblImage.Text = "Image";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnExecAction);
            this.Controls.Add(this.btnActionParams);
            this.Controls.Add(this.cbAction);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtSrcFileName);
            this.Controls.Add(this.lblSrcFileName);
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Marker";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSrcFileName;
        private System.Windows.Forms.TextBox txtSrcFileName;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.ComboBox cbAction;
        private System.Windows.Forms.Button btnActionParams;
        private System.Windows.Forms.Button btnExecAction;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Label lblImage;
    }
}

