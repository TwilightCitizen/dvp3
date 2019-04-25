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

namespace KeepnTabsClient
{
    public partial class frmLists : Form // : iPhonify.iPhone
    {
        public frmLists()
        {
            InitializeComponent();
            Test();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void Test()
        {
            for( var i = 0; i < 5; i++ )
            {
                var item = new SlideItem.SlideItem( "Mark", "Delete", "Item " + i.ToString() );

                item.SlideLeft  += Item_SlideLeft;
                item.SlideRight += Item_SlideRight;

                flowLayoutPanel.Controls.Add( item );
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
