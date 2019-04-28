using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepnTabsClient
{
    public partial class frmDelete : Form
    {
        private int Remaining { get; set; } = 5;

        public frmDelete()
        {
            InitializeComponent();
        }

        private void BtnNo_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void BtnYes_MouseDown( object sender, MouseEventArgs e )
        {
            tmrConfirm.Enabled = true;
            btnYes.Text        = Remaining.ToString();
        }

        private void BtnYes_MouseUp( object sender, MouseEventArgs e )
        {
            tmrConfirm.Enabled = false;
            btnYes.Text        = "Yes";
            Remaining            = 5;
        }

        private void TmrConfirm_Tick( object sender, EventArgs e )
        {
            Remaining--;
            btnYes.Text = Remaining.ToString();

            if( Remaining == 0)
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
        }
    }
}
