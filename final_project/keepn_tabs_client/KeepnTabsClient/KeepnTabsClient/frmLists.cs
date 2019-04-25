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
            
            GetLists();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private async void GetLists()
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
                            var item         = new SlideItem.SlideItem( "Mark", "Delete", list.Element( "title" ).Value );

                            item.Tag         = list.Element( "id" );
                            item.SlideLeft  += Item_SlideLeft;
                            item.SlideRight += Item_SlideRight;

                            flowLayoutPanel.Controls.Add( item );
                        }
                    }
                }
                catch { }
            }
        }

        private void Item_SlideLeft( object sender, EventArgs e )
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            flowLayoutPanel.Controls.Remove( item );
        }

        private void Item_SlideRight( object sender, EventArgs e )
        {
            SlideItem.SlideItem item = (SlideItem.SlideItem) sender;

            item.LeftText = item.LeftText == "Mark" ? "Unmark" : "Mark"; 
        }
    }
}
