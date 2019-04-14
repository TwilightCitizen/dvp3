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
using System.IO;
using SimpleAPI;
using MySql.Data.MySqlClient;

namespace KeepnTabsAPI
{
    public partial class frmController : Form
    {
        /* The Keep'n Tabs API Server */

        private Server Server;

        /* MySql Database Backing Store */

        private const string Path     = "C:\\VFW\\connect.txt";
        private const string User     = "dbsAdmin";
        private const string Password = "password";
        private const string Database = "keepntabs";
        private const string Port     = "8889";
        private const string SslMode  = "none";

        private MySqlConnection Connection;

        private MySqlCommand    UserAddDB;
        private MySqlCommand    UserConfirmDB;
        private MySqlCommand    UserLoginDB;
        private MySqlCommand    UserLogoutDB;
        private MySqlCommand    UserUpdateDB;
        private MySqlCommand    UserDeleteDB;
        private MySqlCommand    ListAddDB;
        private MySqlCommand    ListUpdateDB;
        private MySqlCommand    ListDeleteDB;
        private MySqlCommand    TaskAddDB;
        private MySqlCommand    TaskUpdateDB;
        private MySqlCommand    TaskDeleteDB;

        private MySqlParameter  UserIDDB;
        private MySqlParameter  EmailDB;
        private MySqlParameter  PassDB;
        private MySqlParameter  ConfirmedDB;
        private MySqlParameter  TokenIDDB;
        private MySqlParameter  ExpiresDB;
        private MySqlParameter  ListIDDB;
        private MySqlParameter  TitleDB;
        private MySqlParameter  TaskIDDB;
        private MySqlParameter  DoneDB;
        private MySqlParameter  StringDB;
        private MySqlParameter  BoolDB;  

        /* Reply Shells */

        private XElement ReplyBase;
        private XElement ReplyInvalid;
        private XElement ReplySuccess;
        private XElement ReplyFailure;

