﻿/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

namespace KeepnTabs
{
    /* Fake API literally fakes an API server, directly accessing the database.  No need to listen or anything.
     * Don't even need Internet as long as the database is run locally. This gets passed to HttpClient as the
     * message handler, and send async is overridden to send the responses. */

    public class FakeAPI : HttpMessageHandler
    {
        /* Guard against a request to an invalid host, though we should have none, and route the request to
         * appropriate handlers based on the remaining URL segments. */
        protected override Task< HttpResponseMessage > SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            if( request.RequestUri.Host.ToLower() != Program.ApiHost ) return Invalid();

            var segs = request.RequestUri.Segments.Skip( 1 ).Select( seg => seg.TrimEnd( '/' ) );

            switch( segs.FirstOrDefault() )
            {
                case "user": return User(  segs.Skip( 1 ) );
                case "list": return List_( segs.Skip( 1 ) );
                case "task": return Task_( segs.Skip( 1 ) );
                default:     return Invalid();
            }
        }

        /* Send back an invalid request response. */

        private Task< HttpResponseMessage > Invalid()
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent( "Invalid" )
                }
            );
        }

        /* Guard against invalid user actions, though we should have none, and route the request to
         * appropriate subordinate handlers with the subsequent URL segments. */

        private Task< HttpResponseMessage > User( IEnumerable< string > segs )
        {
            switch( segs.FirstOrDefault() )
            {
                case "login":  return UserLogin(  segs.Skip( 1 ) );
                case "logout": return UserLogout( segs.Skip( 1 ) );
                case "update": return UserUpdate( segs.Skip( 1 ) );
                case "delete": return UserDelete( segs.Skip( 1 ) );
                case "lists":  return UserLists(  segs.Skip( 1 ) );
                default:       return Invalid();
            }
        }

        /* Attempt to login the user with the provided email and password.  Failing that, attempt
         * to first register the user, then login.  Failing that, the combination is invalid. */

        private Task< HttpResponseMessage > UserLogin( IEnumerable< string > segs )
        {
            var email       = segs.Take( 1 ).FirstOrDefault();
            var pass        = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var sqlMatches  = "select ID from Users where Email = @Email and Pass = @Pass";
            var sqlExists   = "select ID from Users where Email = @Email";
            var sqlLogin    = "insert into Tokens( ID, UserID, Expires ) values( @Token, @UserID, @Expires )";
            var sqlRegister = "insert into Users( ID, Email, Pass, Confirmed ) values( @UserID, @Email, @Pass, 1 )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdMatch = new MySqlCommand( sqlMatches, con ) )
                    {
                        cmdMatch.Parameters.AddWithValue( "@Email", email );
                        cmdMatch.Parameters.AddWithValue( "@Pass",  pass  );

                        var rdrMatch = cmdMatch.ExecuteReader();

                        if( rdrMatch.HasRows )
                        {
                            // Matches - Login

                            rdrMatch.Read();

                            var userid  = rdrMatch[ 0 ].ToString();
                            var token   = Guid.NewGuid().ToString();
                            var expires = DateTime.Now.AddMinutes( 30 );
                        
                            rdrMatch.Close();

                            using( var cmdLogin = new MySqlCommand( sqlLogin, con ) )
                            {
                                cmdLogin.Parameters.AddWithValue( "@Token",   token  );
                                cmdLogin.Parameters.AddWithValue( "@UserID",  userid  );
                                cmdLogin.Parameters.AddWithValue( "@Expires", expires );

                                var numLogin = cmdLogin.ExecuteNonQuery();

                                if( numLogin > 0 )
                                    return Task.FromResult(
                                        new HttpResponseMessage()
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Content = new StringContent( token )
                                        }
                                    );
                                else return Invalid();
                            }
                        }
                        else
                        {
                            rdrMatch.Close();

                            using( var cmdExists = new MySqlCommand( sqlExists, con ) )
                            {
                                cmdExists.Parameters.AddWithValue( "@Email", email );

                                var rdrExist = cmdExists.ExecuteReader();

                                if( rdrExist.HasRows )
                                {
                                    // Exists - Don't Login

                                    return Invalid();
                                }
                                else
                                {
                                    // Doesn't Exist - Register and Login

                                    rdrExist.Close();

                                    var userid = Guid.NewGuid().ToString();

                                    using( var cmdRegister = new MySqlCommand( sqlRegister, con ) )
                                    {
                                        cmdRegister.Parameters.AddWithValue( "@UserID", userid );
                                        cmdRegister.Parameters.AddWithValue( "@Email",  email  );
                                        cmdRegister.Parameters.AddWithValue( "@Pass",   pass   );

                                        var numRegister = cmdRegister.ExecuteNonQuery();

                                        if( numRegister > 0 )
                                        {
                                            var token   = Guid.NewGuid().ToString();
                                            var expires = DateTime.Now.AddMinutes( 30 );

                                            using( var cmdLogin = new MySqlCommand( sqlLogin, con ) )
                                            {
                                                cmdLogin.Parameters.AddWithValue( "@Token",   token  );
                                                cmdLogin.Parameters.AddWithValue( "@UserID",  userid  );
                                                cmdLogin.Parameters.AddWithValue( "@Expires", expires );

                                                var rdrLogin = cmdLogin.ExecuteNonQuery();

                                                if( rdrLogin > 0 )
                                                {
                                                    return Task.FromResult(
                                                        new HttpResponseMessage()
                                                        {
                                                            StatusCode = HttpStatusCode.OK,
                                                            Content = new StringContent( token )
                                                        }
                                                    );
                                                } else return Invalid();
                                            }
                                        } else  return Invalid();
                                    }
                                }
                            }
                        }
                    }
                }
            } catch { return Invalid(); }
        }

        /* Attempt to log the user out, returning ok on success and invalid on failure. */

        private Task< HttpResponseMessage > UserLogout( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var sqlLogout = "delete from Tokens where ID = @Token";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdLogout = new MySqlCommand( sqlLogout, con ) )
                    {
                        cmdLogout.Parameters.AddWithValue( "@Token", token );

                        var numLogout = cmdLogout.ExecuteNonQuery();

                        if( numLogout > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( "OK" )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Attempt to update the user's account with the provided email and password by
         * user ID matching login token.  If the email address already exists, fail with
         * invalid, otherwise return success. */

        private Task< HttpResponseMessage > UserUpdate( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var email     = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var pass      = segs.Skip( 2 ).Take( 1 ).FirstOrDefault();

            var sqlExists = "select ID from Users where Email = @Email";
            var sqlUpdate = "update Users set Email = @Email, Pass = @Pass where ID = ( select UserID from Tokens where ID = @Token )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdExists = new MySqlCommand( sqlExists, con ) )
                    {
                        cmdExists.Parameters.AddWithValue( "@Email", email );

                        var rdrExists = cmdExists.ExecuteReader();

                        if( rdrExists.HasRows )
                            return Invalid();
                        else
                        {
                            rdrExists.Close();

                            using( var cmdUpdate = new MySqlCommand( sqlUpdate, con ) )
                            {
                                cmdUpdate.Parameters.AddWithValue( "@Email", email );
                                cmdUpdate.Parameters.AddWithValue( "@Pass",  pass  );
                                cmdUpdate.Parameters.AddWithValue( "@Token", token );

                                var numUpdate = cmdUpdate.ExecuteNonQuery();

                                if( numUpdate > 0)
                                {
                                    return Task.FromResult(
                                        new HttpResponseMessage()
                                        {
                                            StatusCode = HttpStatusCode.OK,
                                            Content = new StringContent( "OK" )
                                        }
                                    );
                                } else return Invalid();
                            }
                        }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Attempt to delete the user by user ID associated with the login token,
         * returning okay on success and invalid on failure. */

        private Task< HttpResponseMessage > UserDelete( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var sqlDelete = "delete from Users where ID = ( select UserID from Tokens where ID = @Token )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdDelete = new MySqlCommand( sqlDelete, con ) )
                    {
                        cmdDelete.Parameters.AddWithValue( "@Token", token );

                        var numDelete = cmdDelete.ExecuteNonQuery();

                        if( numDelete > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( "OK" )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Attempt to return an XML payload of all the lists associated with the
         * user ID associated with the login token, returning it on success and
         * returning invalid on failure. */

        private Task< HttpResponseMessage > UserLists( IEnumerable< string > segs )
        {
            var token    = segs.Take( 1 ).FirstOrDefault();
            var sqlLists = "select ID, Title from Lists where UserID = ( select UserID from Tokens where ID = @Token )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdLists = new MySqlCommand( sqlLists, con ) ) 
                    {
                        cmdLists.Parameters.AddWithValue( "@Token", token );

                        var rdrLists = cmdLists.ExecuteReader();
                        var lists    = new XElement( "lists" );

                        while( rdrLists.Read() )
                        {
                            lists.Add( new XElement( "list",
                                new XElement( "id",    new XText( rdrLists[ "ID"    ].ToString() ) )
                            ,   new XElement( "title", new XText( rdrLists[ "Title" ].ToString() ) )
                            ) );
                        }

                        return Task.FromResult(
                            new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent( lists.ToString() )
                            }
                        );
                    }
                }
                
            } catch {  return Invalid(); }
        }

        /* Guard against invalid list actions, though we should have none, and route the request to
         * appropriate subordinate handlers with the subsequent URL segments. */
        private Task< HttpResponseMessage > List_( IEnumerable< string > segs )
        {
            switch( segs.FirstOrDefault() )
            {
                case "add":    return ListAdd(    segs.Skip( 1 ) );
                case "update": return ListUpdate( segs.Skip( 1 ) );
                case "delete": return ListDelete( segs.Skip( 1 ) );
                case "tasks":  return ListTasks(  segs.Skip( 1 ) );
                default:       return Invalid();
            }
        }

        /* Attempt to return an XML payload of all the lists associated with the
         * user ID associated with the login token, returning it on success and
         * returning invalid on failure. */

        private Task< HttpResponseMessage > ListTasks( IEnumerable< string > segs )
        {
            var token    = segs.Take( 1 ).FirstOrDefault();
            var listid   = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var sqlTasks = "select ID, Title, Done from Tasks where ListID = ( select ID from lists where ID = @ListID and UserID = ( select UserID from Tokens where ID = @Token ) )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdTasks = new MySqlCommand( sqlTasks, con ) ) 
                    {
                        cmdTasks.Parameters.AddWithValue( "@Token",  token  );
                        cmdTasks.Parameters.AddWithValue( "@ListID", listid );

                        var rdrTasks = cmdTasks.ExecuteReader();
                        var tasks    = new XElement( "tasks" );

                        while( rdrTasks.Read() )
                        {
                            tasks.Add( new XElement( "task",
                                new XElement( "id",    new XText( rdrTasks[ "ID"    ].ToString() ) )
                            ,   new XElement( "title", new XText( rdrTasks[ "Title" ].ToString() ) )
                            ,   new XElement( "done",  new XText( rdrTasks[ "Done"  ].ToString() ) )
                            ) );
                        }

                        return Task.FromResult(
                            new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent( tasks.ToString() )
                            }
                        );
                    }
                }
                
            } catch {  return Invalid(); }
        }

        /* Attempt to add a list with the given title to the user ID matching the login
         * token, returning invalid or success accordingly. */

        private Task< HttpResponseMessage > ListAdd( IEnumerable< string > segs )
        {
            var token  = segs.Take( 1 ).FirstOrDefault();
            var title  = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var sqlAdd = "insert into Lists( ID, Title, UserID ) values( @ListID, @Title, ( select UserID from Tokens where ID = @Token ) )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdAdd = new MySqlCommand( sqlAdd, con ) ) 
                    {
                        var listid = Guid.NewGuid().ToString();

                        cmdAdd.Parameters.AddWithValue( "@ListID", listid );
                        cmdAdd.Parameters.AddWithValue( "@Title",  title  );
                        cmdAdd.Parameters.AddWithValue( "@Token",  token );

                        var numAdd = cmdAdd.ExecuteNonQuery();

                        if( numAdd > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( listid )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Attempt to update the list matching the list ID and user ID matching the
         * login token with the give title, returning success or failure. */

        private Task< HttpResponseMessage > ListUpdate( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var listid    = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var title     = segs.Skip( 2 ).Take( 1 ).FirstOrDefault();
            var sqlUpdate = "update Lists set Title = @Title where ID = @ListID and UserID = ( select UserID from Tokens where ID = @Token )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdUpdate = new MySqlCommand( sqlUpdate, con ) ) 
                    {
                        cmdUpdate.Parameters.AddWithValue( "@ListID", listid );
                        cmdUpdate.Parameters.AddWithValue( "@Title",  title  );
                        cmdUpdate.Parameters.AddWithValue( "@Token",  token );

                        var numUpdate = cmdUpdate.ExecuteNonQuery();

                        if( numUpdate > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( "OK" )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
                
            } catch {  return Invalid(); }
        }

        /* Attempt to delete the list matchig the list ID where the user ID matches
         * the login token, returning success or invalid accordingly. */

        private Task< HttpResponseMessage > ListDelete( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var listid    = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var sqlDelete = "delete from Lists where ID = @ListID and UserID = ( select UserID from Tokens where ID = @Token )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdDelete = new MySqlCommand( sqlDelete, con ) ) 
                    {
                        cmdDelete.Parameters.AddWithValue( "@ListID", listid );
                        cmdDelete.Parameters.AddWithValue( "@Token",  token );

                        var numDelete = cmdDelete.ExecuteNonQuery();

                        if( numDelete > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( "OK" )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Guard against invalid task actions, though we should have none, and route the request to
         * appropriate subordinate handlers with the subsequent URL segments. */

        private Task< HttpResponseMessage > Task_( IEnumerable< string > segs )
        {
            switch( segs.FirstOrDefault() )
            {
                case "add":    return TaskAdd( segs.Skip( 1 ) );
                case "update": return TaskUpdate( segs.Skip( 1 ) );
                case "delete": return TaskDelete( segs.Skip( 1 ) );
                default:       return Invalid();
            }
        }

        /* Attempt to add the task to the list matching the list ID where it belongs to the user
         * matching the user ID associated with the login token, returning success or invalid. */

        private Task< HttpResponseMessage > TaskAdd( IEnumerable< string > segs )
        {
            var token  = segs.Take( 1 ).FirstOrDefault();
            var listid = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var title  = segs.Skip( 2 ).Take( 1 ).FirstOrDefault();
            var sqlAdd = "insert into Tasks( ID, Title, Done, ListID ) values( @TaskID, @Title, 0, " +
                         "( select ID from Lists where ID = @ListID and UserID = ( select UserID from Tokens where ID = @Token ) ) )";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdAdd = new MySqlCommand( sqlAdd, con ) ) 
                    {
                        var taskid = Guid.NewGuid().ToString();

                        cmdAdd.Parameters.AddWithValue( "@TaskID", taskid );
                        cmdAdd.Parameters.AddWithValue( "@ListID", listid );
                        cmdAdd.Parameters.AddWithValue( "@Title",  title  );
                        cmdAdd.Parameters.AddWithValue( "@Token",  token  );

                        var numAdd = cmdAdd.ExecuteNonQuery();

                        if( numAdd > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( taskid )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Attempt to update the task matching the task ID with the title and status given where
         * it matches the list ID of the user ID of the login token, returning success or invalid. */

        private Task< HttpResponseMessage > TaskUpdate( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var taskid    = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var title     = segs.Skip( 2 ).Take( 1 ).FirstOrDefault();
            var done      = segs.Skip( 3 ).Take( 1 ).FirstOrDefault();
            var sqlUpdate = "update Tasks set Title = @Title, Done = @Done where ID = @TaskID";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdUpdate = new MySqlCommand( sqlUpdate, con ) ) 
                    {
                        cmdUpdate.Parameters.AddWithValue( "@TaskID", taskid                     );
                        cmdUpdate.Parameters.AddWithValue( "@Done",   bool.Parse( done ) ? 1 : 0 );
                        cmdUpdate.Parameters.AddWithValue( "@Title",  title                      );
                        //cmdUpdate.Parameters.AddWithValue( "@Token",  token                      );

                        var numUpdate = cmdUpdate.ExecuteNonQuery();

                        if( numUpdate > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( "OK" )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }

        /* Attempt to delete the task matching the task ID matching the list ID matching the user
         * ID of the login token, returning success or invalid. */

        private Task< HttpResponseMessage > TaskDelete( IEnumerable< string > segs )
        {
            var token     = segs.Take( 1 ).FirstOrDefault();
            var taskid    = segs.Skip( 1 ).Take( 1 ).FirstOrDefault();
            var sqlDelete = "delete from Tasks where ID = @TaskID";

            try
            {
                using( var con = new MySqlConnection( Program.Connection ) )
                {
                    con.Open();

                    using( var cmdDelete = new MySqlCommand( sqlDelete, con ) ) 
                    {
                        cmdDelete.Parameters.AddWithValue( "@TaskID", taskid );
                        //cmdDelete.Parameters.AddWithValue( "@Token",  token  );

                        var numDelete = cmdDelete.ExecuteNonQuery();

                        if( numDelete > 0 )
                            return Task.FromResult(
                                new HttpResponseMessage()
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Content = new StringContent( "OK" )
                                }
                            );
                        else { return Invalid(); }
                    }
                }
            } catch {  return Invalid(); }
        }
    }
}
