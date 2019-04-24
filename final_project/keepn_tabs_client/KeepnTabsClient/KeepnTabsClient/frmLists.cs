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
    public partial class frmLists : iPhonify.iPhone
    {
        private bool  Slide  { get; set; } = false;

        private Point Origin { get; set; }

        private int   Mouse  { get; set; }

        public frmLists()
        {
            InitializeComponent();
        }

        private void BtnEdit_MouseDown( object sender, MouseEventArgs e )
        {
            Slide  = true;
            Origin = btnEdit.Location;
            Mouse  = e.X;
        }

        private void BtnEdit_MouseMove( object sender, MouseEventArgs e )
        {
            var withinLimitLeft  = btnEdit.Location.X >= btnMark.Location.X - btnMark.Width;
            var withinLimitRight = btnEdit.Location.X <= btnMark.Location.X + btnMark.Width;
            

            if ( Slide && withinLimitLeft && withinLimitRight)
            {
                btnEdit.Location     = new Point( btnEdit.Location.X + ( e.X - Mouse ), btnEdit.Location.Y );

                var beyondLimitLeft  = btnEdit.Location.X <= btnMark.Location.X - btnMark.Width;
                var beyondLimitRight = btnEdit.Location.X >= btnMark.Location.X + btnMark.Width;

                if ( beyondLimitLeft )
                {
                    btnEdit.Location = new Point( btnMark.Location.X - btnMark.Width, btnEdit.Location.Y );
                    MessageBox.Show( "Slid Left!"   );
                    BtnEdit_MouseUp( this, null );
                }

                if( beyondLimitRight )
                {
                    btnEdit.Location = new Point( btnMark.Location.X + btnMark.Width, btnEdit.Location.Y );
                    MessageBox.Show( "Slide Right!" );
                    BtnEdit_MouseUp( this, null );
                }
            }
        }

        private void BtnEdit_MouseUp( object sender, MouseEventArgs e )
        {
            Slide            = false;
            btnEdit.Location = Origin;
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void BtnEdit_Click( object sender, EventArgs e )
        {
            MessageBox.Show( "Clicked!" );
        }
    }
}
