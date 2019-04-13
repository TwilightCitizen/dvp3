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
                { "user/add",     UserAdd     }
            ,   { "user/confirm", UserConfirm }
            ,   { "user/login",   UserLogin   }
            ,   { "user/logout",  UserLogout  }
            ,   { "user/update",  UserUpdate  } 
            ,   { "user/delete",  UserDelete  }
            ,   { "list/add",     ListAdd     }
            ,   { "list/update",  ListUpdate  }
            ,   { "list/delete",  ListDelete  }
            ,   { "task/add",     TaskAdd     }
            ,   { "task/update",  TaskUpdate  }
            ,   { "task/delete",  TaskDelete  }
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

            ReplyInvalid.Element( "status"  ).Add( new XElement( "invalid" ) );
            ReplyInvalid.Element( "content" ).Add( new XText( "The request received was invalid or not well formed." ) );
            ReplySuccess.Element( "status"  ).Add( new XElement( "success" ) );
            ReplyFailure.Element( "status"  ).Add( new XElement( "failure" ) );
            ReplyFailure.Element( "content" ).Add( new XText( "The request recieved was well formed but failed." ) );
        }

        /* Specified Resource Handlers */

        private void UserAdd( object sender, Exchange e )
        {
            try
            {
                var email    = e.Request[ 0 ];
                var password = e.Request[ 1 ];

                var userid   = "";

                var reply    = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "useradded"
                ,   new XElement( "userid", new XText( userid   ) )
                ) );

                e.Reply = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void UserConfirm( object sender, Exchange e )
        {
            try
            {
                var token = e.Request[ 0 ];

                var reply = new XElement( "html"
                ,   new XElement( "head"
                    ,   new XElement( "title"
                        ,   new XText( "Welcome to Keep'n Tabs" )
                    ) )
                ,   new XElement( "body"
                    ,   new XElement( "h1"
                        ,   new XText( 
                                "Congratulations and welcome!  You're account is confirmed."
                        ) )
                    ,   new XElement( "h2"
                        ,   new XText(
                                "Log into your Keep'n Tabs account from the app to get started."
                        ) )
                ) );

                e.Reply = reply.ToString();
            }
            catch{ }
        }

        private void UserLogin( object sender, Exchange e )
        {
            try
            {
                var email    = e.Request[ 0 ];
                var password = e.Request[ 1 ];

                var token    = "";
                var lists    = new List< string >();

                var reply    = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "userloggedin"
                ,   new XElement( "token", new XText( token ) )
                ,   new XElement( "lists", lists.Select( list =>
                        new XElement( "list", new XText( list ) )
                    ) )
                ) );

                e.Reply = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void UserLogout( object sender, Exchange e )
        {
            try
            {
                var token = e.Request[ 0 ];
                var reply = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "userloggedout" ) );

                e.Reply   = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void UserUpdate( object sender, Exchange e )
        {
            try
            {
                var token    = e.Request[ 0 ];
                var email    = e.Request[ 1 ];
                var password = e.Request[ 2 ];

                var reply    = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "userupdate"
                ,   new XElement( "email",    new XText( email    ) )
                ,   new XElement( "password", new XText( password ) )
                ) );

                e.Reply = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void UserDelete( object sender, Exchange e )
        {
            try
            {
                var token = e.Request[ 0 ];

                var reply = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "userdeleted" ) );

                e.Reply   = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void ListAdd( object sender, Exchange e )
        {
            try
            {
                var token  = e.Request[ 0 ];
                var name   = e.Request[ 1 ];

                var listid = "";

                var reply  = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "listadded"
                ,   new XElement( "listid",    new XText( listid ) )
                ) );

                e.Reply    = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void ListUpdate( object sender, Exchange e )
        {
            try
            {
                var token    = e.Request[ 0 ];
                var listid   = e.Request[ 1 ];
                var name     = e.Request[ 2 ];

                var reply    = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "listupdated"
                ,   new XElement( "name", new XText( name ) )
                ) );

                e.Reply = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void ListDelete( object sender, Exchange e )
        {
            try
            {
                var token  = e.Request[ 0 ];
                var listid = e.Request[ 1 ];

                var reply  = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "listdeleted" ) );

                e.Reply    = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void TaskAdd( object sender, Exchange e )
        {
            try
            {
                var token  = e.Request[ 0 ];
                var listid = e.Request[ 1 ];
                var text   = e.Request[ 2 ];
                var done   = e.Request[ 3 ];

                var taskid = "";

                var reply  = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "taskadded"
                ,   new XElement( "taskid", new XText( taskid ) )
                ) );

                e.Reply    = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void TaskUpdate( object sender, Exchange e )
        {
            try
            {
                var token  = e.Request[ 0 ];
                var taskid = e.Request[ 1 ];
                var text   = e.Request[ 2 ];
                var done   = e.Request[ 3 ];

                var reply  = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "taskupdated"
                ,   new XElement( "text", new XText( text ) )
                ,   new XElement( "done", new XText( done ) )
                ) );

                e.Reply    = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void TaskDelete( object sender, Exchange e )
        {
            try
            {
                var token  = e.Request[ 0 ];
                var taskid = e.Request[ 1 ];

                var reply  = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "taskdeleted" ) );

                e.Reply    = reply.ToString();
            }
            catch
            {
                Invalid( sender, e );
            }
        }

        private void Invalid( object sender, Exchange e )
        {
            var reply = new XElement( ReplyInvalid );

            e.Reply   = reply.ToString();
        }

        /* Start/Stop the API Server */

        private void btnStartStop_Click( object sender, EventArgs e )
        {
            btnStartStop.Text = Server.IsRunning ? "&Start" : "&Stop";

            if( Server.IsRunning ) Server.Stop(); else Server.Start();
        }
    }
}
