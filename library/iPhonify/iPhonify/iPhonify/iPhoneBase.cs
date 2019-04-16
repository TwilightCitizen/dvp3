/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Subclass Form
 * Synopsis: Reduce some code and design duplication by creating an
 *           inheritable form that provided the desired appearance
 *           and base functionality up front.
 * Date:     April 11, 2019 */

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iPhonify
{
    /* iPhoneBase will be inherited by iPhone, which will point to iPhoneBase's
     * client size as a readonly property.  This will prevent the designer from
     * overriding the inherited form's size with the defaults. */

    public partial class iPhoneBase : Form
    {
        /* Signal whether to translate mouse movement into form
         * movement or not. */
        
        private bool  MoveForm { get; set; } = false;

        /* Track the form's screen position. */

        private Point Origin   { get; set; }

        /* Constructor */

        /* Prevent inheritance external to this library. */

        internal iPhoneBase()
        {
            InitializeComponent();
        }

        /* Scale the specified control and its constituents by the specified percentage
         * of the screen's size, iterating through all decendent controls, scaling their
         * font by the same factor, too.  The default factor is 75% if none is specified.
         * 
         * This works better than Keith Websters method for fixing the client window
         * size when dealing with displays at different resolutions and DPIs.  One
         * caveat is that display scaling on the Windows VM needs to be set to 100%,
         * or Visual Studio starts borking the proportions in the designer.
         * 
         * This probably needs some additional eyes on it. */

        private void ScaleToFitScreen( Form form, float percentage = 0.75f )
        {
            /* Guard against scaling the form to same size or larger
             * which would be pointless. */

            if( percentage >= 1f ) throw new ArgumentException( "Value must be less than 1.0f.", "percentage" );

            /* Check the form's height against the available screen's height. */

            var screen = Screen.PrimaryScreen.WorkingArea.Size;

            if( Height > screen.Height )
            {
                /* Scale the form down. */

                var target = screen.Height * percentage;
                var factor = target / Height;

                Scale( new SizeF( factor, factor ) );
                
                /* Scale the form's child controls. */

                Action< Control >  ScaleChildren = null;

                ScaleChildren = ( Control parent ) =>
                {
                    foreach( Control child in parent.Controls )
                    {
                        child.Font = new Font( child.Font.Name, child.Font.SizeInPoints * factor * factor );

                        ScaleChildren( child );
                    }
                };

                ScaleChildren( form );

                /* Menus do not constitute "controls" so scale them separately. */

                MainMenuStrip?.Items.OfType< ToolStripItem >().ToList().ForEach( item =>
                    item.Font = new Font( item.Font.Name, item.Font.SizeInPoints * percentage * percentage )
                );
            }
        }

        /* Event Handlers for translating mouse movement to form movement. */

        /* Translate mouse movement to form movement. */
        
        private void iPhone_MouseDown( object sender, MouseEventArgs e )
        {
            MoveForm = true;
            Origin   = e.Location;
        }

        /* When translating mouse movement to form movement, move the form
         * equal distances from its origin that the mouse has moved. */

        private void iPhone_MouseMove( object sender, MouseEventArgs e )
        {
            if(  MoveForm )
            {
                Location =  new Point( Location.X - Origin.X + e.X
                                     , Location.Y - Origin.Y + e.Y );
                Update();
            }
        }

        /* Do not translate mouse movement to form movement. */

        private void iPhone_MouseUp( object sender, MouseEventArgs e )
        {
            MoveForm = false;
        }

        /* This took a bit to figure out.  Calling ScaleToFitScreen from the
         * constructor passing "this" does not work, and calling it from the
         * derived forms' constructors nearly defeats the point. */

        private void iPhoneBase_Load( object sender, EventArgs e )
        {
            ScaleToFitScreen( sender as Form );
        }
    }
}
