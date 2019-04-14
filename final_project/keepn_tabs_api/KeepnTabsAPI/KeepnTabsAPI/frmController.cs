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

        private const string Path  =
            "C:\\VFW\\connect.txt",
            User     = "dbsAdmin",
            Password = "password",
            Database = "keepntabs",
            Port     = "8889",
            SslMode  = "none";

        private MySqlConnection Connection;

        private MySqlCommand
            UserAddDB,    UserConfirmDB, UserLoginDB,  UserLogoutDB,
            UserUpdateDB, UserDeleteDB,  ListAddDB,    ListUpdateDB,
            ListDeleteDB, TaskAddDB,     TaskUpdateDB, TaskDeleteDB,
            UserListsDB,  ListTasksDB;

        private MySqlParameter
            UserIDDB, EmailDB,  PassDB, TokenIDDB, ListIDDB,
            TitleDB,  TaskIDDB, DoneDB, StringDB,  BoolDB;  

        /* Reply Shells */

        private XElement
            ReplyBase,   ReplyInvalid,   ReplySuccess,   ReplyFailure,
            ConfirmAll;

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
                { "user/add",     UserAdd     },   { "user/confirm", UserConfirm }
            ,   { "user/login",   UserLogin   },   { "user/logout",  UserLogout  }
            ,   { "user/update",  UserUpdate  },   { "user/delete",  UserDelete  }
            ,   { "list/add",     ListAdd     },   { "list/update",  ListUpdate  }
            ,   { "list/delete",  ListDelete  },   { "task/add",     TaskAdd     }
            ,   { "task/update",  TaskUpdate  },   { "task/delete",  TaskDelete  }
            ,   { "user/lists",   UserLists   },   { "list/tasks",   ListTasks  }
            };
        }

        /* Set up database connection, the commands, and their parameters. */

        private void InitializeDatabase()
        {
            Connection = new MySqlConnection( GetConnection() );

            UserAddDB     = new MySqlCommand( "UserAdd",     Connection );
            UserConfirmDB = new MySqlCommand( "UserConfirm", Connection );
            UserLoginDB   = new MySqlCommand( "UserLogin",   Connection );
            UserLogoutDB  = new MySqlCommand( "UserLogout",  Connection );
            UserUpdateDB  = new MySqlCommand( "UserUpdate",  Connection );
            UserDeleteDB  = new MySqlCommand( "UserDelete",  Connection );
            UserListsDB   = new MySqlCommand( "UserLists",   Connection );
            ListAddDB     = new MySqlCommand( "ListAdd",     Connection );
            ListUpdateDB  = new MySqlCommand( "ListUpdate",  Connection );
            ListDeleteDB  = new MySqlCommand( "ListDelete",  Connection );
            ListTasksDB   = new MySqlCommand( "ListTasks",   Connection );
            TaskAddDB     = new MySqlCommand( "TaskAdd",     Connection );
            TaskUpdateDB  = new MySqlCommand( "TaskUpdate",  Connection );
            TaskDeleteDB  = new MySqlCommand( "TaskDelete",  Connection );

            UserAddDB     .CommandType = UserConfirmDB .CommandType =  
            UserLoginDB   .CommandType = UserLogoutDB  .CommandType =  
            UserUpdateDB  .CommandType = UserDeleteDB  .CommandType =  
            ListAddDB     .CommandType = ListUpdateDB  .CommandType =  
            ListDeleteDB  .CommandType = TaskAddDB     .CommandType =  
            TaskUpdateDB  .CommandType = TaskDeleteDB  .CommandType =
            UserListsDB   .CommandType = ListTasksDB   .CommandType =
                CommandType.StoredProcedure;

            UserIDDB    = new MySqlParameter( "u",  MySqlDbType.String   );
            EmailDB     = new MySqlParameter( "e",  MySqlDbType.String   );
            PassDB      = new MySqlParameter( "p",  MySqlDbType.String   );
            TokenIDDB   = new MySqlParameter( "tk", MySqlDbType.String   );
            ListIDDB    = new MySqlParameter( "l",  MySqlDbType.String   );
            TitleDB     = new MySqlParameter( "t",  MySqlDbType.String   );
            TaskIDDB    = new MySqlParameter( "ti", MySqlDbType.String   );
            DoneDB      = new MySqlParameter( "d",  MySqlDbType.Bit      );
            StringDB    = new MySqlParameter( "r",  MySqlDbType.String   );
            BoolDB      = new MySqlParameter( "r",  MySqlDbType.Bit      );

            StringDB.Direction = ParameterDirection.ReturnValue;
            BoolDB.Direction   = ParameterDirection.ReturnValue;

            UserAddDB     .Parameters.Add( EmailDB   );
            UserAddDB     .Parameters.Add( PassDB    );
            UserAddDB     .Parameters.Add( StringDB  );

            UserConfirmDB .Parameters.Add( UserIDDB  );

            UserLoginDB   .Parameters.Add( EmailDB   );
            UserLoginDB   .Parameters.Add( PassDB    );
            UserLoginDB   .Parameters.Add( StringDB  );
                
            UserLogoutDB  .Parameters.Add( TokenIDDB );
                              
            UserUpdateDB  .Parameters.Add( TokenIDDB );
            UserUpdateDB  .Parameters.Add( EmailDB   );
            UserUpdateDB  .Parameters.Add( PassDB    );
            UserUpdateDB  .Parameters.Add( BoolDB    );

            UserDeleteDB  .Parameters.Add( TokenIDDB );
            UserDeleteDB  .Parameters.Add( BoolDB    );

            UserListsDB   .Parameters.Add( TokenIDDB );

            ListAddDB     .Parameters.Add( TokenIDDB );
            ListAddDB     .Parameters.Add( TitleDB   );
            ListAddDB     .Parameters.Add( StringDB  );

            ListTasksDB   .Parameters.Add( TokenIDDB );
            ListTasksDB   .Parameters.Add( ListIDDB  );

            ListUpdateDB  .Parameters.Add( TokenIDDB );
            ListUpdateDB  .Parameters.Add( ListIDDB  );
            ListUpdateDB  .Parameters.Add( TitleDB   );
            ListUpdateDB  .Parameters.Add( BoolDB    );

            ListDeleteDB  .Parameters.Add( TokenIDDB );
            ListDeleteDB  .Parameters.Add( ListIDDB  );
            ListDeleteDB  .Parameters.Add( BoolDB    );

            TaskAddDB     .Parameters.Add( TokenIDDB );
            TaskAddDB     .Parameters.Add( ListIDDB  );
            TaskAddDB     .Parameters.Add( TitleDB   );
            TaskAddDB     .Parameters.Add( DoneDB    );
            TaskAddDB     .Parameters.Add( StringDB  );

            TaskUpdateDB  .Parameters.Add( TokenIDDB );
            TaskUpdateDB  .Parameters.Add( TaskIDDB  );
            TaskUpdateDB  .Parameters.Add( TitleDB   );
            TaskUpdateDB  .Parameters.Add( DoneDB    );
            TaskUpdateDB  .Parameters.Add( BoolDB    );

            TaskDeleteDB  .Parameters.Add( TokenIDDB );
            TaskDeleteDB  .Parameters.Add( TaskIDDB  );
            TaskDeleteDB  .Parameters.Add( BoolDB    );
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
            ReplySuccess.Element( "status"  ).Add( new XElement( "success" ) );
            ReplyFailure.Element( "status"  ).Add( new XElement( "failure" ) );

            ConfirmAll = new XElement( "html"
            ,   new XElement( "head"
                ,   new XElement( "title"
                    , new XText("Welcome to Keep'n Tabs") ) )
            ,   new XElement( "body"
                ,   new XElement( "h1"
                    ,   new XText(  "Congratulations and welcome!  You're account is confirmed." ) )
                ,   new XElement( "h2"
                    ,   new XText( "Log into your Keep'n Tabs account from the app to get started." ) ) ) );
        }

        /* Specified Resource Handlers */

        private void UserAdd( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var email    = e.Request[ 0 ];
                var password = e.Request[ 1 ];

                UserAddDB.Parameters[ 0 ].Value = email;
                UserAddDB.Parameters[ 1 ].Value = password;

                Connection.Open();

                using( reader = UserAddDB.ExecuteReader() )
                { 
                    reader.Read();

                    var userid = reader[ 0 ].ToString();

                    XElement reply;

                    if( userid == "" )
                    { 
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "userexists" ) );
                    }
                    else
                    { 
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "useradded"
                            , new XElement( "userid", new XText( userid ) ) ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void UserConfirm( object sender, Exchange e )
        {
            try
            {
                var userid = e.Request[ 0 ];

                UserConfirmDB.Parameters[ 0 ].Value = userid;

                Connection.Open();
                UserConfirmDB.ExecuteNonQuery();

                var reply = new XElement( ConfirmAll );

                e.Reply = reply.ToString();
            }
            catch   {  Invalid( sender, e ); }
            finally {  Connection.Close(); }
        }

        private void UserLogin( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var email    = e.Request[ 0 ];
                var password = e.Request[ 1 ];

                UserLoginDB.Parameters[ 0 ].Value = email;
                UserLoginDB.Parameters[ 1 ].Value = password;

                Connection.Open();

                using( reader = UserLoginDB.ExecuteReader( ) )
                { 
                    reader.Read();

                    var token = reader[ 0 ].ToString();

                    XElement reply;

                    if( token == "" )
                    {
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }
                    else
                    {
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "userloggedin"
                            ,   new XElement( "token", new XText( token ) ) ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void UserLists( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token = e.Request[ 0 ];

                UserListsDB.Parameters[ 0 ].Value = token;

                Connection.Open();

                using( reader = UserListsDB.ExecuteReader( ) )
                { 
                    XElement reply;

                    reply = new XElement( ReplySuccess );

                    reply.Element("content").Add( new XElement( "lists" ) );

                    if ( reader.HasRows )
                        while( reader.Read() )
                            reply.Descendants( "lists" ).First().Add( new XElement( "list"
                            ,   new XText( reader[ 0 ].ToString() ) ) );

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void UserLogout( object sender, Exchange e )
        {
            try
            {
                var token = e.Request[ 0 ];

                UserLogoutDB.Parameters[ 0 ].Value = token;

                Connection.Open();
                UserLogoutDB.ExecuteNonQuery();

                var reply = new XElement( ReplySuccess );

                reply.Element( "content" ).Add( new XElement( "userloggedout" ) );

                e.Reply = reply.ToString();
            }
            catch   {  Invalid( sender, e ); }
            finally {  Connection.Close(); }
        }

        private void UserUpdate( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token    = e.Request[ 0 ];
                var email    = e.Request[ 1 ];
                var password = e.Request[ 2 ];

                UserUpdateDB.Parameters[ 0 ].Value = token;
                UserUpdateDB.Parameters[ 1 ].Value = email;
                UserUpdateDB.Parameters[ 2 ].Value = password;

                Connection.Open();

                using( reader = UserUpdateDB.ExecuteReader( ) )
                { 
                    reader.Read();

                    var updated = Convert.ToInt32( reader[ 0 ] ) == 1 ? true : false;

                    XElement reply;

                    if( updated )
                    {
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "userupdated" ) );
                    }
                    else
                    {
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void UserDelete( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token = e.Request[ 0 ];

                UserDeleteDB.Parameters[ 0 ].Value = token;

                Connection.Open();

                using( reader = UserDeleteDB.ExecuteReader( ) )
                { 
                    reader.Read();

                    var deleted = Convert.ToInt32( reader[ 0 ] ) == 1 ? true : false;

                    XElement reply;

                    if( deleted )
                    {
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "userdeleted" ) );
                    }
                    else
                    {
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void ListAdd( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token = e.Request[ 0 ];
                var title = e.Request[ 1 ];

                ListAddDB.Parameters[ 0 ].Value = token;
                ListAddDB.Parameters[ 1 ].Value = title;

                Connection.Open();

                using( reader = ListAddDB.ExecuteReader() )
                { 
                    reader.Read();

                    var listid = reader[ 0 ].ToString();

                    XElement reply;

                    if( listid == "" )
                    { 
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }
                    else
                    { 
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "listadded"
                            , new XElement( "listid", new XText( listid ) ) ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void ListTasks( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token  = e.Request[ 0 ];
                var listid = e.Request[ 1 ];

                ListTasksDB.Parameters[ 0 ].Value = token;
                ListTasksDB.Parameters[ 1 ].Value = listid;

                Connection.Open();

                using( reader = ListTasksDB.ExecuteReader( ) )
                { 
                    XElement reply;

                    reply = new XElement( ReplySuccess );

                    reply.Element("content").Add( new XElement( "tasks" ) );

                    if ( reader.HasRows )
                        while( reader.Read() )
                            reply.Descendants( "tasks" ).First().Add( new XElement( "task"
                            ,   new XText( reader[ 0 ].ToString() ) ) );

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void ListUpdate( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token  = e.Request[ 0 ];
                var listid = e.Request[ 1 ];
                var title  = e.Request[ 2 ];

                ListUpdateDB.Parameters[ 0 ].Value = token;
                ListUpdateDB.Parameters[ 1 ].Value = listid;
                ListUpdateDB.Parameters[ 2 ].Value = title;


                Connection.Open();

                using( reader = ListUpdateDB.ExecuteReader() )
                { 
                    reader.Read();

                    var updated = Convert.ToInt32( reader[ 0 ] ) == 1 ? true : false;

                    XElement reply;

                    if( updated )
                    { 
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "listupdated" ) );
                    }
                    else
                    { 
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void ListDelete( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token  = e.Request[ 0 ];
                var listid = e.Request[ 1 ];

                ListDeleteDB.Parameters[ 0 ].Value = token;
                ListDeleteDB.Parameters[ 1 ].Value = listid;

                Connection.Open();

                using( reader = ListDeleteDB.ExecuteReader( ) )
                { 
                    reader.Read();

                    var deleted = Convert.ToInt32( reader[ 0 ] ) == 1 ? true : false;

                    XElement reply;

                    if( deleted )
                    {
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "listdeleted" ) );
                    }
                    else
                    {
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void TaskAdd( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token  = e.Request[ 0 ];
                var listid = e.Request[ 1 ];
                var title  = e.Request[ 2 ];
                var done   = e.Request[ 3 ];

                TaskAddDB.Parameters[ 0 ].Value = token;
                TaskAddDB.Parameters[ 1 ].Value = listid;
                TaskAddDB.Parameters[ 2 ].Value = title;
                TaskAddDB.Parameters[ 3 ].Value = bool.Parse( done ) ? 1 : 0;

                Connection.Open();

                using( reader = TaskAddDB.ExecuteReader() )
                { 
                    reader.Read();

                    var taskid = reader[ 0 ].ToString();

                    XElement reply;

                    if( taskid == "" )
                    { 
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }
                    else
                    { 
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "taskadded"
                            , new XElement( "taskid", new XText( taskid ) ) ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch( Exception ex )   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void TaskUpdate( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token  = e.Request[ 0 ];
                var taskid = e.Request[ 1 ];
                var title  = e.Request[ 2 ];
                var done   = e.Request[ 3 ];

                TaskUpdateDB.Parameters[ 0 ].Value = token;
                TaskUpdateDB.Parameters[ 1 ].Value = taskid;
                TaskUpdateDB.Parameters[ 2 ].Value = title;
                TaskUpdateDB.Parameters[ 3 ].Value = bool.Parse( done ) ? 1 : 0;

                Connection.Open();

                using( reader = TaskUpdateDB.ExecuteReader() )
                { 
                    reader.Read();

                    var updated = Convert.ToInt32( reader[ 0 ] ) == 1 ? true : false;

                    XElement reply;

                    if( updated )
                    { 
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "taskupdated" ) );
                    }
                    else
                    { 
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
        }

        private void TaskDelete( object sender, Exchange e )
        {
            MySqlDataReader reader;

            try
            {
                var token  = e.Request[ 0 ];
                var taskid = e.Request[ 1 ];

                TaskDeleteDB.Parameters[ 0 ].Value = token;
                TaskDeleteDB.Parameters[ 1 ].Value = taskid;

                Connection.Open();

                using( reader = TaskDeleteDB.ExecuteReader( ) )
                { 
                    reader.Read();

                    var deleted = Convert.ToInt32( reader[ 0 ] ) == 1 ? true : false;

                    XElement reply;

                    if( deleted )
                    {
                        reply = new XElement( ReplySuccess );

                        reply.Element( "content" ).Add( new XElement( "taskdeleted" ) );
                    }
                    else
                    {
                        reply = new XElement( ReplyFailure );

                        reply.Element( "content" ).Add( new XElement( "tryagain" ) );
                    }

                    e.Reply = reply.ToString();
                }
            }
            catch   { Invalid( sender, e ); }
            finally { Connection.Close();   }
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
