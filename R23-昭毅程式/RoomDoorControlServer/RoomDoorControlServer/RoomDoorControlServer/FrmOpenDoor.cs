using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RoomDoorControlServer
{
    public partial class FrmOpenDoor : Form
    {
        public FrmOpenDoor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ADAMControl control = new ADAMControl("", txtIP.Text, Convert.ToInt32(numPort.Value));
            control.OpenDoor(Convert.ToInt32(numDO.Value));
        }

   
    }
}