        public frmController()
        {
            InitializeComponent();
            InitializeAPIServer();
            InitializeDatabase();
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

        /* Set up database connection, the commands, and their parameters. */

        private void InitializeDatabase()
        {
            try
            {
                Connection = new MySqlConnection( GetConnection() );

                Connection.Open();

                UserAddDB     = new MySqlCommand( "useradd",     Connection );
                UserConfirmDB = new MySqlCommand( "UserConfirm", Connection );
                UserLoginDB   = new MySqlCommand( "UserLogin",   Connection );
                UserLogoutDB  = new MySqlCommand( "UserLogout",  Connection );
                UserUpdateDB  = new MySqlCommand( "UserUpdate",  Connection );
                UserDeleteDB  = new MySqlCommand( "UserDelete",  Connection );
                ListAddDB     = new MySqlCommand( "ListAdd",     Connection );
                ListUpdateDB  = new MySqlCommand( "ListUpdate",  Connection );
                ListDeleteDB  = new MySqlCommand( "ListDelete",  Connection );
                TaskAddDB     = new MySqlCommand( "TaskAdd",     Connection );
                TaskUpdateDB  = new MySqlCommand( "TaskUpdate",  Connection );
                TaskDeleteDB  = new MySqlCommand( "TaskDelete",  Connection );

                UserAddDB     .CommandType = CommandType.StoredProcedure;
                UserConfirmDB .CommandType = CommandType.StoredProcedure; 
                UserLoginDB   .CommandType = CommandType.StoredProcedure; 
                UserLogoutDB  .CommandType = CommandType.StoredProcedure; 
                UserUpdateDB  .CommandType = CommandType.StoredProcedure; 
                UserDeleteDB  .CommandType = CommandType.StoredProcedure; 
                ListAddDB     .CommandType = CommandType.StoredProcedure; 
                ListUpdateDB  .CommandType = CommandType.StoredProcedure; 
                ListDeleteDB  .CommandType = CommandType.StoredProcedure; 
                TaskAddDB     .CommandType = CommandType.StoredProcedure; 
                TaskUpdateDB  .CommandType = CommandType.StoredProcedure; 
                TaskDeleteDB  .CommandType = CommandType.StoredProcedure;

                UserIDDB    = new MySqlParameter( "@UserID",    MySqlDbType.String   );
                EmailDB     = new MySqlParameter( "e",     MySqlDbType.String   );
                PassDB      = new MySqlParameter( "p",  MySqlDbType.String   );
                ConfirmedDB = new MySqlParameter( "@Confirmed", MySqlDbType.Bit      );
                TokenIDDB   = new MySqlParameter( "@TokenID",   MySqlDbType.String   );
                ExpiresDB   = new MySqlParameter( "@Expires",   MySqlDbType.DateTime );
                ListIDDB    = new MySqlParameter( "@ListID",    MySqlDbType.String   );
                TitleDB     = new MySqlParameter( "@Title",     MySqlDbType.String   );
                TaskIDDB    = new MySqlParameter( "@TaskID",    MySqlDbType.String   );
                DoneDB      = new MySqlParameter( "@Done",      MySqlDbType.Bit      );
                StringDB    = new MySqlParameter( "r",          MySqlDbType.String   );
                BoolDB      = new MySqlParameter( "r",          MySqlDbType.Bit      );

                StringDB.Direction = ParameterDirection.ReturnValue;
                BoolDB.Direction   = ParameterDirection.ReturnValue;

                UserAddDB     .Parameters.Add( EmailDB   );
                UserAddDB     .Parameters.Add( PassDB    );
                UserAddDB     .Parameters.Add( StringDB  );

                UserConfirmDB .Parameters.Add( UserIDDB  );

                UserLoginDB   .Parameters.Add( EmailDB   );
                UserLoginDB   .Parameters.Add( PassDB    );
                
                UserLogoutDB  .Parameters.Add( TokenIDDB );
                              
                UserUpdateDB  .Parameters.Add( TokenIDDB );
                UserUpdateDB  .Parameters.Add( EmailDB   );
                UserUpdateDB  .Parameters.Add( PassDB    );
                              
                UserDeleteDB  .Parameters.Add( TokenIDDB );

                ListAddDB     .Parameters.Add( TokenIDDB );
                ListAddDB     .Parameters.Add( TitleDB   );

                ListUpdateDB  .Parameters.Add( TokenIDDB );
                ListUpdateDB  .Parameters.Add( ListIDDB  );
                ListUpdateDB  .Parameters.Add( TitleDB   );

                ListDeleteDB  .Parameters.Add( TokenIDDB );
                ListDeleteDB  .Parameters.Add( ListIDDB  );

                TaskAddDB     .Parameters.Add( TokenIDDB );
                TaskAddDB     .Parameters.Add( ListIDDB  );
                TaskAddDB     .Parameters.Add( TitleDB   );
                TaskAddDB     .Parameters.Add( DoneDB    );

                TaskUpdateDB  .Parameters.Add( TokenIDDB );
                TaskUpdateDB  .Parameters.Add( TaskIDDB  );
                TaskUpdateDB  .Parameters.Add( TitleDB   );
                TaskUpdateDB  .Parameters.Add( DoneDB    );

                TaskDeleteDB  .Parameters.Add( TokenIDDB );
                TaskDeleteDB  .Parameters.Add( TaskIDDB  );
            }
            catch
            {
                Connection.Dispose();
            }
        }

        /* Get MySql connection string. */

        private string GetConnection()
        {
            return string.Concat(
                $"server={ GetServer() };"  
            ,   $"userid={ User };"
            ,   $"password={ Password };"
            ,   $"database={ Database };"
            ,   $"port={ Port };"
            ,   $"sslmode={ SslMode }"
            );
        }

        /* Read server path from text file at hard coded path. */
        
        private string GetServer( )
        {
            try   { return File.ReadAllText( Path ); }
            catch { return null;                     }
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
        }

        /* Specified Resource Handlers */

        private void UserAdd( object sender, Exchange e )
        {
            try
            {
                var email                       = e.Request[ 0 ];
                var password                    = e.Request[ 1 ];

                UserAddDB.Parameters[ 0 ].Value = email;
                UserAddDB.Parameters[ 1 ].Value = password;

                var reader = UserAddDB.ExecuteReader( CommandBehavior.SingleResult );

                reader.Read();

                var userid                      = reader[ 0 ].ToString();
                var reply                       = new XElement( ReplySuccess );

                reader.Close();

                reply.Element( "content" ).Add( new XElement( "useradded"
                ,   new XElement( "userid", new XText( userid   ) )
                ) );

                e.Reply = reply.ToString();
            }
            catch( Exception ex )
            {
                Invalid( sender, e );
            }
        }

        private void UserConfirm( object sender, Exchange e )
        {
            try
            {
                var userID = e.Request[ 0 ];

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
                var title  = e.Request[ 1 ];

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
                var title     = e.Request[ 2 ];

                var reply    = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "listupdated"
                ,   new XElement( "title", new XText( title ) )
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
                var title  = e.Request[ 2 ];
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
                var title  = e.Request[ 2 ];
                var done   = e.Request[ 3 ];

                var reply  = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "taskupdated"
                ,   new XElement( "title", new XText( title ) )
                ,   new XElement( "done",  new XText( done  ) )
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
