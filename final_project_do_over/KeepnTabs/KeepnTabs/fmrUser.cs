using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

namespace KeepnTabs
{
    public partial class fmrUser : Form
    {
        private bool LoggedIn = false;

        public fmrUser()
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
            btnLogInOutRegister.Enabled = txtPassword.Text.Any() && new EmailAddressAttribute().IsValid( txtEmail.Text );
        }

        private void LogOut()
        {
            txtEmail.Text            =
            txtPassword.Text         = "";
            txtEmail.Enabled         =
            txtPassword.Enabled      = true;
            LoggedIn                 = false;
            btnLogInOutRegister.Text = "Login";
        }

        private void LogInRegister()
        {
            txtEmail.Enabled         = 
            txtPassword.Enabled      = false;
            LoggedIn                 = true;
            btnLogInOutRegister.Text = "Logout";
        }

        private void Delete()
        {
            txtEmail.Text            =
            txtPassword.Text         = "";
            txtEmail.Enabled         =
            txtPassword.Enabled      = true;
            LoggedIn                 = false;
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
