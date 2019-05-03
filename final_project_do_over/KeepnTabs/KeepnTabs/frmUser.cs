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
using System.Net.Http;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace KeepnTabs
{
    public partial class frmUser : Form
    {
        private bool   LoggedIn = false;
        private bool   Updating = false;
        private string LoginToken;

        public frmUser()
        {
            InitializeComponent();
        }

        private void BtnLogInOutRegister_Click( object sender, EventArgs e )
        {
            if( LoggedIn ) LogOut(); else LogInRegister();
        }

        private void BtnQuit_Click( object sender, EventArgs e )
        {
            LogOut();
            Close();
        }

        private void BtnUpdateCommit_Click( object sender, EventArgs e )
        {
            if( Updating ) Commit(); else Update_();
        }

        private void BtnDelete_Click( object sender, EventArgs e )
        {
            Delete();
        }

        private void BtnLists_Click( object sender, EventArgs e )
        {
            Lists();
        }

        private void TxtEmail_TextChanged( object sender, EventArgs e )
        {
            CheckCredentials();
        }

        private void TxtPassword_TextChanged( object sender, EventArgs e )
        {
            CheckCredentials();
        }

        private void CheckCredentials()
        {
            btnLogInOutRegister.Enabled =
                txtPassword.Text.Any() &&
                new EmailAddressAttribute().IsValid( txtEmail.Text ) &&
                !LoggedIn;

            btnUpdateCommit.Enabled     = 
                txtPassword.Text.Any() && 
                new EmailAddressAttribute().IsValid( txtEmail.Text ) &&
                Updating;
        }

        private void LogOut()
        {
            txtEmail.Text            =
            txtPassword.Text         = "";
            txtEmail.Enabled         =
            txtPassword.Enabled      = true;
            btnDelete.Enabled        =
            btnLists.Enabled         = false;
            LoggedIn                 = false;
            btnLogInOutRegister.Text = "Login";
        }

        private async void LogInRegister()
        {
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"user/login/{ txtEmail.Text }/{ txtPassword.Text }" );

                    if( response.IsSuccessStatusCode )
                    {
                        LoginToken               = await response.Content.ReadAsStringAsync();
                        txtEmail.Enabled         = 
                        txtPassword.Enabled      = false;
                        btnDelete.Enabled        =
                        btnLists.Enabled         = true;
                        LoggedIn                 = true;
                        btnLogInOutRegister.Text = "Logout";
                    }
                } catch { }
            }
        }

        private void Update_()
        {
            txtEmail.Enabled    =
            txtPassword.Enabled = true;
            Updating            = true;
        }

        private void Commit()
        {
            txtEmail.Enabled    =
            txtPassword.Enabled = false;
            Updating = false;
        }

        private void Delete()
        {
            txtEmail.Text =
            txtPassword.Text = "";
            txtEmail.Enabled =
            txtPassword.Enabled = true;
            btnDelete.Enabled =
            btnLists.Enabled = false;
            LoggedIn = false;
            btnLogInOutRegister.Text = "Login";
        }

        private void Lists()
        {
            var frm = new frmLists();

            Hide();

            frm.ShowDialog( this );

            Show();
        }
    }
}
