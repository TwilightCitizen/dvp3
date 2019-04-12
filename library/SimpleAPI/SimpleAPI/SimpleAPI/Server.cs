/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: SimpleAPI Library
 * Synopsis: A custom web API is being built for the DVP3 Final
 *           project.  This API library will help abstract some
 *           of the functionality from the API Server Form itself.
 * Date:     April 12, 2019 */

using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleAPI
{
    public class Server
    {
        /* Source to Stop Async Listen Loop */

        private CancellationTokenSource Source;

        /* Events Fired at Server Start and Stop */

        public event EventHandler Started;
        public event EventHandler Stopped;

        /* Track Server's Running Status */

        public bool IsRunning { get; private set; }  = false;

        /* URL Prefix Server will Listen On */

        private string Prefix;

        /* Expose Prefix Listening On when Listening */

        public string ListeningOn
        {
            get { return IsRunning ? Prefix : null; }
        }

        /* Event Fired when Request Arrives at Listening Prefix */

        public event EventHandler< RequestReply > RequestArrived;

        /* Constructor */

        public Server( string prefix )
        {
            /* Prevent listening on an invalid prefix. */
            
            Uri parsed;

            if( !Uri.TryCreate( prefix, UriKind.Absolute, out parsed ) || parsed.Scheme != Uri.UriSchemeHttp )
                throw new ArgumentException( "Prefix must be a valid HTTP URL.", "prefix" );

            Prefix = parsed.ToString();
        }

        /* Start the Server. */

        public async void Start()
        {
            if( IsRunning ) return;

            IsRunning = true;
            Source    = new CancellationTokenSource();

            Started?.Invoke( this, null );

            await ListenAsync( Source.Token );
        }

        /* Stop the Server */

        public void Stop()
        {
            if( !IsRunning ) return;

            Source.Cancel();
            Stopped?.Invoke( this, null );

            IsRunning = false;
        }

        /* Listen on the prefix, asynchronously processing requests as they
         * arrive until the server is stopped.  Alert subscribers when an API
         * request has arrived with a list of the requested URL's segments
         * less any prefix segments.  If subscribers return a text reply to
         * the API request, pass it on for asynchronous processing. */

        private async Task ListenAsync( CancellationToken token )
        {
            using( var listener = new HttpListener() )
            {
                listener.Prefixes.Add( Prefix );
                listener.Start();

                await Task.Run( async () =>
                {
                    while( !token.IsCancellationRequested )
                    {
                        try
                        {
                            var task = listener.GetContextAsync();
                            
                            task.Wait( token );

                            var context = task.Result;
                            var prefix  = new Uri( listener.Prefixes.First() ).Segments;

                            var request = context.Request.Url.Segments.Except( prefix ).ToList().Select( segment =>
                                segment.Replace( "/", "" ) ).ToList();

                            var requestReply = new RequestReply( request );

                            RequestArrived?.Invoke( this, requestReply );

                            if( requestReply.Reply != null )
                                    await ProcessReplyAsync( context.Response, requestReply.Reply );
                        }
                        catch{ /* Cancellations Throw */ }
                    }
                } );

                listener.Stop();
            }
        }

        /* Process text replies to API requests asynchronously. */

        private async Task ProcessReplyAsync( HttpListenerResponse response, string reply )
        {
            await Task.Run( () =>
            {
                var buffer = Encoding.UTF8.GetBytes( reply );

                response.ContentLength64 = buffer.Length;

                using ( var output = response.OutputStream )
                    output.WriteAsync( buffer, 0, buffer.Length );
            } );
        }
    }
}
