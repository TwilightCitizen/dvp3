/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Event Handlers
 * Date:     April 5, 2019 */

using System;
using System.Drawing;
using System.Windows.Forms;

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
    }
}
