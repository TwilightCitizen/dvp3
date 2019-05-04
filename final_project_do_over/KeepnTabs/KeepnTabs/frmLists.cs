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
    public partial class frmLists : Form
    {
        /* Token for Logged in User */

        private string LoginToken;

        /* Constructors */

        public frmLists()
        {
            InitializeComponent();
        }

        public frmLists( string loginToken )
        {
            InitializeComponent();

            LoginToken = loginToken;
            
            Lists();
        }

        /* Event Handlers */

        private void BtnAdd_Click( object sender, EventArgs e )
        {
            Add();
        }

        private void BtnRename_Click( object sender, EventArgs e )
        {
            Rename();
        }

        private void BtnDelete_Click( object sender, EventArgs e )
        {
            Delete();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void LstLists_SelectedIndexChanged( object sender, EventArgs e )
        {
            CheckSelection();
        }

        private void LstLists_DoubleClick( object sender, EventArgs e )
        {
            Tasks();
        }

        private void BtnTasks_Click( object sender, EventArgs e )
        {
            Tasks();
        }

        private void BtnRotate_Click( object sender, EventArgs e )
        {
            Program.SimulateRotation( this );
        }

        /* Tie renaming and deletion to selection state. */

        private void CheckSelection()
        {
            btnRename.Enabled =
            btnDelete.Enabled = 
            btnTasks.Enabled  = lstLists.SelectedItems.Count > 0;
        }

        /* Attempt to add a new list for the user, cancelling on
         * user cancel or empty list title. */

        private async void Add()
        {
            var title = Interaction.InputBox(
                "What title would you like for this list?  "
            +   "Leave it blank to cancel adding the list."
            );

            if( !string.IsNullOrEmpty( title ) )
            using ( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"list/add/{ LoginToken }/{ title }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var listid = response.Content.ReadAsStringAsync();
                        var toadd  = new ListViewItem( title );

                        toadd.Tag = listid;

                        lstLists.Items.Add( toadd );
                    }
                } catch { }
            }
        }

        /* Update the list title to the user's choice unless it's blank. */
        private async void Rename()
        {
            var title = Interaction.InputBox(
                "To what title would you like to rename this list?  "
            +   "Leave it blank to cancel renaming the list."
            );

            if( !string.IsNullOrEmpty( title ) )
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    var listid = lstLists.SelectedItems[ 0 ].Tag;

                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"list/update/{ LoginToken }/{ listid }/{ title }" );

                    if( response.IsSuccessStatusCode ) lstLists.SelectedItems[ 0 ].Text = title;
                    else MessageBox.Show( "There was an error updating your list." );
                } catch { }
            }

            CheckSelection();
        }

        /* Delete the list from the user's account on confirmation. */

        private async void Delete()
        {
            var confirm = MessageBox.Show( 
                "Are you sure you want to delete the list?" +
                "This will permanently remove it and all tasks in it.",
                "Whoa!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
            );

            if( confirm == DialogResult.Yes )
            {
                using( var client = new HttpClient( new FakeAPI() ) )
                {
                    try
                    {
                        var listid = lstLists.SelectedItems[ 0 ].Tag;

                        HttpResponseMessage response = await client.GetAsync( 
                            Program.ApiBase + $"list/delete/{ LoginToken }/{ listid }" );

                        if( response.IsSuccessStatusCode ) lstLists.SelectedItems[ 0 ].Remove();    
                        else MessageBox.Show( "There was an error deleting your list." );
                    } catch { }
                }
            }

            CheckSelection();
        }

        /* Get the lists for the logged in user from the API and add
         * them to the list view. We can expect an XML payload here
         * carrying the hiearchical list data. */

        private async void Lists()
        {
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"user/lists/{ LoginToken }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var lists = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        foreach( XElement list in lists.Descendants( "list" ) )
                        {
                            var toadd = new ListViewItem( WebUtility.UrlDecode( list.Element( "title" ).Value ) );

                            toadd.Tag = list.Element( "id" ).Value;

                            lstLists.Items.Add( toadd );
                        }
                    }
                } catch { }
            }
        }

        /* Navigate to the logged in user's tasks for the selected list. */
        
        private void Tasks()
        {
            var frm = new frmTasks( LoginToken, lstLists.SelectedItems[ 0 ].Tag.ToString() );

            Hide();

            frm.ShowDialog( this );

            Show();
        }
    }
}
