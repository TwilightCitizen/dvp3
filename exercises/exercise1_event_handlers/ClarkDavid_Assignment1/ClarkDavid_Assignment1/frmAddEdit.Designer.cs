/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Event Handlers
 * Date:     April 5, 2019 */

namespace ClarkDavid_Assignment1
{
    partial class frmAddEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddEdit));
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.lblAddEdit = new System.Windows.Forms.Label();
            this.txtAddEdit = new System.Windows.Forms.TextBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            this.grpMain.BackColor = System.Drawing.Color.Transparent;
            this.grpMain.Controls.Add(this.btnCancel);
            this.grpMain.Controls.Add(this.lblAddEdit);
            this.grpMain.Controls.Add(this.txtAddEdit);
            this.grpMain.Controls.Add(this.btnOkay);
            this.grpMain.Location = new System.Drawing.Point(45, 173);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(574, 964);
            this.grpMain.TabIndex = 1;
            this.grpMain.TabStop = false;
            // 
            // lblAddEdit
            // 
            this.lblAddEdit.AutoSize = true;
            this.lblAddEdit.Location = new System.Drawing.Point(143, 792);
            this.lblAddEdit.Name = "lblAddEdit";
            this.lblAddEdit.Size = new System.Drawing.Size(264, 29);
            this.lblAddEdit.TabIndex = 0;
            this.lblAddEdit.Text = "&Enter the Course Name";
            // 
            // txtAddEdit
            // 
            this.txtAddEdit.Location = new System.Drawing.Point(148, 824);
            this.txtAddEdit.Name = "txtAddEdit";
            this.txtAddEdit.Size = new System.Drawing.Size(280, 35);
            this.txtAddEdit.TabIndex = 1;
            this.txtAddEdit.TextChanged += new System.EventHandler(this.txtAddEdit_TextChanged);
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(148, 901);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(135, 57);
            this.btnOkay.TabIndex = 2;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(293, 901);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 57);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmAddEdit
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(659, 1272);
            this.Controls.Add(this.grpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add or Edit a Class";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.Load += new System.EventHandler(this.frmAddEdit_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmAddEdit_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAddEdit_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmAddEdit_MouseUp);
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMain;
        private System.Windows.Forms.TextBox txtAddEdit;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Label lblAddEdit;
        private System.Windows.Forms.Button btnCancel;
    }
}