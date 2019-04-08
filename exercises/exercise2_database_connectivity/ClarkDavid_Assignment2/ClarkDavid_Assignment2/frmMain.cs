/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Database Connectivity
 * Date:     April 8, 2019 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClarkDavid_Assignment2
{
    public partial class frmMain : Form
    {
        /* Form Movement Capture */

        private bool  CaptureMove { get; set; } = false;
        private Point Origin      { get; set; }

        /* Constructors */

        public frmMain()
        {
            InitializeComponent();
            ScaleToFitScreen( this );
        }

        /* Handlers to Support Movement of Borderless Form */

        /* Prepare to capture form movement. */

        private void frmMain_MouseDown( object sender, MouseEventArgs e )
        {
            CaptureMove = true;
            Origin      = e.Location;
        }

        /* Capture form movement. */

        private void frmMain_MouseMove( object sender, MouseEventArgs e )
        {
            if( CaptureMove ) Location =  new Point( Location.X - Origin.X + e.X
                                                   , Location.Y - Origin.Y + e.Y );
            Update();
        }

        /* Stop capturing form movement. */

        private void frmMain_MouseUp( object sender, MouseEventArgs e )
        {
            CaptureMove = false;
        }

        /* Scale the specified control and its constituents by the specified factor,
         * iterating through all decendent controls, scaling their font by the
         * specified factor, too.  The default factor is 75% if none is specified.
         * 
         * This works better than Keith Websters method for fixing the client window
         * size when dealing with displays at different resolutions and DPIs.  One
         * caveat is that display scaling on the Windows VM needs to be set to 100%,
         * or Visual Studio starts borking the proportions in the designer.
         * 
         * I am now running a UWQHD display at 3440x1440 resolution, so the
         * specified iPhone image and window sizes exceed the height of my screen,
         * making things look very clunky.  Calling this method after initialization
         * scales it down so it fits.  On the MacBook Pro's Retina Display at 2880x
         * 1800 resolution, the screen exceeds the bounds of the iPhone image and
         * window, so no scaling need occur. */

        private void ScaleToFitScreen( Control control, float factor = 0.75f )
        {
            if( Height > Screen.PrimaryScreen.WorkingArea.Size.Height )
            {
                Scale( new SizeF( factor, factor ) );
                
                Action< Control > ScaleChildren = null;
                
                ScaleChildren = ( Control parent ) =>
                {
                    foreach( Control child in parent.Controls)
                    {
                        Font font = child.Font;

                        child.Font = new Font( font.Name, font.SizeInPoints * factor * factor );

                        ScaleChildren( child );
                    }
                };

                ScaleChildren( control );
            }
        }
    }
}
