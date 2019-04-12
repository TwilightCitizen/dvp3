using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleAPI;

namespace KeepnTabsAPI
{
    public partial class frmController : Form
    {
        private Server Server = new Server( "http://192.168.82.128:8080/todo/" );

        public frmController()
        {
            InitializeComponent();

            Server.RequestArrived += Server_RequestArrived;
        }

        private void Server_RequestArrived( object sender, Exchange e )
        {
            var requests = new Dictionary< string, Dictionary< string, Action< Exchange > > >()
            {
                [ "user" ] = new Dictionary< string, Action< Exchange > >()
                {
                    [ "add" ]     = UserAdd,
                    [ "confirm" ] = UserConfirm,
                    [ "update" ]  = UserUpdate,
                    [ "delete" ]  = UserDelete
                },
                [ "list" ] = new Dictionary< string, Action< Exchange > >()
                {
                    [ "add" ]     = ListAdd,
                    [ "update" ]  = ListUpdate,
                    [ "delete" ]  = ListDelete
                },
                [ "task" ] = new Dictionary< string, Action< Exchange > >()
                {
                    [ "add" ]     = TaskAdd,
                    [ "update" ]  = TaskUpdate,
                    [ "delete" ]  = TaskDelete
                }
            };

            try
            {
                requests[ e.Request[ 0 ] ][ e.Request[ 1 ] ]( e );
            }
            catch
            {
                Invalid( e );
            }
        }

        private void UserAdd( Exchange e )
        {
            e.Reply = "User Add";
        }

        private void UserConfirm( Exchange e )
        {
            e.Reply = "User Confirm";
        }

        private void UserUpdate( Exchange e )
        {
            e.Reply = "User Update";
        }

        private void UserDelete( Exchange e )
        {
            e.Reply = "User Delete";
        }

        private void ListAdd( Exchange e )
        {
            e.Reply = "List Add";
        }

        private void ListUpdate( Exchange e )
        {
            e.Reply = "List Update";
        }

        private void ListDelete( Exchange e )
        {
            e.Reply = "List Delete";
        }

        private void TaskAdd( Exchange e )
        {
            e.Reply = "Task Add";
        }

        private void TaskUpdate( Exchange e )
        {
            e.Reply = "Task Update";
        }

        private void TaskDelete( Exchange e )
        {
            e.Reply = "Task Delete";
        }

        private void Invalid( Exchange e )
        {
            e.Reply = "Invalid Request";
        }

        private void btnStartStop_Click( object sender, EventArgs e )
        {
            btnStartStop.Text = Server.IsRunning ? "&Start" : "&Stop";

            if( Server.IsRunning ) Server.Stop(); else Server.Start();
        }
    }
}
