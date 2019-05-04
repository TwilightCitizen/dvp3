﻿/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading;
using MySql.Data.MySqlClient;

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

            var segs = request.RequestUri.Segments.Skip( 1 ).Select( seg => seg.TrimEnd( '/' ).ToLower() );

            switch( segs.FirstOrDefault() )
            {
                case "user": return User( segs.Skip( 1 ) );
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
                case "login":  return UserLogin( segs.Skip( 1 ) );
                case "logout": return UserLogout( segs.Skip( 1 ) );
                case "update": return UserUpdate( segs.Skip( 1 ) );
                case "delete": return UserDelete( segs.Skip( 1 ) );
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

                            using( var cmdExist = new MySqlCommand( sqlExists, con ) )
                            {
                                cmdExist.Parameters.AddWithValue( "@Email", email );

                                var rdrExist = cmdExist.ExecuteReader();

                                if( rdrExist.HasRows )
                                {
                                    // Exists - Don't Login

                                    return Invalid();
                                }
                                else
                                {
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

                        var rdrMatch = cmdLogout.ExecuteReader();

                        if( rdrMatch.HasRows )
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
            }
            catch {  return Invalid(); }
        }

        private Task< HttpResponseMessage > UserUpdate( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task< HttpResponseMessage > UserDelete( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        /* Guard against invalid list actions, though we should have none, and route the request to
         * appropriate subordinate handlers with the subsequent URL segments. */
        private Task< HttpResponseMessage > List_( IEnumerable< string > segs )
        {
            switch( segs.Skip( 1 ).FirstOrDefault() )
            {
                case "add":    return ListAdd( segs.Skip( 1 ) );
                case "update": return ListUpdate( segs.Skip( 1 ) );
                case "delete": return ListDelete( segs.Skip( 1 ) );
                default:       return Invalid();
            }
        }

        private Task< HttpResponseMessage > ListAdd( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task< HttpResponseMessage > ListUpdate( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task< HttpResponseMessage > ListDelete( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        /* Guard against invalid task actions, though we should have none, and route the request to
         * appropriate subordinate handlers with the subsequent URL segments. */

        private Task< HttpResponseMessage > Task_( IEnumerable< string > segs )
        {
            switch( segs.Skip( 1 ).FirstOrDefault() )
            {
                case "add":    return TaskAdd( segs.Skip( 1 ) );
                case "update": return TaskUpdate( segs.Skip( 1 ) );
                case "delete": return TaskDelete( segs.Skip( 1 ) );
                default:       return Invalid();
            }
        }

        private Task< HttpResponseMessage > TaskAdd( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task< HttpResponseMessage > TaskUpdate( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task< HttpResponseMessage > TaskDelete( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }
    }
}
