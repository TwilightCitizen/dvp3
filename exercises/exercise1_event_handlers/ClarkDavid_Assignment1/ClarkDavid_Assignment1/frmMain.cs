/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Event Handlers
 * Date:     April 5, 2019 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace ClarkDavid_Assignment1
{
    public partial class frmMain : Form
    {
        // Form Movement Capture

        private bool  CaptureMove { get; set; } = false;
        private Point Origin      { get; set; }

        // Constructor
        public frmMain()
        {
            InitializeComponent();
            FixClientWinSize();
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

        private void mnuExit_Click( object sender, EventArgs e )
        {
            Close();
        }

        // Display Add/Edit form for adding a course.  Added course will
        // arrive via custom event to registered handler.
        private void btnAdd_Click( object sender, EventArgs e )
        {
            Hide();

            var dlg                = new frmAddEdit();
            
            dlg.CourseAddedEdited += CourseAdded;

            dlg.ShowDialog( this );
            Show();
        }

        // Display Add/Edit form for editing a selected course.  The edited
        // course will arrive via a custom event to the registered handler.
        private void btnEdit_Click( object sender, EventArgs e )
        {
            Hide();

            var dlg                = new frmAddEdit();

            dlg.CourseAddedEdited += CourseEdited;
            dlg.CourseName         = lstPending.SelectedIndex != -1 ?
                                     lstPending.SelectedItem.ToString() :
                                     lstComplete.SelectedItem.ToString();

            dlg.ShowDialog( this );
            Show();
            DisableButtons();
        }

        // Add the received course to courses pending completion.  These
        // can be moved to completed via the swap button.
        private void CourseAdded( object sender, AddedEditedCourse e )
        {
            lstPending.Items.Add( e.Name );
        }

        // Either a pending or completed course is complete, but not both.
        // Update the selected one with the name returned from the dialog.
        private void CourseEdited( object sender, AddedEditedCourse e )
        {
            int index = -1;

            if( lstPending.SelectedIndex != -1 )
            {
                index = lstPending.SelectedIndex;

                lstPending.Items.RemoveAt( index );
                lstPending.Items.Insert( index, e.Name );
            }
            else
            {
                index = lstComplete.SelectedIndex;

                lstComplete.Items.RemoveAt( index );
                lstComplete.Items.Insert( index, e.Name );
            }
        }

        // The next two methods assure mutally exclusive item selection between
        // the pending and completed courses lists, also enabling appropriate
        // user actions on a selected course.

        private void lstPending_SelectedIndexChanged( object sender, EventArgs e )
        {
            lstComplete.SelectedIndex = -1;

            if( lstPending.SelectedIndex != -1 )
                EnableButtons();
            else
                DisableButtons();
        }

        private void lstComplete_SelectedIndexChanged( object sender, EventArgs e )
        {
            lstPending.SelectedIndex = -1;

            if( lstComplete.SelectedIndex != -1 )
                EnableButtons();
            else
                DisableButtons();
        }

        // Enable selected item user actions.
        private void EnableButtons()
        {
            btnEdit.Enabled   = true;
            btnSwap.Enabled   = true;
            btnDelete.Enabled = true;
        }

        // Disable selected item user actions.
        private void DisableButtons()
        {
            btnEdit.Enabled   = false;
            btnSwap.Enabled   = false;
            btnDelete.Enabled = false;
        }

        // Swap the selected item to the other list.
        private void btnSwap_Click( object sender, EventArgs e )
        {
            if( lstPending.SelectedIndex != -1 )
            { 
                lstComplete.Items.Add( lstPending.SelectedItem );
                lstPending.Items.Remove( lstPending.SelectedItem );
            }
            else
            {
                lstPending.Items.Add( lstComplete.SelectedItem );
                lstComplete.Items.Remove( lstComplete.SelectedItem );
            }

            DisableButtons();
        }

        // Remove the selected item.
        private void btnDelete_Click( object sender, EventArgs e )
        {
            if( lstPending.SelectedIndex != -1 )
                lstPending.Items.Remove( lstPending.SelectedItem );
            else
                lstComplete.Items.Remove( lstComplete.SelectedItem );

            DisableButtons();
        }

        // Save the lists to an XML file.  XML will be encoded to Base64,
        // prepended with MD5 sum to preserve file integrity.
        private void mnuSave_Click( object sender, EventArgs e )
        {
            var dlg = new SaveFileDialog();

            // Filter for Exercise 1 files.
            dlg.Filter = "EX1 Files (*.ex1)|*.ex1";

            if( dlg.ShowDialog() == DialogResult.OK )
            {
                using( StreamWriter sw = new StreamWriter( dlg.OpenFile() ) )
                {
                    // Convert the lists to XML.
                    var xml   = new XElement( "courses",
                        new XElement
                        (
                            "pending"
                        ,   from string course in lstPending.Items
                            select new XElement( "course", course )
                        )
                    ,   new XElement
                    ( 
                            "complete"
                        ,   from string course in lstComplete.Items
                            select new XElement( "course", course )
                        )
                    ).ToString();

                    // Convert XML to Base64.
                    var base64 = xml.ToBase64();
                    var md5    = base64.ToMD5();

                    /* Write the file. */
                    sw.Write( md5 );
                    sw.Write( base64 );
                }
            }
        }

        // Save the lists to a human-readable/printable text file.
        private void mnuPrint_Click( object sender, EventArgs e )
        {
            var dlg = new SaveFileDialog();

            // Filter for text files.
            dlg.Filter = "Text Files (*.txt)|*.txt";

            if( dlg.ShowDialog() == DialogResult.OK )
            {
                // Write the file.
                using( StreamWriter sw = new StreamWriter( dlg.OpenFile() ) )
                {
                    sw.WriteLine( "Pending:" + Environment.NewLine );

                    foreach( var course in lstPending.Items )
                        sw.WriteLine( " - " + course.ToString() );

                    sw.WriteLine( Environment.NewLine + "Complete:" + Environment.NewLine );

                    foreach( var course in lstComplete.Items )
                        sw.WriteLine( " - " + course.ToString() );
                }
            }
        }

        // Load an XML file to populate the lists.
        private void mnuLoad_Click( object sender, EventArgs e )
        {
            var dlg = new OpenFileDialog();

            // Filter for Exercise 1 files.
            dlg.Filter = "EX1 Files (*.ex1)|*.ex1";

            if( dlg.ShowDialog() == DialogResult.OK )
            {
                using( StreamReader sr = new StreamReader( dlg.OpenFile() ) )
                {
                    // Read in the file, checking the MD5 hash integrity.
                    var file   = sr.ReadToEnd();
                    var md5    = file.Substring( 0, 32 );
                    var base64 = file.Substring( 32 );
                    var text   = base64.FromBase64();

                    // Deserialize the XML, adding the courses to the lists
                    // or notifiy of an invalid file.
                    if( md5 == base64.ToMD5() )
                    {
                        try
                        {
                            var xml = XDocument.Parse( text );

                            lstPending.Items.Clear();
                            lstComplete.Items.Clear();

                            foreach( var course in xml.Descendants( "pending" ) )
                                lstPending.Items.Add( course.Value.ToString() );

                            foreach( var course in xml.Descendants( "complete" ) )
                                lstComplete.Items.Add( course.Value.ToString() );
                        }
                        catch
                        {
                            NotifyInvalidFile();
                        }
                    }
                    else
                        NotifyInvalidFile();
                }
            }
        }

        // Notify the user of an invalid file.
        private void NotifyInvalidFile()
        {
            MessageBox.Show
            ( 
                "File is not a valid Exercise 1 file or was corrupted!"
            ,   "Invalid File"
            ,   MessageBoxButtons.OK
            ,   MessageBoxIcon.Exclamation
            );
        }
    }
}
