/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */
 
 namespace KeepnTabs
{
    partial class frmUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUser));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogInOutRegister = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnLists = new System.Windows.Forms.Button();
            this.btnUpdateCommit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picLogo.BackgroundImage")));
            this.picLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picLogo.Location = new System.Drawing.Point(60, 122);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(209, 129);
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(60, 396);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(209, 20);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.TextChanged += new System.EventHandler(this.TxtEmail_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(60, 422);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(209, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // btnLogInOutRegister
            // 
            this.btnLogInOutRegister.Enabled = false;
            this.btnLogInOutRegister.Location = new System.Drawing.Point(60, 448);
            this.btnLogInOutRegister.Name = "btnLogInOutRegister";
            this.btnLogInOutRegister.Size = new System.Drawing.Size(105, 23);
            this.btnLogInOutRegister.TabIndex = 3;
            this.btnLogInOutRegister.Text = "Login";
            this.btnLogInOutRegister.UseVisualStyleBackColor = true;
            this.btnLogInOutRegister.Click += new System.EventHandler(this.BtnLogInOutRegister_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(167, 477);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(61, 477);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(104, 23);
            this.btnQuit.TabIndex = 5;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // btnLists
            // 
            this.btnLists.Enabled = false;
            this.btnLists.Location = new System.Drawing.Point(61, 506);
            this.btnLists.Name = "btnLists";
            this.btnLists.Size = new System.Drawing.Size(210, 23);
            this.btnLists.TabIndex = 7;
            this.btnLists.Text = "Lists ▶";
            this.btnLists.UseVisualStyleBackColor = true;
            this.btnLists.Click += new System.EventHandler(this.BtnLists_Click);
            // 
            // btnUpdateCommit
            // 
            this.btnUpdateCommit.Enabled = false;
            this.btnUpdateCommit.Location = new System.Drawing.Point(167, 448);
            this.btnUpdateCommit.Name = "btnUpdateCommit";
            this.btnUpdateCommit.Size = new System.Drawing.Size(104, 23);
            this.btnUpdateCommit.TabIndex = 8;
            this.btnUpdateCommit.Text = "Update";
            this.btnUpdateCommit.UseVisualStyleBackColor = true;
            this.btnUpdateCommit.Click += new System.EventHandler(this.BtnUpdateCommit_Click);
            // 
            // frmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(327, 636);
            this.Controls.Add(this.btnUpdateCommit);
            this.Controls.Add(this.btnLists);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnLogInOutRegister);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.picLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogInOutRegister;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnLists;
        private System.Windows.Forms.Button btnUpdateCommit;
    }
}

