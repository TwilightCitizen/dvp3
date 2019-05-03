/* Name:     David A. Clark, Jr.
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
            switch( segs.Skip( 1 ).FirstOrDefault() )
            {
                case "login":  return UserLogin( segs.Skip( 1 ) );
                case "logout": return UserLogout( segs.Skip( 1 ) );
                case "update": return UserUpdate( segs.Skip( 1 ) );
                case "delete": return UserDelete( segs.Skip( 1 ) );
                default:       return Invalid();
            }
        }

        private Task<HttpResponseMessage> UserLogin( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> UserLogout( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> UserUpdate( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> UserDelete( IEnumerable< string > segs )
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

        private Task<HttpResponseMessage> ListAdd( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> ListUpdate( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> ListDelete( IEnumerable< string > segs )
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

        private Task<HttpResponseMessage> TaskAdd( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> TaskUpdate( IEnumerable< string > segs )
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }

        private Task<HttpResponseMessage> TaskDelete( IEnumerable< string > segs )
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
