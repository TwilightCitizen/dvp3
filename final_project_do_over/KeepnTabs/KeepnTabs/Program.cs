/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KeepnTabs
{
    public static class Program
    {
        /* Project File and Database Strings */

        public static readonly string
            ApiHost    = $"www.keepntabs.com",
            ApiBase    = $"http://{ ApiHost }/",
            DBPath     = $"C:\\VFW\\connect.txt",
            User       = $"dbsAdmin",
            Password   = $"password",
            Database   = $"keepntabs",
            Port       = $"8889",
            SslMode    = $"none",
            Connection = string.Concat(
                             $"server={ GetServer() };"  
                         ,   $"userid={ User };"
                         ,   $"password={ Password };"
                         ,   $"database={ Database };"
                         ,   $"port={ GetPort() };"
                         ,   $"sslmode={ SslMode }"
                         );

        /* Get the database server address from a hardcoded file. */
        public static string GetServer()
        {
            try   { return File.ReadAllLines( DBPath )[ 0 ]; }
            catch { return null;                             }
        }

        /* Get the database server port from a hardcoded file. */
        public static string GetPort()
        {
            try   { return File.ReadAllLines( DBPath )[ 1 ]; }
            catch { return Port;                             }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmUser());
        }
    }
}
