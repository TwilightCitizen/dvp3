using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class frmTicTacToe : Form
    {
        private enum Pieces            { x, o, z }

        private Pieces                 StartingPiece = Pieces.x;

        private enum Colors            { blue, red }

        private Colors                 CurrentColor  = Colors.blue;

        private List< Button >         AllTiles;

        private List< List< Button > > WinningRuns;

        public frmTicTacToe()
        {
            InitializeComponent();
            CollectTiles();
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
            SwitchColors( Colors.blue );
        }

        private void redToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SwitchColors( Colors.red );
        }

        private void xToolStripMenuItem_Click( object sender, EventArgs e )
        {
            StartWith( Pieces.x );
        }

        private void oToolStripMenuItem_Click( object sender, EventArgs e )
        {
            StartWith( Pieces.o );
        }

        private void toolStripButton1_Click( object sender, EventArgs e )
        {
            // New Game
        }

        private void r1c1button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r1c2button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r1c3button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r2c1button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r2c2button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r2c3button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r3c1button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r3c2button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void r3c3button_Click( object sender, EventArgs e )
        {
            button_Click( sender, e );
        }

        private void button_Click( object sender, EventArgs e )
        {
            var button = sender as Button;

            button.Tag = StartingPiece;

            CheckGameProgress();
        }

        private void CheckGameProgress()
        {
            if( WinningRuns.Any( run => 
                run.All( tile => 
                    StartingPiece == ( tile.Tag != null
                        ? (Pieces) tile.Tag
                        : Pieces.z ) ) ) )
                MessageBox.Show( "Win" );
        }

        private void CollectTiles()
        {
            var nums    = Enumerable.Range( 1, 3 );
            var runs    = "rc";

            AllTiles    = Controls.OfType< Button >().ToList();

            WinningRuns = 
                new List< List< Button > > (
                from run in runs
                from num in nums
                select new List< Button > (
                    from tl in AllTiles
                    where tl.Name.Contains( $"{ run }{ num }" )
                    select tl ) );

            WinningRuns.Add( new List< Button >(){ r1c1button, r2c2button, r3c3button } );
            WinningRuns.Add( new List< Button >(){ r1c3button, r2c2button, r3c1button } );
        }

        private void LoadGame()
        {
        }

        private void SaveGame()
        {

        }

        private void SwitchColors( Colors color )
        {

        }

        private void StartWith( Pieces piece )
        {
            switch( piece )
            {
                case Pieces.x:
                case Pieces.o: break;
                default:       throw( new ArgumentException( "Piece must be x or o!", "piece" ) );
            }
        }
    }
}
