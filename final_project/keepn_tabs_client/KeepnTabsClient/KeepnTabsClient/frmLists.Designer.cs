/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Client App
 * Synopsis: Client app for the Keep'n Tabs stack.
 * Date:     April 14, 2019 */

namespace KeepnTabsClient
{
    partial class frmLists
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnMark = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtEdit = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDeleteGhost = new System.Windows.Forms.Button();
            this.btnMarkGhost = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(49, 1158);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(51, 48);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "<";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // btnMark
            // 
            this.btnMark.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMark.Location = new System.Drawing.Point(97, 271);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(141, 49);
            this.btnMark.TabIndex = 2;
            this.btnMark.Text = "Done";
            this.btnMark.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(446, 271);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(141, 49);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // txtEdit
            // 
            this.txtEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEdit.Location = new System.Drawing.Point(97, 271);
            this.txtEdit.Name = "txtEdit";
            this.txtEdit.Size = new System.Drawing.Size(0, 47);
            this.txtEdit.TabIndex = 4;
            this.txtEdit.Text = "Some text in here.";
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(97, 271);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(490, 49);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Some kind of text.";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            this.btnEdit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnEdit_MouseDown);
            this.btnEdit.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BtnEdit_MouseMove);
            this.btnEdit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnEdit_MouseUp);
            // 
            // btnDeleteGhost
            // 
            this.btnDeleteGhost.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteGhost.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteGhost.Location = new System.Drawing.Point(446, 271);
            this.btnDeleteGhost.Name = "btnDeleteGhost";
            this.btnDeleteGhost.Size = new System.Drawing.Size(141, 49);
            this.btnDeleteGhost.TabIndex = 7;
            this.btnDeleteGhost.Text = "Delete";
            this.btnDeleteGhost.UseVisualStyleBackColor = false;
            // 
            // btnMarkGhost
            // 
            this.btnMarkGhost.BackColor = System.Drawing.Color.Transparent;
            this.btnMarkGhost.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMarkGhost.Location = new System.Drawing.Point(97, 271);
            this.btnMarkGhost.Name = "btnMarkGhost";
            this.btnMarkGhost.Size = new System.Drawing.Size(141, 49);
            this.btnMarkGhost.TabIndex = 6;
            this.btnMarkGhost.Text = "Done";
            this.btnMarkGhost.UseVisualStyleBackColor = false;
            // 
            // frmLists
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDeleteGhost);
            this.Controls.Add(this.btnMarkGhost);
            this.Controls.Add(this.txtEdit);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMark);
            this.Name = "frmLists";
            this.Text = "frmLists";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnMark;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtEdit;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDeleteGhost;
        private System.Windows.Forms.Button btnMarkGhost;
    }
}