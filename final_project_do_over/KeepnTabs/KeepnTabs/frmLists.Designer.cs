﻿/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */
 
 namespace KeepnTabs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLists));
            this.lstLists = new System.Windows.Forms.ListView();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblLists = new System.Windows.Forms.Label();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnTasks = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstLists
            // 
            this.lstLists.HideSelection = false;
            this.lstLists.Location = new System.Drawing.Point(61, 126);
            this.lstLists.MultiSelect = false;
            this.lstLists.Name = "lstLists";
            this.lstLists.Size = new System.Drawing.Size(210, 305);
            this.lstLists.TabIndex = 9;
            this.lstLists.UseCompatibleStateImageBehavior = false;
            this.lstLists.View = System.Windows.Forms.View.List;
            this.lstLists.SelectedIndexChanged += new System.EventHandler(this.LstLists_SelectedIndexChanged);
            this.lstLists.DoubleClick += new System.EventHandler(this.LstLists_DoubleClick);
            // 
            // btnRename
            // 
            this.btnRename.Enabled = false;
            this.btnRename.Location = new System.Drawing.Point(167, 449);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(104, 23);
            this.btnRename.TabIndex = 13;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.BtnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(167, 478);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(61, 478);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(104, 23);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "◀ Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(61, 449);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // lblLists
            // 
            this.lblLists.AutoSize = true;
            this.lblLists.Location = new System.Drawing.Point(58, 110);
            this.lblLists.Name = "lblLists";
            this.lblLists.Size = new System.Drawing.Size(28, 13);
            this.lblLists.TabIndex = 15;
            this.lblLists.Text = "Lists";
            // 
            // btnRotate
            // 
            this.btnRotate.BackColor = System.Drawing.Color.Black;
            this.btnRotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotate.Location = new System.Drawing.Point(151, 585);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(27, 32);
            this.btnRotate.TabIndex = 16;
            this.btnRotate.UseVisualStyleBackColor = false;
            this.btnRotate.Click += new System.EventHandler(this.BtnRotate_Click);
            // 
            // btnTasks
            // 
            this.btnTasks.Enabled = false;
            this.btnTasks.Location = new System.Drawing.Point(61, 507);
            this.btnTasks.Name = "btnTasks";
            this.btnTasks.Size = new System.Drawing.Size(210, 23);
            this.btnTasks.TabIndex = 17;
            this.btnTasks.Text = "Tasks ▶";
            this.btnTasks.UseVisualStyleBackColor = true;
            this.btnTasks.Click += new System.EventHandler(this.BtnTasks_Click);
            // 
            // frmLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(327, 636);
            this.Controls.Add(this.btnTasks);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.lblLists);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lstLists);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLists";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmLists";
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView lstLists;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblLists;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnTasks;
    }
}