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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ClarkDavid_Assignment2
{
    public partial class dlgAddEdit : Form
    {
        /* Form Movement Capture */

        private bool  CaptureMove { get; set; } = false;
        private Point Origin      { get; set; }

        /* Series to Edit */

        private DataRow AddOrEdit { get; set; } = null;

        /* Constructors */

        public dlgAddEdit( DataRow addOrEdit )
        {
            InitializeComponent();
            ScaleToFitScreen( this );

            AddOrEdit = addOrEdit;
        }

        /* Handlers to Support Movement of Borderless Form */

        /* Prepare to capture form movement. */

        private void dlgAddEdit_MouseDown( object sender, MouseEventArgs e )
        {
            CaptureMove = true;
            Origin      = e.Location;
        }

        /* Capture form movement. */

        private void dlgAddEdit_MouseMove( object sender, MouseEventArgs e )
        {
            if( CaptureMove ) Location =  new Point( Location.X - Origin.X + e.X
                                                   , Location.Y - Origin.Y + e.Y );
            Update();
        }

        /* Stop capturing form movement. */

        private void dlgAddEdit_MouseUp( object sender, MouseEventArgs e )
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

        private void ScaleToFitScreen( Form form, float factor = 0.75f )
        {
            if( Height > Screen.PrimaryScreen.WorkingArea.Size.Height )
            {
                Scale( new SizeF( factor, factor ) );
                
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

                /* Menus do not constitute "controls".  They need to be scaled separately. */

                form.MainMenuStrip?.Items.OfType< ToolStripItem >().ToList().ForEach( item =>
                    item.Font = new Font( item.Font.Name, item.Font.SizeInPoints * factor * factor )
                );
            }
        }

        /* Populate the editor controls with the appropriate values from the
         * passed in data row. */

        private void PopulateControls()
        {
            txtTitle.Text     = AddOrEdit[ "title"     ].ToString();
            txtPublisher.Text = AddOrEdit[ "publisher" ].ToString();
            txtAuthor.Text    = AddOrEdit[ "author"    ].ToString();
            txtDirector.Text  = AddOrEdit[ "director"  ].ToString();
            txtGenre.Text     = AddOrEdit[ "genre"     ].ToString();

            nudYear.Value     = decimal.Parse( AddOrEdit[ "yearReleased" ].ToString() == "" ? "1901" : AddOrEdit[ "yearReleased" ].ToString() );

            try
            {
                /* On the chance that a series has a non-existent image,
                    * do not install it to the image list.  The list view
                    * item will index a non-existent image and show nothing. */ 

                var bytes  = (byte[]) AddOrEdit[ "icon" ];

                using( var stream = new MemoryStream( bytes ) ) picCover.Image = Image.FromStream( stream );
            }
            catch{ }
        }

        /* Update the data row with whatever values are in the editor controls. */

        private void SaveChanges()
        {
            try
            {
                AddOrEdit[ "title"        ] = txtTitle.Text;
                AddOrEdit[ "publisher"    ] = txtPublisher.Text;
                AddOrEdit[ "author"       ] = txtAuthor.Text;
                AddOrEdit[ "director"     ] = txtDirector.Text;
                AddOrEdit[ "genre"        ] = txtGenre.Text;
                AddOrEdit[ "yearReleased" ] = nudYear.Value;
            }
            catch( Exception e )
            {
                MessageBox.Show( e.Message );
            }
        }

        /* Present the user with a dialog to select a JPEG image for the series. */

        private void ChangeImage()
        {
            var dlg = new OpenFileDialog();

            dlg.Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg";

            if( dlg.ShowDialog() == DialogResult.OK )
            {
                byte[] bytes = File.ReadAllBytes( dlg.FileName );

                AddOrEdit[ "icon" ] = bytes;

                using( var stream = new MemoryStream( bytes ) ) picCover.Image = Image.FromStream( stream );
            }

            /* For whatever reason, the form wants to close here, which is not the
             * desired behavior, so hook a handler into form close to cancel it. */

            FormClosingEventHandler handler = null;

            handler = ( object sender, FormClosingEventArgs e ) =>
            {
                e.Cancel = true;

                this.FormClosing -= handler;
            };

            this.FormClosing += handler;
        }

        /* Form Event Handlers */

        private void dlgAddEdit_Load( object sender, EventArgs e )
        {
            PopulateControls();
            txtTitle_TextChanged( this, null );
        }

        private void btnSave_Click( object sender, EventArgs e )
        {
            SaveChanges();
            Close();
        }

        private void btnCancel_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void txtTitle_TextChanged( object sender, EventArgs e )
        {
            /* Prevent saving a series without a title.  All else can
             * be blank. */

            btnSave.Enabled = txtTitle.Text.Length > 0;
        }

        private void btnChange_Click( object sender, EventArgs e )
        {
            ChangeImage();
        }
    }
}
