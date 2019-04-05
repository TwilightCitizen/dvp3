using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClarkDavid_Assignment1
{
    public partial class frmMain : Form
    {
        private bool  CaptureMove { get; set; } = false;
        private Point Origin      { get; set; }

        public frmMain()
        {
            InitializeComponent();
            FixClientWinSize();
        }

        private void frmMain_Load( object sender, EventArgs e )
        {

        }

        // The following 4 methods probably warrant subclassing.

        // More concise client size fixer. 
        private void FixClientWinSize()
        {
            Size  size   = Screen.PrimaryScreen.WorkingArea.Size;
            int   height = Convert.ToInt32( size.Height / 1.4f );
            int   width  = Convert.ToInt32( size.Width  / 6.0f  );

                  height = height < Size.Height ? Size.Height : height;
                  width  = width  < Size.Width  ? Size.Width  : width;
                  Size   = new Size( width, height );
        }

        // Prepare to capture form movement.
        private void frmMain_MouseDown( object sender, MouseEventArgs e )
        {
            CaptureMove = true;
            Origin      = e.Location;
        }

        // Capture form movement.
        private void frmMain_MouseMove( object sender, MouseEventArgs e )
        {
            if( CaptureMove ) Location =  new Point( Location.X - Origin.X + e.X
                                                   , Location.Y - Origin.Y + e.Y );
            Update();
        }

        // Stop capturing form movement.
        private void frmMain_MouseUp( object sender, MouseEventArgs e )
        {
            CaptureMove = false;
        }

        private void button1_Click( object sender, EventArgs e )
        {

        }

        private void mnuExit_Click( object sender, EventArgs e )
        {
            Close();
        }
    }
}
