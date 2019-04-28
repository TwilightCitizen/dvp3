/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Client App
 * Synopsis: Client app for the Keep'n Tabs stack.
 * Date:     April 14, 2019 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeepnTabsClient
{
    public partial class frmDelete : Form
    {
        /* Time to Wait to Confirm Deletion. */

        private int    Remaining { get; set; }

        /* What is Being Deleted. */

        private string Deleting  {  get; set; }

        /* Constructors */
        public frmDelete()
        {
            InitializeComponent();
        }

        public frmDelete( int holdFor, string deleting )
        {
            if( holdFor < 1 )
                throw new ArgumentException( "holdFor cannot be less than 1 second.", "holdFor" );

            if( string.IsNullOrEmpty( deleting  ) )
                throw new ArgumentException( "deleting cannot be null.", "deleting" );

            InitializeComponent();

            Remaining = holdFor;
            Deleting  = deleting;

            txtMessage.Text =
                $"This action will permanently delete { Deleting }, and it cannot be undone."
            +   Environment.NewLine + Environment.NewLine
            +   $"Are you sure you want to do this?  Hold yes to proceed.";
        }

        /* Close with No Result */

        private void BtnNo_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.No;
            Close();
        }

        /* Start Timer and Change Button to Time Remaining. */

        private void BtnYes_MouseDown( object sender, MouseEventArgs e )
        {
            tmrConfirm.Enabled = true;
            btnYes.Text        = Remaining.ToString();
        }

        /* Stop Timer, Reset Remaining, and Change Button to Yes. */

        private void BtnYes_MouseUp( object sender, MouseEventArgs e )
        {
            tmrConfirm.Enabled = false;
            btnYes.Text        = "Yes";
            Remaining            = 5;
        }

        /* Decrease Remaining, Update Button, and Close with Yes Result on 0. */

        private void TmrConfirm_Tick( object sender, EventArgs e )
        {
            Remaining--;
            btnYes.Text = Remaining.ToString();

            if( Remaining == 0 )
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        /* Simulate Screen Rotation. */

        private void BtnRotate_Click( object sender, EventArgs e )
        {
            SimulateRotation();
        }

        /* Simulate Rotating the Phone */

        private void SimulateRotation()
        {
            using( var bmp = new Bitmap( Width, Height  ) )
            {
                DrawToBitmap( bmp, new Rectangle( 0, 0, Width, Height ) );
                bmp.RotateFlip( RotateFlipType.Rotate270FlipNone );

                var sim             = new Form();

                sim.Width           = bmp.Width;
                sim.Height          = bmp.Height;
                sim.FormBorderStyle = FormBorderStyle.None;
                sim.BackgroundImage = bmp;
                sim.TransparencyKey = Color.Fuchsia;

                void Sim_Click( object sender, EventArgs e) => sim.Close();

                sim.Click           += Sim_Click;

                Hide();

                sim.ShowDialog( this );

                Show();
            }
        }
    }
}
