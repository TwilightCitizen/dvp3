using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlideItem
{
    public partial class SlideItem: UserControl
    {
        /* Indicate whether to capture slide gesture or not. */

        private bool  Slide  { get; set; } = false;

        /* Events to signal slide gestures. */

        public     event EventHandler SlideLeft;
        public     event EventHandler SlideRight;
        public new event EventHandler Click;

        /* Capture origin point of edit button. */

        private Point Origin { get; set; }

        /* Capture origin of mouse pointer. */

        private int   Mouse  { get; set; }

        /* Text labels of the buttons. */

        public string LeftText
        {
            get { return btnLeft.Text;  }
            set { btnLeft.Text = value; }
        }

        public string RightText
        {
            get { return btnRight.Text;  }
            set { btnRight.Text = value; }
        }

        public string MainText
        {
            get { return btnMain.Text;  }
            set { btnMain.Text = value; }
        }

        /* Constructors */

        public SlideItem( string leftText = "Left", string rightText = "Right", string mainText = "Main" )
        {
            InitializeComponent();

            LeftText = leftText; RightText = rightText; MainText = mainText;
        }

        public SlideItem( )
        {
            InitializeComponent();

            LeftText = "Left"; RightText = "Right"; MainText = "Main";
        }

        /* Capture slide, main button location, and mouse location on mouse down. */

        private void BtnEdit_MouseDown( object sender, MouseEventArgs e )
        {
            Slide  = true;
            Origin = btnMain.Location;
            Mouse  = e.X;
        }

        /* Slide the main button left or right with the user's mouse movement when
         * the mouse is held down.  Once it crosses the left or right slide limit
         * thresholds, fire the appropriate slide event, and fire the mouse up event
         * to reset the main button to a sane location. */

        private void BtnEdit_MouseMove( object sender, MouseEventArgs e )
        {
            var withinLimitLeft  = btnMain.Location.X >= btnLeft.Location.X - btnLeft.Width;
            var withinLimitRight = btnMain.Location.X <= btnLeft.Location.X + btnLeft.Width;
            

            if ( Slide && withinLimitLeft && withinLimitRight)
            {
                btnMain.Location     = new Point( btnMain.Location.X + ( e.X - Mouse ), btnMain.Location.Y );

                var beyondLimitLeft  = btnMain.Location.X <= btnLeft.Location.X - btnLeft.Width;
                var beyondLimitRight = btnMain.Location.X >= btnLeft.Location.X + btnLeft.Width;

                if ( beyondLimitLeft )
                {
                    btnMain.Location = new Point( btnLeft.Location.X - btnLeft.Width, btnMain.Location.Y );
                    SlideLeft?.Invoke( this, null );
                    BtnEdit_MouseUp( this, null );
                }

                if( beyondLimitRight )
                {
                    btnMain.Location = new Point( btnLeft.Location.X + btnLeft.Width, btnMain.Location.Y );
                    SlideRight?.Invoke( this, null );
                    BtnEdit_MouseUp( this, null );
                }
            }
        }

        /* Release slide and return main button to origin on mouse up. */

        private void BtnEdit_MouseUp( object sender, MouseEventArgs e )
        {
            Slide            = false;
            btnMain.Location = Origin;
        }

        /* Translate main button click as control click. */

        private void BtnEdit_Click( object sender, EventArgs e )
        {
            Click?.Invoke( this, null );
        }
    }
}
