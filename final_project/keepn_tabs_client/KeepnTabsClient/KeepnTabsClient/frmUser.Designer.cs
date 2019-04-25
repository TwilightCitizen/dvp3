/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Client App
 * Synopsis: Client app for the Keep'n Tabs stack.
 * Date:     April 14, 2019 */

namespace KeepnTabsClient
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
            this.tblNav = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRegUpdate = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnLists = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogInOut = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tblNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tblNav
            // 
            this.tblNav.BackColor = System.Drawing.Color.Transparent;
            this.tblNav.ColumnCount = 2;
            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblNav.Controls.Add(this.btnDelete, 1, 3);
            this.tblNav.Controls.Add(this.btnRegUpdate, 1, 2);
            this.tblNav.Controls.Add(this.btnQuit, 0, 3);
            this.tblNav.Controls.Add(this.btnLists, 0, 4);
            this.tblNav.Controls.Add(this.txtPassword, 0, 1);
            this.tblNav.Controls.Add(this.btnLogInOut, 0, 2);
            this.tblNav.Controls.Add(this.txtEmail, 0, 0);
            this.tblNav.Location = new System.Drawing.Point(139, 775);
            this.tblNav.Name = "tblNav";
            this.tblNav.RowCount = 5;
            this.tblNav.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblNav.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblNav.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblNav.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblNav.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblNav.Size = new System.Drawing.Size(421, 281);
            this.tblNav.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(213, 171);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(205, 50);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnRegUpdate
            // 
            this.btnRegUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRegUpdate.Enabled = false;
            this.btnRegUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegUpdate.Location = new System.Drawing.Point(213, 115);
            this.btnRegUpdate.Name = "btnRegUpdate";
            this.btnRegUpdate.Size = new System.Drawing.Size(205, 50);
            this.btnRegUpdate.TabIndex = 6;
            this.btnRegUpdate.Text = "Register";
            this.btnRegUpdate.UseVisualStyleBackColor = true;
            this.btnRegUpdate.Click += new System.EventHandler(this.BtnRegUpdate_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(3, 171);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(204, 50);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // btnLists
            // 
            this.tblNav.SetColumnSpan(this.btnLists, 2);
            this.btnLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLists.Enabled = false;
            this.btnLists.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLists.Location = new System.Drawing.Point(3, 227);
            this.btnLists.Name = "btnLists";
            this.btnLists.Size = new System.Drawing.Size(415, 51);
            this.btnLists.TabIndex = 5;
            this.btnLists.Text = "Go to My Lists ▶";
            this.btnLists.UseVisualStyleBackColor = true;
            this.btnLists.Click += new System.EventHandler(this.BtnLists_Click);
            // 
            // txtPassword
            // 
            this.tblNav.SetColumnSpan(this.txtPassword, 2);
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(3, 59);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.Size = new System.Drawing.Size(415, 40);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // btnLogInOut
            // 
            this.btnLogInOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogInOut.Enabled = false;
            this.btnLogInOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogInOut.Location = new System.Drawing.Point(3, 115);
            this.btnLogInOut.Name = "btnLogInOut";
            this.btnLogInOut.Size = new System.Drawing.Size(204, 50);
            this.btnLogInOut.TabIndex = 1;
            this.btnLogInOut.Text = "Log In";
            this.btnLogInOut.UseVisualStyleBackColor = true;
            this.btnLogInOut.Click += new System.EventHandler(this.BtnLogInOut_Click);
            // 
            // txtEmail
            // 
            this.tblNav.SetColumnSpan(this.txtEmail, 2);
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(3, 3);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(415, 40);
            this.txtEmail.TabIndex = 3;
            this.txtEmail.TextChanged += new System.EventHandler(this.TxtEmail_TextChanged);
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picLogo.BackgroundImage")));
            this.picLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picLogo.Location = new System.Drawing.Point(139, 245);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(421, 275);
            this.picLogo.TabIndex = 4;
            this.picLogo.TabStop = false;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(139, 584);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(417, 134);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.Text = "Login or register below.";
            this.txtMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMessage.TextChanged += new System.EventHandler(this.TxtMessage_TextChanged);
            // 
            // frmUser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(687, 1351);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.tblNav);
            this.Controls.Add(this.picLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.FrmUser_Load);
            this.tblNav.ResumeLayout(false);
            this.tblNav.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblNav;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogInOut;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnLists;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnRegUpdate;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnDelete;
    }
}

