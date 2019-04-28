namespace KeepnTabsClient
{
    partial class frmDelete
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDelete));
            this.grpSolid = new System.Windows.Forms.GroupBox();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tmrConfirm = new System.Windows.Forms.Timer(this.components);
            this.grpSolid.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSolid
            // 
            this.grpSolid.Controls.Add(this.btnNo);
            this.grpSolid.Controls.Add(this.btnYes);
            this.grpSolid.Controls.Add(this.txtLabel);
            this.grpSolid.Controls.Add(this.txtMessage);
            this.grpSolid.Location = new System.Drawing.Point(48, 146);
            this.grpSolid.Name = "grpSolid";
            this.grpSolid.Size = new System.Drawing.Size(595, 1058);
            this.grpSolid.TabIndex = 10;
            this.grpSolid.TabStop = false;
            // 
            // btnNo
            // 
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Location = new System.Drawing.Point(307, 742);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(204, 50);
            this.btnNo.TabIndex = 0;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.BtnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnYes.Location = new System.Drawing.Point(91, 742);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(204, 50);
            this.btnYes.TabIndex = 7;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnYes_MouseDown);
            this.btnYes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnYes_MouseUp);
            // 
            // txtLabel
            // 
            this.txtLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabel.ForeColor = System.Drawing.Color.OrangeRed;
            this.txtLabel.Location = new System.Drawing.Point(135, 444);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(119, 33);
            this.txtLabel.TabIndex = 1;
            this.txtLabel.Text = "Warning:";
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(106, 444);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(388, 274);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "Warning: This action will permanently delete your account and all lists and tasks" +
    " in it, and it cannot be undone.\r\n\r\nAre you sure you want to do this?  Hold yes " +
    "to proceed.";
            this.txtMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tmrConfirm
            // 
            this.tmrConfirm.Interval = 1000;
            this.tmrConfirm.Tick += new System.EventHandler(this.TmrConfirm_Tick);
            // 
            // frmDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(687, 1351);
            this.Controls.Add(this.grpSolid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDelete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmDelete";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.grpSolid.ResumeLayout(false);
            this.grpSolid.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSolid;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Timer tmrConfirm;
    }
}