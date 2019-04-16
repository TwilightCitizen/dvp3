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
    public partial class frmUser : iPhonify.iPhone
    {
        private bool LoggedIn { get; set; } = false;

        public frmUser()
        {
            InitializeComponent();
        }

        private void SizeAndPositionMessage()
        {
            var logoBottom    = picLogo.Top + picLogo.Height;
            var clusterTop    = tblNav.Top;
            var between       = ( clusterTop - logoBottom ) / 2 + logoBottom;

            txtMessage.Height = txtMessage.CreateGraphics()
                                          .MeasureString( txtMessage.Text,
                                                          txtMessage.Font,
                                                          txtMessage.Width ).ToSize().Height+10;

            txtMessage.Top    = between - ( txtMessage.Height / 2 );
        }

        private void CredentialsChanged()
        {
            btnLogInOut.Enabled = LoggedIn || txtEmail.Text.Any() && txtPassword.Text.Any();
            btnRegister.Enabled =             txtEmail.Text.Any() && txtPassword.Text.Any();
        }

        private void Login()
        {
            LoggedIn            = true;
            btnLogInOut.Text    = "Logout";
            txtEmail.Enabled    =
            txtPassword.Enabled = false;
            btnLists.Enabled    = true;
            btnRegister.Enabled = false;

            ShowLists();
        }

        private void Logout()
        {
            LoggedIn            = false;
            btnLogInOut.Text    = "Login";
            txtEmail.Text       =
            txtPassword.Text    = default;
            txtEmail.Enabled    =
            txtPassword.Enabled = true;
            btnLogInOut.Enabled = false;
            btnLists.Enabled    = false;
        }

        private void Register()
        {
            txtEmail.Text       =
            txtPassword.Text    = default;
            txtEmail.Enabled    =
            txtPassword.Enabled = true;
            btnLogInOut.Enabled = false;
            btnRegister.Enabled = false;
        }

        private void ShowLists()
        {
            var next = new frmLists();

            Hide();
            next.ShowDialog( this );
            Show();
        }

        private void BtnLists_Click( object sender, EventArgs e )
        {
            ShowLists();
        }

        private void TxtEmail_TextChanged( object sender, EventArgs e )
        {
            CredentialsChanged();
        }

        private void TxtPassword_TextChanged( object sender, EventArgs e )
        {
            CredentialsChanged();
        }

        private void BtnLogInOut_Click( object sender, EventArgs e )
        {
            if (LoggedIn) Logout(); else Login();
        }

        private void BtnQuit_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void BtnRegister_Click( object sender, EventArgs e )
        {
            Register();
        }

        private void FrmUser_Load( object sender, EventArgs e )
        {
            SizeAndPositionMessage();
        }
    }
}
