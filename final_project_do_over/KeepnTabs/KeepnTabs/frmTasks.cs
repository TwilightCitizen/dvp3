/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.IO;

namespace KeepnTabs
{
    public partial class frmTasks : Form
    {
        /* Token for Logged in User */

        private string LoginToken;

        /* ID and Title for the Selected List */

        private string ListID;
        private string ListTitle;

        /* Constructors */

        public frmTasks()
        {
            InitializeComponent();
        }

        public frmTasks( string loginToken, string listid, string listTitle )
        {
            InitializeComponent();

            LoginToken    = loginToken;
            ListID        = listid;
            ListTitle     = 
            lblTasks.Text = listTitle;
            
            Tasks();
        }

        /* Event Handlers */

        private void BtnAdd_Click( object sender, EventArgs e )
        {
            Add();
        }

        private void BtnToggle_Click( object sender, EventArgs e )
        {
            Toggle();
        }

        private void BtnRename_Click( object sender, EventArgs e )
        {
            Rename();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void BtnDelete_Click( object sender, EventArgs e )
        {
            Delete();
        }

        private void LstTasks_SelectedIndexChanged( object sender, EventArgs e )
        {
            CheckSelection();
        }

        private void BtnExport_Click( object sender, EventArgs e )
        {
            Export();
        }

        private void BtnRotate_Click( object sender, EventArgs e )
        {
            Program.SimulateRotation( this );
        }

        private void CheckSelection()
        {
            btnRename.Enabled =
            btnDelete.Enabled =
            btnToggle.Enabled =
            btnExport.Enabled = lstTasks.SelectedItems.Count > 0;
        }

        /* Get the tasks for the logged in user for the selected list
         * from the API and add them to the list view. We can expect
         * an XML payload here carrying the hiearchical task data. */

        private async void Tasks()
        {
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"list/tasks/{ LoginToken }/{ ListID }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var tasks = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        foreach( XElement task in tasks.Descendants( "task" ) )
                        {
                            var toadd = new ListViewItem( WebUtility.UrlDecode( task.Element( "title" ).Value ) );

                            toadd.Tag = task.Element( "id" ).Value;

                            lstTasks.Items.Add( toadd );
                        }
                    }
                } catch { }
            }
        }

        /* Attempt to add a new task to the list for the user, cancelling on
         * user cancel or empty task title. */
        private async void Add()
        {
            var title = Interaction.InputBox(
                "What title would you like for this task?  "
            +   "Leave it blank to cancel adding the task."
            );

            if( !string.IsNullOrEmpty( title ) )
            using ( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"task/add/{ LoginToken }/{ ListID }/{ title }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var taskid = response.Content.ReadAsStringAsync();
                        var toadd  = new ListViewItem( title );

                        toadd.Tag = taskid;

                        lstTasks.Items.Add( toadd );
                    }
                } catch { }
            }
        }

        /* Attempt to toggle the complete/incomplete status of the task. */

        private async void Toggle()
        {
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                var task   = lstTasks.SelectedItems[ 0 ];
                var taskid = task.Tag;
                var title  = task.Text;
                var done   = !task.Font.Strikeout;

                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"task/update/{ LoginToken }/{ taskid }/{ title }/{ done }" );

                    if( response.IsSuccessStatusCode )
                        task.Font = new Font( task.Font, done ? FontStyle.Strikeout : FontStyle.Regular );
                } catch { }
            }

            CheckSelection();
        }

        /* Update the task title to the user's choice unless it's blank. */

        private async void Rename()
        {
            var title = Interaction.InputBox(
                "To what title would you like to rename this task?  "
            +   "Leave it blank to cancel renaming the list."
            );

            if( !string.IsNullOrEmpty( title ) )
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                var task   = lstTasks.SelectedItems[ 0 ];
                var taskid = task.Tag;
                var done   = !task.Font.Strikeout;

                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"task/update/{ LoginToken }/{ taskid }/{ title }/{ done }" );

                    if( response.IsSuccessStatusCode ) task.Text = title;
                } catch { }
            }

            CheckSelection();
        }

        /* Delete the selected task on user confirmation. */

        private async void Delete()
        {
            var confirm = MessageBox.Show( 
                "Are you sure you want to delete the task?",
                "Whoa!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
            );

            if( confirm == DialogResult.Yes )
            {
                using( var client = new HttpClient( new FakeAPI() ) )
                {
                    try
                    {
                        var taskid = lstTasks.SelectedItems[ 0 ].Tag;

                        HttpResponseMessage response = await client.GetAsync( 
                            Program.ApiBase + $"task/delete/{ LoginToken }/{ taskid }" );

                        if( response.IsSuccessStatusCode ) lstTasks.SelectedItems[ 0 ].Remove();    
                        else MessageBox.Show( "There was an error deleting your list." );
                    } catch { }
                }
            }

            CheckSelection();
        }

        /* Export the tasks of the current list to XML. */

        private void Export()
        {
            var frm = new SaveFileDialog();

            frm.Filter = "XML Files (*.xml)|*.xml";

            if( frm.ShowDialog() == DialogResult.OK )
            {
                using( StreamWriter sw = new StreamWriter( frm.OpenFile() ) )
                {
                    var xml = new XElement( "list",
                        new XElement( "title",
                            new XText( lblList.Text ) )
                    ,   new XElement( "tasks",
                            flowLayoutPanel.Controls.OfType< SlideItem.SlideItem >().ToList().Select( item =>
                                new XElement( "task",
                                    new XElement( "title",
                                        new XText( item.Controls[ "btnMain" ].Text ) )
                                ,   new XElement( "done",
                                        new XText( ( (bool) item.Controls[ "btnLeft" ].Tag ).ToString() ) ) ) ) ) );

                    /* Write the file. */
                    await sw.WriteAsync( xml.ToString() );
                }
            }
        }
    }
}
