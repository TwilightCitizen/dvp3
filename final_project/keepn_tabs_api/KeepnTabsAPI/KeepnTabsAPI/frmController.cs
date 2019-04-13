/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Web API Server
 * Synopsis: A custom web API server for the Keep'n Tabs
 *           client app.  Leverages the SimpleAPI class
 *           library to help separate concerns.
 * Date:     April 12, 2019 */
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SimpleAPI;

namespace KeepnTabsAPI
{
    public partial class frmController : Form
    {
        /* The Keep'n Tabs API Server */

        private Server Server;

        /* Reply Shells */

        private XElement ReplyBase;
        private XElement ReplyInvalid;
        private XElement ReplySuccess;
        private XElement ReplyFailure;

        public frmController()
        {
            InitializeComponent();
            InitializeAPIServer();
            InitializeReplyShells();
        }

        /* Set up the Keep'n Tabs API Server */

        private void InitializeAPIServer()
        {
            Server          = new Server( "http://192.168.82.128:8080/todo/", Invalid );

            Server.Handlers = new Dictionary< string, Server.Handler >
            {
                { "user/add",     UserAdd }
            ,   { "user/confirm", UserConfirm }
            ,   { "user/update",  UserUpdate }
            ,   { "user/delete",  UserDelete }
            ,   { "list/add",     ListAdd }
            ,   { "list/update",  ListUpdate }
            ,   { "list/delete",  ListDelete }
            ,   { "task/add",     TaskAdd }
            ,   { "task/update",  TaskUpdate }
            ,   { "task/delete",  TaskDelete }
            };
        }

        /* Set up some reply shells. */
        private void InitializeReplyShells()
        {
            ReplyBase = new XElement( "reply",
                new XElement( "status" )
            ,   new XElement( "content" )
            );

            ReplyInvalid = new XElement( ReplyBase );
            ReplySuccess = new XElement( ReplyBase );
            ReplyFailure = new XElement( ReplyBase );

            ReplyInvalid.Element( "status"  ).Add( new XText( "invalid" ) );
            ReplyInvalid.Element( "content" ).Add( new XText( "The request received was invalid or not well formed." ) );
            ReplySuccess.Element( "status"  ).Add( new XText( "success" ) );
            ReplyFailure.Element( "status"  ).Add( new XText( "failure" ) );
            ReplyFailure.Element( "content" ).Add( new XText( "The request recieved was well formed but failed." ) );
        }

        /* Specified Resource Handlers */

        private void UserAdd( object sender, Exchange e )
        {
            try
            {
                var email    = e.Request[ 0 ];
                var password = e.Request[ 1 ];
                var reply    = new XElement( ReplySuccess );

                reply.Element( "content"  ).Add( new XElement( "email",    new XText( email ) ) );
                reply.Element( "content"  ).Add( new XElement( "password", new XText( password ) ) );

                e.Reply = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void UserConfirm( object sender, Exchange e )
        {
            e.Reply = "User Confirm";
        }

        private void UserUpdate( object sender, Exchange e )
        {
            e.Reply = "User Update";
        }

        private void UserDelete( object sender, Exchange e )
        {
            e.Reply = "User Delete";
        }

        private void ListAdd( object sender, Exchange e )
        {
            e.Reply = "List Add";
        }

        private void ListUpdate( object sender, Exchange e )
        {
            e.Reply = "List Update";
        }

        private void ListDelete( object sender, Exchange e )
        {
            e.Reply = "List Delete";
        }

        private void TaskAdd( object sender, Exchange e )
        {
            e.Reply = "Task Add";
        }

        private void TaskUpdate( object sender, Exchange e )
        {
            e.Reply = "Task Update";
        }

        private void TaskDelete( object sender, Exchange e )
        {
            e.Reply = "Task Delete";
        }

        private void Invalid( object sender, Exchange e )
        {
            var reply = new XElement( ReplyInvalid );

            e.Reply = reply.ToString();
        }

        /* Start/Stop the API Server */

        private void btnStartStop_Click( object sender, EventArgs e )
        {
            btnStartStop.Text = Server.IsRunning ? "&Start" : "&Stop";

            if( Server.IsRunning ) Server.Stop(); else Server.Start();
        }
    }
}
