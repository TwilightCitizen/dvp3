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
using System.Windows.Forms;
using System.Net.Http;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace KeepnTabsClient
{
    public partial class frmLists : Form // : iPhonify.iPhone
    {
        /* Token for the logged in user. */

        private string LoginToken { get; set; }

        /* Base API URL:  Get From File or Fail Gracefully with Blank */

        private string BaseApiUrl { get; set; }

        /* Constructors */

        public frmLists()
        {
            InitializeComponent();
        }

        public frmLists( string loginToken, string baseApiUrl )
        {
            InitializeComponent();

            LoginToken = loginToken; BaseApiUrl = baseApiUrl;
            
            TryGetLists();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void Item_SlideLeft( object sender, EventArgs e )
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            TryDelete( item );
        }

        private void Item_SlideRight( object sender, EventArgs e )
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            TryRename( item );
        }

        private void BtnAdd_Click( object sender, EventArgs e )
        {
            TryAdd();
        }

        private async void TryGetLists()
        {
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"user/lists/{ LoginToken }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                        foreach( XElement list in reply.Descendants( "list" ) )
                        {
                            var item         = new SlideItem.SlideItem( "Rename", "Delete", list.Element( "title" ).Value );

                            item.Tag         = list.Element( "id" ).Value;
                            item.SlideLeft  += Item_SlideLeft;
                            item.SlideRight += Item_SlideRight;

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
                        BaseApiUrl + $"list/delete/{ LoginToken }/{ item.Tag }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                            flowLayoutPanel.Controls.Remove( item );
                    }
                } catch { }
            }
        }

        private async void TryRename( SlideItem.SlideItem item )
        {
            var title = Interaction.InputBox(
                "To what title would you like to rename this list?  "
            +   "Leave it blank to cancel renaming the list."
            );

            if( !string.IsNullOrEmpty( title ) )
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"list/update/{ LoginToken }/{ item.Tag }/{ title }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                            item.Controls[ 1 ].Text = title;
                    }
                } catch { }
            }
        }

        private async void TryAdd()
        {
            var title = Interaction.InputBox(
                "What title would you like for this list?  "
            +   "Leave it blank to cancel adding the list."
            );

            if( !string.IsNullOrEmpty( title ) )
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"list/add/{ LoginToken }/{ title }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                        {
                            var item         = new SlideItem.SlideItem( "Rename", "Delete", title );

                            item.Tag         = reply.Descendants( "listid" ).FirstOrDefault().Value;
                            item.SlideLeft  += Item_SlideLeft;
                            item.SlideRight += Item_SlideRight;

                            flowLayoutPanel.Controls.Add( item );
                        }
                    }
                } catch { }
            }
        }
    }
}
