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
    public partial class FrmNewCard : Form
    {
        public string ABA
        {
            get { return txtABA.Text; }
        }

        public string WEG
        {
            get 
            {
                try
                {
                    return TCommon.GetWEG(txtABA.Text);
                }
                catch
                {
                    return "0000000000";
                }
            }  
        }

        public string Message
        {
            get { return txtMessage.Text; }
        }

        public FrmNewCard()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
