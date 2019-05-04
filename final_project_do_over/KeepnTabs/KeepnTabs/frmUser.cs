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
        /* Module Level State Flags */

        private bool   LoggedIn = false;
        private bool   Updating = false;

        /* Token for Logged in User */

        private string LoginToken;

        /* Constructors */

        public frmUser()
        {
            InitializeComponent();
        }

        /* Event Handlers */

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

        private void BtnRotate_Click( object sender, EventArgs e )
        {
            Program.SimulateRotation( this );
        }

        /* Tie the clickability of buttons to the contents of the text
         * boxes and the module level state. */

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

        /* Log the user out.  Even if this fails at the API end, we
         * do it on the client end for real.  The tokens expire anyway. */

        private async void LogOut()
        {
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"user/logout/{ LoginToken }" );
                } catch { }
            }

            txtEmail.Text            =
            txtPassword.Text         = "";
            txtEmail.Enabled         =
            txtPassword.Enabled      = true;
            btnDelete.Enabled        =
            btnLists.Enabled         =
            btnUpdateCommit.Enabled  =
            LoggedIn                 = false;
            btnLogInOutRegister.Text = "Login";
        }

        /* Log the user in, or register and log the user in if the credentials are
         * new.  If the email matches but the password doesn't, fail. */

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
                        btnUpdateCommit.Enabled  = true;
                        btnDelete.Enabled        =
                        btnLists.Enabled         = true;
                        LoggedIn                 = true;
                        btnLogInOutRegister.Text = "Logout";
                    }
                    else MessageBox.Show( "Check the username and password and try again." );
                } catch { }
            }
        }

        /* Let the user update the login credentials. */

        private void Update_()
        {
            txtEmail.Enabled     =
            txtPassword.Enabled  =
            Updating             = true;
            btnDelete.Enabled    = false;
            btnUpdateCommit.Text = "Done";
        }

        /* Attempt to commit the user's changed login credentials.  Won't fail unless
         * the email already exists and/or the user attempts to keep same email. This
         * is somewhat brittle and forceful. */

        private async void Commit()
        {
            using( var client = new HttpClient( new FakeAPI() ) )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        Program.ApiBase + $"user/update/{ LoginToken }/{ txtEmail.Text }/{ txtPassword.Text }" );

                    if( response.IsSuccessStatusCode )
                    {
                        txtEmail.Enabled     =
                        txtPassword.Enabled  =
                        Updating             = false;
                        btnDelete.Enabled    = true;
                        btnUpdateCommit.Text = "Update";
                    }
                    else MessageBox.Show( "Try again with another email.  That one is taken." );
                }
                catch { }
            }

        }

        /* Delete the user's account on confirmation. */

        private async void Delete()
        {
            var confirm = MessageBox.Show( 
                "Are you sure you want to delete the user account?" +
                "This will permanently remove everything in the account.",
                "Whoa!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
            );

            if( confirm == DialogResult.Yes )
            {
                using( var client = new HttpClient( new FakeAPI() ) )
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync( 
                            Program.ApiBase + $"user/delete/{ LoginToken }" );

                        if( response.IsSuccessStatusCode )
                        {
                            txtEmail.Text =
                            txtPassword.Text = "";
                            txtEmail.Enabled =
                            txtPassword.Enabled = true;
                            btnDelete.Enabled =
                            btnLists.Enabled =
                            LoggedIn = false;
                            btnLogInOutRegister.Text = "Login";
                        }
                        else MessageBox.Show( "There was an error deleting your account." );
                    } catch { }
                }
            }
        }

        /* Navigate to the logged in user's lists. */

        private void Lists()
        {
            var frm = new frmLists( LoginToken );

            Hide();

            frm.ShowDialog( this );

            Show();
        }
    }
}
