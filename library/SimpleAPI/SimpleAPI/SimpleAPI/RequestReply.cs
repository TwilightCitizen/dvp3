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
using System.Collections.Generic;
using System.Linq;

namespace SimpleAPI
{
    public class RequestReply : EventArgs
    {
        public List< string > Request { get; private set; }

        public string         Reply   { get; set; }

        public RequestReply( List< string > request )
        {
            Request = request;
        }

        public RequestReply( string[] request)
        {
            Request = request.ToList();
        }
    }
}
