/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: TicTacToe
 * Date:     April 16, 2019 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using static ClarkDavid_Assignment1.StringExtensions;

namespace TicTacToe
{
    public partial class frmTicTacToe : iPhonify.iPhone
    {
        private bool                    Running     = true;          
        private enum Pieces            { z = -1, x, o }

        private Pieces                 Piece        = Pieces.x;
        private enum Colors            { blue, red }

        private Colors                 CurrentColor = Colors.blue;

        private List< Button >         AllTiles;

        private List< List< Button > > WinningRuns;

        public frmTicTacToe()
        {
            InitializeComponent();
            CollectTiles();
            CollectWinningRuns();
            TagTiles();
            ColorTiles();
            SubscribeClicks();
        }

        private void loadGameToolStripMenuItem_Click( object sender, EventArgs e )
        {
            LoadGame();
        }

        private void saveGameToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SaveGame();
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void blueToolStripMenuItem_Click( object sender, EventArgs e )
        {
            blueToolStripMenuItem.Checked = true;
            redToolStripMenuItem.Checked  = false;

            ColorTiles( Colors.blue );
        }

        private void redToolStripMenuItem_Click( object sender, EventArgs e )
        {
            redToolStripMenuItem.Checked  = true;
            blueToolStripMenuItem.Checked = false;

            ColorTiles( Colors.red );
        }

        private void xToolStripMenuItem_Click( object sender, EventArgs e )
        {
            xToolStripMenuItem.Checked = true;
            oToolStripMenuItem.Checked = false;
            StartWith( Pieces.x );
        }

        private void oToolStripMenuItem_Click( object sender, EventArgs e )
        {
            oToolStripMenuItem.Checked = true;
            xToolStripMenuItem.Checked = false;
            StartWith( Pieces.o );
        }

        private void toolStripButton1_Click( object sender, EventArgs e )
        {
            TagTiles();

            Running                         = true;
            selectToolStripMenuItem.Enabled = true;
        }

        private void button_Click( object sender, EventArgs e )
        {
            if( !Running ) return;

            var button = sender as Button;

            button.Tag                      = Piece;
            button.ImageIndex               = (int) Piece;
            selectToolStripMenuItem.Enabled = false;

            CheckGameProgress();

            Piece = Piece == Pieces.x ? Pieces.o : Pieces.x;
        }

        private void SubscribeClicks()
        {
            Controls.OfType< Button >().ToList().ForEach( button =>
                button.Click += button_Click );
        }

        private void CheckGameProgress()
        {
            if( WinningRuns.Any( run => 
                run.All( tile => 
                    Piece == (Pieces) tile.Tag ) ) )
            {
                MessageBox.Show( ( Piece == Pieces.x ? "X" : "O" ) + " Wins!" );

                Running = false;
            }
            else if( AllTiles.All( tile =>
                    (Pieces) tile.Tag != Pieces.z ) )
            {
                MessageBox.Show("Stalemate!");

                Running = false;
            }
        }

        private void TagTiles()
        {
            AllTiles.ForEach( tile => {
                tile.Tag        = Pieces.z;
                tile.ImageIndex = -1;
            ; } );
        }

        private void ColorTiles( Colors color = Colors.blue )
        {
            CurrentColor = color;

            AllTiles.ForEach( tile =>
                tile.ImageList = CurrentColor == Colors.blue
                    ? blueImages : redImages );
        }

        private void CollectTiles()
        {
            AllTiles = Controls.OfType< Button >().ToList();
        }

        private void CollectWinningRuns()
        {
            var nums = Enumerable.Range(1, 3);
            var runs = "rc";

            WinningRuns =
               new List<List<Button>>(
               from run in runs
               from num in nums
               select new List<Button>(
                   from tl in AllTiles
                   where tl.Name.Contains($"{ run }{ num }")
                   select tl));

            WinningRuns.Add(new List<Button>() { r1c1button, r2c2button, r3c3button });
            WinningRuns.Add(new List<Button>() { r1c3button, r2c2button, r3c1button });
        }

        private void SaveGame()
        {
            var dlg = new SaveFileDialog();

            // Filter for TicTacToe files.
            dlg.Filter = "TTT Files (*.ttt)|*.ttt";

            if( dlg.ShowDialog() == DialogResult.OK )
            {
                using( StreamWriter sw = new StreamWriter( dlg.OpenFile() ) )
                {
                    // Convert the game to XML.
                    var xml    = new XElement( "game",
                            new XElement( "running",
                                new XText( Convert.ToString( Running ) ) ),
                            new XElement( "piece",
                                new XText( Convert.ToString( (int) Piece ) ) ),
                            new XElement( "tiles",
                                AllTiles.Select( tile => new XElement( "tile",
                                    new XText( Convert.ToString( (int) tile.Tag ) ) ) ) ) ).ToString();

                    // Convert XML to Base64.
                    var base64 = xml.ToBase64();
                    var md5    = base64.ToMD5();

                    /* Write the file. */
                    sw.Write( md5 );
                    sw.Write( base64 );
                }
            }
        }

        private void LoadGame()
        {
            var dlg = new OpenFileDialog();

            // Filter for TicTacToe files.
            dlg.Filter = "TTT Files (*.ttt)|*.ttt";

            void NotifyError()
            {
                MessageBox.Show("A terrible error has occurred!", ""
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ( dlg.ShowDialog() == DialogResult.OK )
            {
                using( StreamReader sr = new StreamReader( dlg.OpenFile() ) )
                {
                    // Read in the file, checking the MD5 hash integrity.
                    var file   = sr.ReadToEnd();
                    var md5    = file.Substring( 0, 32 );
                    var base64 = file.Substring( 32 );
                    var text   = base64.FromBase64();

                    // Deserialize the XML, populating the game tiles.
                    if( md5 == base64.ToMD5() )
                    {
                        try
                        {
                            var xml   = XDocument.Parse( text );

                            var saved = xml.Descendants( "tile" ).Select( tile =>
                                (Pieces) Convert.ToInt32( tile.Value ) ).ToList();

                            for( var i = 0; i < AllTiles.Count; i++)
                            {
                                AllTiles[ i ].Tag        =       saved[ i ];
                                AllTiles[ i ].ImageIndex = (int) saved[ i ];
                            }

                            Running =          Convert.ToBoolean( xml.Descendants( "running" ).First().Value.ToString() );
                            Piece   = (Pieces) Convert.ToInt32(   xml.Descendants( "piece"   ).First().Value.ToString() );

                            Piece = Piece == Pieces.x ? Pieces.o : Pieces.x;

                            CheckGameProgress();

                            Piece = Piece == Pieces.x ? Pieces.o : Pieces.x;
                        }
                        catch { NotifyError(); }
                    } else NotifyError();
                }
            }
        }

        private void StartWith( Pieces piece )
        {
            if( piece == Pieces.z )
                throw ( new ArgumentException( "Piece must be x or o!", "piece" ) );
            else
                Piece = piece;
        }
    }
}
