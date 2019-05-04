/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace KeepnTabs
{
    public partial class frmTasks : Form
    {
        /* Token for Logged in User */

        private string LoginToken;

        /* ID for the Selected List */

        private string ListID;

        /* Constructors */

        public frmTasks()
        {
            InitializeComponent();
        }

        public frmTasks( string loginToken, string listid )
        {
            InitializeComponent();

            LoginToken = loginToken;
            ListID     = listid;
            
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

        private async void Toggle()
        {

        }

        private async void Rename()
        {
            CheckSelection();
        }

        private async void Delete()
        {
            CheckSelection();
        }
    }
}
