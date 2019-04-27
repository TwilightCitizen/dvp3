/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Client App
 * Synopsis: Client app for the Keep'n Tabs stack.
 * Date:     April 14, 2019 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace KeepnTabsClient
{
    public partial class frmTasks : Form // : iPhonify.iPhone
    {
        /* Token for the logged in user. */

        private string LoginToken { get; set; }

        /* Base API URL:  Get From File or Fail Gracefully with Blank */

        private string BaseApiUrl { get; set; }

        /* Elements of the Selected List */

        private string ListID     { get; set; }

        /* Constructors */

        public frmTasks()
        {
            InitializeComponent();
        }

        public frmTasks( string loginToken, string baseApiUrl, string listID, string listTitle )
        {
            InitializeComponent();

            LoginToken  = loginToken; BaseApiUrl   = baseApiUrl;
            ListID     = listID;      lblList.Text = listTitle;

            TryGetTasks();
        }

        /* Event Handlers */

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            TryAdd();
        }

        private void Item_SlideLeft( object sender, EventArgs e )
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            TryDelete( item );
        }

        private void Item_SlideRight( object sender, EventArgs e )
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            TryToggle( item );
        }

        private void Item_Click( object sender, EventArgs e)
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            TryRename( item );
        }

        /* Implementation Methods. */

        private async void TryGetTasks()
        {
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"list/tasks/{ LoginToken }/{ ListID }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                        foreach( XElement task in reply.Descendants( "task" ) )
                        {
                            var item         = new SlideItem.SlideItem(
                                "Toggle", "Delete"
                            ,   WebUtility.UrlDecode( task.Element( "title" ).Value )
                            );

                            item.Tag         = task.Element( "id" ).Value;
                            item.Click      += Item_Click;
                            item.SlideLeft  += Item_SlideLeft;
                            item.SlideRight += Item_SlideRight;

                            item.Controls[ "btnLeft" ].Tag = bool.Parse( task.Element( "done").Value );

                            if( bool.Parse( task.Element( "done").Value ) )
                                    item.Controls[ "btnMain" ].Font = new Font(
                                        item.Controls["btnMain"].Font, FontStyle.Strikeout );

                            flowLayoutPanel.Controls.Add( item );
                        }
                    }
                } catch { }
            }
        }

        private async void TryAdd()
        {
            var title = Interaction.InputBox(
                "What title would you like for this task?  "
            +   "Leave it blank to cancel adding the task."
            );

            if( !string.IsNullOrEmpty( title ) )
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"task/add/{ LoginToken }/{ ListID }/{ title }/false" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                        {
                            var item         = new SlideItem.SlideItem( "Toggle", "Delete", title );

                            item.Tag         = reply.Descendants( "taskid" ).FirstOrDefault().Value;
                            item.SlideLeft  += Item_SlideLeft;
                            item.SlideRight += Item_SlideRight;

                            item.Controls[ "btnLeft" ].Tag = false;

                            flowLayoutPanel.Controls.Add( item );
                        }
                    }
                } catch { }
            }
        }

        private async void TryDelete( SlideItem.SlideItem item )
        {
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"task/delete/{ LoginToken }/{ item.Tag }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                            flowLayoutPanel.Controls.Remove( item );
                    }
                } catch { }
            }
        }

        private async void TryToggle( SlideItem.SlideItem item )
        {
            using( var client = new HttpClient() )
            {
                var done = (bool) item.Controls[ "btnLeft" ].Tag;

                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"task/update/{ LoginToken }/{ item.Tag }/"
                                   + $"{ item.Controls[ "btnMain" ].Text }/"
                                   + $"{ ! done }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                        {
                            item.Controls[ "btnLeft" ].Tag = done = ! done;

                            item.Controls[ "btnMain" ].Font = new Font(
                                item.Controls[ "btnMain" ].Font
                            ,   done ? FontStyle.Strikeout : FontStyle.Regular );
                        }
                    }
                } catch { }
            }
        }

        private async void TryRename( SlideItem.SlideItem item )
        {
            var title = Interaction.InputBox(
                "To what title would you like to rename this task?  "
            +   "Leave it blank to cancel renaming the task."
            );

            if( !string.IsNullOrEmpty( title ) )
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"task/update/{ LoginToken }/{ item.Tag }/"
                                   + $"{ title }/"
                                   + $"{ item.Controls[ "btnLeft" ].Tag }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                            item.Controls[ "btnMain" ].Text = title;
                    }
                } catch { }
            }
        }
    }
}
