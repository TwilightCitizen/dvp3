/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Subclass Form
 * Synopsis: Reduce some code and design duplication by creating an
 *           inheritable form that provided the desired appearance
 *           and base functionality up front.
 * Date:     April 11, 2019 */
 
 namespace iPhonify
{
    partial class iPhoneBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(iPhoneBase));
            this.SuspendLayout();
            // 
            // iPhoneBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(687, 1351);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "iPhoneBase";
            this.Text = "iPhone";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.iPhone_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.iPhone_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.iPhone_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}