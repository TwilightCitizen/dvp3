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

        private const string SELECT   = "select id, title, yearReleased, publisher, author, director, genre, icon from SeriesTitles";

        /* Data Table */

        private DataTable Table { get; set; } = null;

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
                
                Action< Control >  ScaleChildren = null;

                ScaleChildren = ( Control parent ) =>
                {
                    foreach( Control child in parent.Controls )
                    {
                        child.Font = new Font( child.Font.Name, child.Font.SizeInPoints * factor * factor );

                        ScaleChildren( child );
                    }
                };

                ScaleChildren( form );

                /* Menus do not constitute "controls".  They need to be scaled separately. */

                form.MainMenuStrip?.Items.OfType< ToolStripItem >().ToList().ForEach( item =>
                    item.Font = new Font( item.Font.Name, item.Font.SizeInPoints * factor * factor )
                );
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

        /* Return a connection string built up from the passed components,
         * returning null if any are blank or null. */

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

        /* Connect to the database specified in the connection string, execute the
         * provided query, and return a datatable containing the results.  Return null
         * if any steps fail. */

        private async Task< DataTable >GetDataTableAsync( string connect, string select )
        {
            if( connect == null ) return null;

            try
            {
                using( var connection = new MySqlConnection( connect ) )
                using( var adapter    = new MySqlDataAdapter( select, connection ) )
                {
                    var table = new DataTable();

                    await adapter.FillAsync( table );

                    return table;
                }
            }
            catch // ( Exception e )
            {
                // MessageBox.Show( e.Message );

                return null;
            }
        }

        /* Connect to the database specified in the connection string, build non-
         * select queries from the provided query, update the database, and return
         * the number of affected rows.  Return null if any steps fail. */

        private async Task< int? >SaveTableChangesAsync( string connect, string select, DataTable table )
        {
            if( connect == null ) return null;

            try
            {
                using( var connection = new MySqlConnection( connect ) )
                using( var adapter    = new MySqlDataAdapter( select, connection ) )
                using( var builder    = new MySqlCommandBuilder( adapter ) )
                {

                    /* This feels like a cludge, but it is unfortunately necessary.  Update
                     * commands called when the data adapter affects record insertions do
                     * NOT return the newly created ID and do not update the added row
                     * in the data table as a result.  This results in added records that
                     * cannot be deleted.  Also, once a failed deletion occurs, accept changes
                     * called after any other operation that works will still fail becuase the
                     * deletion is queued to occur.  This handler checks for an insertion after
                     * a row update occurs, and if an insertion occured, it does what the
                     * data adapter should be doing in the first place. */

                    MySqlRowUpdatedEventHandler handler = null;

                    handler = ( object sender, MySqlRowUpdatedEventArgs args ) =>
                    {
                        if( args.Command.CommandText.Contains( "INSERT" ))
                            table.Rows[ table.Rows.Count - 1 ][ "id" ] = args.Command.LastInsertedId;

                        adapter.RowUpdated -= handler;
                    };

                    adapter.RowUpdated += handler;

                    return await adapter.UpdateAsync( table );
                }
            }
            catch // ( Exception e )
            {
                // MessageBox.Show( e.Message );

                return null;
            }
        }


        /* Retreive data table from database, populating the image list and list view
         * with the returned images and other values. */

        private async Task PopulateListAysnc()
        {
            var server = await ReadServerAsync( PATH );

            if( server != null )
            {
                var connect = GetConnectString( server, USER, PASSWORD, DATABASE, PORT );

                Table = await GetDataTableAsync( connect, SELECT );

                if( Table != null )
                {
                    imgIcons.Images.Clear();

                    foreach( DataRow row in Table.Rows )
                    {
                        try
                        {
                            /* On the chance that a series has a non-existent image,
                             * do not install it to the image list.  The list view
                             * item will index a non-existent image and show nothing. */ 

                            var bytes  = (byte[]) row[ "icon" ];

                            using( var stream = new MemoryStream( bytes ) )
                                imgIcons.Images.Add( row[ "id" ].ToString(), Image.FromStream( stream ) );
                        }
                        catch{ }

                        var item = new ListViewItem( row[ "title" ].ToString(), row[ "id" ].ToString() );

                        item.Tag = row;

                        lstSeries.Items.Add( item );
                    }
                }
            }
        }

        /* Remove the selected series from the data table, passing changes to save to
         * the database.  If changes update successfully, remove the series from the
         * list and accept the deletion in the data table. */

        private async Task DeleteSeriesAsync( ListViewItem delete )
        {
            var server = await ReadServerAsync( PATH );

            if( server != null )
            {
                var connect = GetConnectString( server, USER, PASSWORD, DATABASE, PORT );

                Table.Rows[ Table.Rows.IndexOf( (DataRow) delete.Tag ) ].Delete();

                if( await SaveTableChangesAsync( connect, SELECT, Table ) != null )
                {
                    lstSeries.Items.Remove( delete );

                    Table.AcceptChanges();
                }
            }
        }

        /* Present the user with the add/edit screen for editing the passed in
         * data row, presumably a fresh one, though an existing one would work.
         * If the user does not cancel the add, persist it to the database and
         * the list view. */

        private async Task AddSeriesAsync( DataRow add )
        {
            using( var dlg = new dlgAddEdit( add ) )
            {
                Hide();
                dlg.ShowDialog( this );
                Show();
                lstSeries.SelectedIndices.Clear();

                if( dlg.DialogResult == DialogResult.OK )
                {
                    var server = await ReadServerAsync( PATH );

                    if( server != null )
                    {
                        var connect = GetConnectString( server, USER, PASSWORD, DATABASE, PORT );

                        Table.Rows.Add( add );

                        if( await SaveTableChangesAsync( connect, SELECT, Table ) != null )
                        {
                            var item = new ListViewItem( add[ "title" ].ToString(), add[ "id" ].ToString() );

                            item.Tag = add;

                            lstSeries.Items.Add( item );
                            Table.AcceptChanges();
                        }
                        else
                            Table.Rows.Remove( add );
                    }
                }  
            }
        }

        /* Present the user with the add/edit screen for editing the passed in
         * data row, presumably an existing one.  If the user does not cancel the
         * edit, persist it to the database and overwrite the list view item with
         * one created from the edits. */

        private async Task EditSeriesAsync( DataRow edit )
        {
            using( var dlg = new dlgAddEdit( edit ) )
            {
                Hide();
                edit.BeginEdit();
                dlg.ShowDialog( this );
                Show();
                lstSeries.SelectedIndices.Clear();

                if( dlg.DialogResult == DialogResult.OK )
                {
                    var server = await ReadServerAsync( PATH );

                    if( server != null )
                    {
                        var connect = GetConnectString( server, USER, PASSWORD, DATABASE, PORT );

                        if( await SaveTableChangesAsync( connect, SELECT, Table ) != null )
                        {
                            var item = new ListViewItem( edit[ "title" ].ToString(), edit[ "id" ].ToString() );

                            item.Tag = edit;

                            Table.AcceptChanges();
                        }
                        else
                            edit.CancelEdit();
                    }
                }  
            }
        }

        /* Form Event Handlers */

        private async void frmMain_Load( object sender, EventArgs e )
        {
            await PopulateListAysnc();
        }

        private void mnuPrint_Click( object sender, EventArgs e )
        {

        }

        private void mnuQuit_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void lstSeries_SelectedIndexChanged( object sender, EventArgs e )
        {
            /* Edit and deletion depend on actual selection */

            btnEdit.Enabled   =
            btnDelete.Enabled = lstSeries.SelectedItems.Count > 0;
        }

        private async void btnDelete_Click( object sender, EventArgs e )
        {
            /* Confirm Deletion */

            var choice = MessageBox.Show (
                "Are you sure you want to delete this series?"
            ,   "Series Deletion"
            ,   MessageBoxButtons.YesNo
            ,   MessageBoxIcon.Question
            );

            if( choice == DialogResult.Yes ) await DeleteSeriesAsync( lstSeries.SelectedItems[ 0 ] ); 
        }

        private async void btnAdd_Click( object sender, EventArgs e )
        {
            await AddSeriesAsync( Table.NewRow() );
        }

        private async void btnEdit_Click( object sender, EventArgs e )
        {
            await EditSeriesAsync( (DataRow) lstSeries.SelectedItems[ 0 ].Tag );
        }
    }
}
