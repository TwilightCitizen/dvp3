/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Database Connectivity
 * Date:     April 8, 2019 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace ClarkDavid_Assignment2
{
    public partial class frmMain : Form
    {
        /* Form Movement Capture */

        private bool  CaptureMove { get; set; } = false;
        private Point Origin      { get; set; }

        /* Constants */

        private const string PATH     = "C:\\VFW\\connect.txt";

        private const string USER     = "dbsAdmin";
        private const string PASSWORD = "password";
        private const string DATABASE = "Series";
        private const string PORT     = "8889";

        private const string SELECT   = "select id, title, yearReleased, author, director, genre, icon from SeriesTitles";

        /* Constructors */

        public frmMain()
        {
            InitializeComponent();
            ScaleToFitScreen( this );
        }

        /* Handlers to Support Movement of Borderless Form */

        /* Prepare to capture form movement. */

        private void frmMain_MouseDown( object sender, MouseEventArgs e )
        {
            CaptureMove = true;
            Origin      = e.Location;
        }

        /* Capture form movement. */

        private void frmMain_MouseMove( object sender, MouseEventArgs e )
        {
            if( CaptureMove ) Location =  new Point( Location.X - Origin.X + e.X
                                                   , Location.Y - Origin.Y + e.Y );
            Update();
        }

        /* Stop capturing form movement. */

        private void frmMain_MouseUp( object sender, MouseEventArgs e )
        {
            CaptureMove = false;
        }

        /* Scale the specified control and its constituents by the specified factor,
         * iterating through all decendent controls, scaling their font by the
         * specified factor, too.  The default factor is 75% if none is specified.
         * 
         * This works better than Keith Websters method for fixing the client window
         * size when dealing with displays at different resolutions and DPIs.  One
         * caveat is that display scaling on the Windows VM needs to be set to 100%,
         * or Visual Studio starts borking the proportions in the designer.
         * 
         * I am now running a UWQHD display at 3440x1440 resolution, so the
         * specified iPhone image and window sizes exceed the height of my screen,
         * making things look very clunky.  Calling this method after initialization
         * scales it down so it fits.  On the MacBook Pro's Retina Display at 2880x
         * 1800 resolution, the screen exceeds the bounds of the iPhone image and
         * window, so no scaling need occur. */

        private void ScaleToFitScreen( Form form, float factor = 0.75f )
        {
            if( Height > Screen.PrimaryScreen.WorkingArea.Size.Height )
            {
                Scale( new SizeF( factor, factor ) );
                
                Action< Control > ScaleChildren = null;
                
                ScaleChildren = ( Control parent ) =>
                {
                    foreach( Control child in parent.Controls )
                    {
                        Font font = child.Font;

                        child.Font = new Font( font.Name, font.SizeInPoints * factor * factor );

                        ScaleChildren( child );
                    }
                };

                ScaleChildren( form );
            }
        }

        /* Attempts to read a server address from a text file at a hard
         * coded path, return the server address on success or null on
         * failure. */
        
        private async Task< string > ReadServerAsync( string path  )
        {
            if( string.IsNullOrEmpty( path ) ) return null;

            try
            {
                var reader = new StreamReader( path );
                return await reader.ReadToEndAsync();
            }
            catch
            {
                return null;
            }
        }

        /* Return a connection string built up from the passed in server
         * and the hardcoded constants for the other portions. */

        private string GetConnectString( string server, string user, string password, string database, string port, string sslMode = "none" )
        {
            var args = new string[]{  server, user, password, port, sslMode };

            if( args.Any( arg => string.IsNullOrEmpty( arg ) ) ) return null;
            
            return string.Concat(
                $"server={ server };"
            ,   $"userid={ user };"
            ,   $"password={ password };"
            ,   $"database={ database };"
            ,   $"port={ port };"
            ,   $"sslmode={ sslMode }"
            );
        }

        private async Task< DataTable >GetDataTableAsync()
        {
            var server  = await ReadServerAsync( PATH );

            if( server  == null ) return null;

            var connect = GetConnectString( server, USER, PASSWORD, DATABASE, PORT );

            if( connect == null ) return null;

            try
            {
                using( var connection = new MySqlConnection( connect ) )
                using( var adapter    = new MySqlDataAdapter( SELECT, connection ) )
                {
                    var table = new DataTable();

                    adapter.Fill( table );

                    return table;
                }
            }
            catch // ( Exception e )
            {
                // MessageBox.Show( e.Message );

                return null;
            }
        }

        private async Task< int? >SaveTableChangesAsync( DataTable table )
        {
            var server  = await ReadServerAsync( PATH );

            if( server  == null ) return null;

            var connect = GetConnectString( server, USER, PASSWORD, DATABASE, PORT );

            if( connect == null ) return null;

            try
            {
                using( var connection = new MySqlConnection( connect ) )
                using( var adapter    = new MySqlDataAdapter( SELECT, connection ) )
                using( var builder    = new MySqlCommandBuilder( adapter ) )
                {
                    return adapter.Update( table );
                }
            }
            catch // ( Exception e )
            {
                // MessageBox.Show( e.Message );

                return null;
            }
        }


        /* Form Event Handlers */

        private async void frmMain_Load( object sender, EventArgs e )
        {
            var table = await GetDataTableAsync();

            //imgIcons.Images.Clear();

            foreach( DataRow row in table.Rows )
            {
                var stream = new MemoryStream( (byte[]) row[ "icon" ] );
                var image  = Image.FromStream( stream );

                stream.Dispose();

                imgIcons.Images.Add( row[ "id" ].ToString(), image );

                var item  = new ListViewItem( row[ "title" ].ToString(), row[ "id" ].ToString() );

                item.Tag = row;

                lstSeries.Items.Add( item );
            }

            // table.Rows[ 0 ][ "title" ] = "Karate";
            // table.Rows[ 5 ].Delete();

            var rows = await SaveTableChangesAsync( table );

            MessageBox.Show( "Stop!" );
        }

        private void mnuPrint_Click( object sender, EventArgs e )
        {

        }

        private void mnuQuit_Click( object sender, EventArgs e )
        {
            Close();
        }
    }
}
