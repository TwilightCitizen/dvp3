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
using System.IO;
using System.Net.Http;
using System.Xml.Linq;
using FluentStateMachine;

namespace KeepnTabsClient
{
    public partial class frmUser : iPhonify.iPhone
    {
        /* Base Server File Path */

        private static string ServerFilePath = @"C:\VFW\listen.txt";

        /* Base API URL:  Get From File or Fail Gracefully with Blank */

        private string BaseApiUrl;

        /* States and Triggers for User/Account State Machine */

        enum State   { loggedOut,       emailLogin,     passwordLogin
                     , canLogin,        loggingIn,      logginFailed
                     , loggedIn,        registering,    regFailed
                     , registered,      emailUpdate,    passwordUpdate
                     , canUpdate,       updating,       cannotUpdate     }
        enum Trigger { emailEntered,    emailBlanked,   passwordEntered
                     , passwordBlanked, inOutClicked,   regUpdateClicked
                     , listsClicked,    tokenReturned,  failureReturned
                     , successReturned                                   }
        
        /* User/Account State Machine */

        private StateMachine< State, Trigger > UserAccount;

        /* User/Account Login Token */

        private string LoginToken;

        public frmUser()
        {
            InitializeComponent();
            InitializeStateMachine();
            InitializeBaseApiUrl();
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

        private void BtnLists_Click( object sender, EventArgs e )
        {
            
        }

        /* Trigger Email Blanked/Provided */

        private void TxtEmail_TextChanged( object sender, EventArgs e )
        {
            UserAccount.Trigger( txtEmail.Text == ""    ? Trigger.emailBlanked    : Trigger.emailEntered    );
        }

        /* Trigger Password Blanked/Provided */
        private void TxtPassword_TextChanged( object sender, EventArgs e )
        {
            UserAccount.Trigger( txtPassword.Text == "" ? Trigger.passwordBlanked : Trigger.passwordEntered );
        }

        /* Trigger Login(out) Clicked */
        private void BtnLogInOut_Click( object sender, EventArgs e )
        {
            UserAccount.Trigger( Trigger.inOutClicked );
        }

        /* Halt the State Machine and Exit the Application */
        private void BtnQuit_Click( object sender, EventArgs e )
        {
            UserAccount.Stop();
            Close();
        }

        private void BtnRegUpdate_Click( object sender, EventArgs e )
        {
            UserAccount.Trigger( Trigger.regUpdateClicked );
        }

        private void FrmUser_Load( object sender, EventArgs e )
        {
            SizeAndPositionMessage();
        }

        /* Setup Appropriate States, Triggers, and Transitions for User/Account */
        private void InitializeStateMachine()
        {
            UserAccount = new StateMachine< State, Trigger >( State.loggedOut )

                .For( State.loggedOut      ).On( Trigger.emailEntered,     null,      State.emailLogin     )
                .For( State.loggedOut      ).On( Trigger.passwordEntered,  null,      State.passwordLogin  )
                .For( State.loggedOut      ).On( Trigger.emailBlanked,     null,      State.loggedOut      )
                .For( State.loggedOut      ).On( Trigger.passwordBlanked,  null,      State.loggedOut      )
                                                                                                           
                .For( State.emailLogin     ).On( Trigger.emailEntered,     null,      State.emailLogin     )
                .For( State.emailLogin     ).On( Trigger.passwordEntered,  null,      State.canLogin       )
                .For( State.emailLogin     ).On( Trigger.emailBlanked,     null,      State.loggedOut      )
                                                                                                           
                .For( State.passwordLogin  ).On( Trigger.passwordEntered,  null,      State.passwordLogin  )
                .For( State.passwordLogin  ).On( Trigger.emailEntered,     null,      State.canLogin       )
                .For( State.passwordLogin  ).On( Trigger.passwordBlanked,  null,      State.loggedOut      )
                                                                                                           
                .For( State.canLogin       ).On( Trigger.passwordEntered,  null,      State.canLogin       )
                .For( State.canLogin       ).On( Trigger.emailEntered,     null,      State.canLogin       )
                .For( State.canLogin       ).On( Trigger.passwordBlanked,  null,      State.emailLogin     )
                .For( State.canLogin       ).On( Trigger.emailBlanked,     null,      State.passwordLogin  )
                .For( State.canLogin       ).On( Trigger.inOutClicked,     TryLogin,  State.loggingIn      )
                                                                                                           
                .For( State.canLogin       ).OnEntry( CanLogin    )                                        
                .For( State.emailLogin     ).OnEntry( CannotLogin )                                        
                .For( State.passwordLogin  ).OnEntry( CannotLogin )                                        
                .For( State.loggedOut      ).OnEntry( CannotLogin )                                        
                                                                                                           
                .For( State.loggingIn      ).On( Trigger.tokenReturned,    OnLogin,   State.loggedIn       )
                .For( State.loggingIn      ).On( Trigger.failureReturned,  NoLogin,   State.canLogin       )
                                                                                                           
                .For( State.loggedIn       ).On( Trigger.inOutClicked,     OnLogout,  State.loggedOut      )
                .For( State.loggedIn       ).On( Trigger.regUpdateClicked, CanUpdate, State.canUpdate      )
                                                                                                           
                .For( State.cannotUpdate   ).On( Trigger.emailEntered,     null,      State.emailUpdate    )
                .For( State.cannotUpdate   ).On( Trigger.passwordEntered,  null,      State.passwordUpdate )
                .For( State.cannotUpdate   ).On( Trigger.emailBlanked,     null,      State.cannotUpdate   )
                .For( State.cannotUpdate   ).On( Trigger.passwordBlanked,  null,      State.cannotUpdate   )
                                                                                                           
                .For( State.emailUpdate    ).On( Trigger.emailEntered,     null,      State.emailUpdate    )
                .For( State.emailUpdate    ).On( Trigger.passwordEntered,  null,      State.canUpdate      )
                .For( State.emailUpdate    ).On( Trigger.emailBlanked,     null,      State.cannotUpdate   )
                                                                                                           
                .For( State.passwordUpdate ).On( Trigger.passwordEntered,  null,      State.passwordUpdate )
                .For( State.passwordUpdate ).On( Trigger.emailEntered,     null,      State.canUpdate      )
                .For( State.passwordUpdate ).On( Trigger.passwordBlanked,  null,      State.cannotUpdate   )
                                                                                                           
                .For( State.canUpdate      ).On( Trigger.emailEntered,     null,      State.canUpdate      )
                .For( State.canUpdate      ).On( Trigger.passwordEntered,  null,      State.canUpdate      )
                .For( State.canUpdate      ).On( Trigger.emailBlanked,     null,      State.passwordUpdate )
                .For( State.canUpdate      ).On( Trigger.passwordBlanked,  null,      State.emailUpdate    )
                .For( State.canUpdate      ).On( Trigger.regUpdateClicked, TryUpdate, State.updating       )
                                                                                                           
                .For( State.canUpdate      ).OnEntry( CanUpdate    )                                       
                .For( State.emailUpdate    ).OnEntry( CannotUpdate )                                       
                .For( State.passwordUpdate ).OnEntry( CannotUpdate )
                .For( State.cannotUpdate   ).OnEntry( CannotUpdate )
                                                                                                           
                .For( State.updating       ).On( Trigger.successReturned,  OnUpdate,  State.loggedIn       )
                .For( State.updating       ).On( Trigger.failureReturned,  NoUpdate,  State.canUpdate      );

            UserAccount.Start();
        }

        /* Initialize the Base API URL to Saved Server with Port and Prefix, Failing with Blank */
        private void InitializeBaseApiUrl()
        {
            try
            {
                BaseApiUrl = $"http://{ File.ReadAllText( ServerFilePath ).TrimEnd( '\n', '\r' ) }:8080/todo/";
            }
            catch
            {
                BaseApiUrl = "";
            }
        }

        /* Email and Password Provided */
        private void CanLogin()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                btnLogInOut.Enabled  = true;
                btnRegUpdate.Enabled = true;
            } ) );
        }

        /* Email, Password, or Both Not Provided */
        private void CannotLogin()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                btnLogInOut.Enabled  = false;
                btnRegUpdate.Enabled = false;
            } ) );
        }

        /* Try Logging the User In */
        private async void TryLogin()
        {
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"user/login/{ txtEmail.Text }/{ txtPassword.Text }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                        {
                            LoginToken = reply.Descendants( "token" ).FirstOrDefault().Value;

                            UserAccount.Trigger( Trigger.tokenReturned );
                        }
                        else UserAccount.Trigger( Trigger.failureReturned );
                    }
                }
                catch { UserAccount.Trigger( Trigger.failureReturned ); }
            }
        }

        /* Modify UI After Login */

        private void OnLogin()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                btnLogInOut.Text     = "Logout";
                btnRegUpdate.Text    = "Update";
                btnRegUpdate.Enabled = true;
                btnLists.Enabled     = true;
                txtEmail.Enabled     = false;
                txtPassword.Enabled  = false;
            } ) );
        }

        /* Notify of Failed Login */

        private void NoLogin()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                txtMessage.Text = "Login failed with those credentials.  "
                                + "Make sure they're correct and the account is verified.  "
                                + "Or, try registering a new account with those credentials.";
            } ) );
        }

        /* Logout Cannot Fail Since Tokens Expire */

        private async void OnLogout()
        {
            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"user/logout/{ LoginToken }" );
                }
                catch {  }

                Invoke( new Action( () =>
                {
                    txtMessage.Text      = "Login or register below.";
                    btnLogInOut.Text     = "Login";
                    btnRegUpdate.Text    = "Register";
                    btnRegUpdate.Enabled = false;
                    btnLists.Enabled     = false;
                    txtEmail.Enabled     = true;
                    txtEmail.Text        = "";
                    txtPassword.Enabled  = true;
                    txtPassword.Text     = "";
                } ) );
            }
        }

        /* Get the User's Credentials Update */

        private void CanUpdate()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                txtMessage.Text      = "Change your email and password below and click done.";
                txtEmail.Enabled     = true;
                txtPassword.Enabled  = true;
                btnLogInOut.Enabled  = false;
                btnLists.Enabled     = false;
                btnRegUpdate.Enabled = true;
                btnRegUpdate.Text    = "Done";
            } ) );
        }


        private void CannotUpdate()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                btnRegUpdate.Enabled = false;
            } ) );
        }

        /* Try Updating the User's Credentials */

        private async void TryUpdate()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                txtEmail.Enabled    = false;
                txtPassword.Enabled = false;
                btnLogInOut.Enabled = true;
                btnLists.Enabled    = true;
                btnRegUpdate.Text   = "Update";
            } ) );

            using( var client = new HttpClient() )
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync( 
                        BaseApiUrl + $"user/update/{ LoginToken }/{ txtEmail.Text }/{ txtPassword.Text }" );

                    if( response.IsSuccessStatusCode )
                    {
                        var reply = XDocument.Parse( await response.Content.ReadAsStringAsync() );

                        if( reply.Descendants( "success" ).Any() )
                            UserAccount.Trigger( Trigger.successReturned );
                        else
                            UserAccount.Trigger( Trigger.failureReturned );
                    }
                }
                catch { UserAccount.Trigger( Trigger.failureReturned ); }
            }
        }

        /* Notify of Updated Credentials */

        private void OnUpdate()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                txtMessage.Text = "Login credentials updated.  "
                                + "Use your new credentials at next login.";
            } ) );
        }

        /* Notify of Failed Credential Update */

        private void NoUpdate()
        {
            /* UI Changes Must Occur on UI Thread */

            Invoke( new Action( () =>
            {
                txtMessage.Text = "Could not update your credentials.  "
                                + "A user with that email already exists.";
            } ) );
        }
    }
}
