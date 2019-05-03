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
    public partial class frmTasks : Form
    {
        public frmTasks()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click( object sender, EventArgs e )
        {
            Add();
        }

        private void BtnToggle_Click( object sender, EventArgs e )
        {
            Toggle();
        }

        private void BtnRename_Click( object sender, EventArgs e )
        {
            Rename();
        }

        private void BtnBack_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void BtnDelete_Click( object sender, EventArgs e )
        {
            Delete();
        }

        private void LstTasks_SelectedIndexChanged( object sender, EventArgs e )
        {
            CheckSelection();
        }

        private void CheckSelection()
        {
            btnRename.Enabled =
            btnDelete.Enabled = lstTasks.SelectedItems.Count > 0;
        }

        private void Add()
        {

        }

        private void Toggle()
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
    }
}
