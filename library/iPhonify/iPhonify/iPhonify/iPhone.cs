/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Subclass Form
 * Synopsis: Reduce some code and design duplication by creating an
 *           inheritable form that provided the desired appearance
 *           and base functionality up front.
 * Date:     April 11, 2019 */

using System.Drawing;
using System.Windows.Forms;

namespace iPhonify
{
    public partial class iPhone : iPhoneBase
    {
        /* Constructor */

        public iPhone()
        {
            InitializeComponent();
        }

        /* Overrides to prevent the form designer from
         * reverting the inheriting form's dimensions to defaults. */

        new public SizeF ClientSize
        {
            get { return new System.Drawing.Size( 687, 1351 ); }
            set { }
        }
    }
}
