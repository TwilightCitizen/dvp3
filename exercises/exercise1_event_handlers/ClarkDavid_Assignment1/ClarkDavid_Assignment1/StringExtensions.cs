/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Event Handlers
 * Date:     April 5, 2019 */

using System;
using System.Text;
using System.Security.Cryptography;

namespace ClarkDavid_Assignment1
{
    public static class StringExtensions
    {
      
        // Return the MD5 hash of a string.
        public static string ToMD5( this string any )
        {
            MD5 md5                = MD5.Create();

            byte[]        inBytes  = Encoding.UTF8.GetBytes( any );
            byte[]        outBytes = md5.ComputeHash( inBytes );
            StringBuilder builder  = new StringBuilder();

            foreach( byte b in outBytes ) builder.Append( b.ToString( "X2" ) );

            return builder.ToString();
        }

        // Return the Base64 conversion of a string.
        public static string ToBase64( this string any )
        {
            return Convert.ToBase64String( Encoding.UTF8.GetBytes( any ) );
        }

        // Return the normal conversion of a Base64 string.
        public static string FromBase64( this string any )
        {
            return Encoding.UTF8.GetString( Convert.FromBase64String( any ) );
        }
    }
}
