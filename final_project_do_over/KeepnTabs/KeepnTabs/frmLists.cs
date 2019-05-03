/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Keep'n Tabs Do Over
 * Date:     May 3, 2019 */
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepnTabs
{
    public partial class frmLists : Form
    {
        public frmLists()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void BtnRename_Click( object sender, EventArgs e )
        {
            Rename();
        }

        private void BtnDelete_Click( object sender, EventArgs e )
        {
            Delete();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void LstLists_SelectedIndexChanged( object sender, EventArgs e )
        {
            CheckSelection();
        }

        private void LstLists_DoubleClick( object sender, EventArgs e )
        {
            Tasks();
        }

        private void CheckSelection()
        {
            btnRename.Enabled =
            btnDelete.Enabled = lstLists.SelectedItems.Count > 0;
        }

        private void Add()
        {

        }

        private void Rename()
        {
            CheckSelection();
        }

        private void Delete()
        {
            CheckSelection();
        }

        private void Tasks()
        {
            var frm = new frmTasks();

            Hide();

            frm.ShowDialog( this );

            Show();
        }
    }
}
