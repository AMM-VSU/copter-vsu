using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm.KP.AeroQuad
{
    public partial class FrmControl : Form
    {
        public FrmControl()
        {
            InitializeComponent();
        }

        private void btnSend_i_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnConvertToCsv_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileFormats.ConvertF0ToCsv(openFileDialog.FileName, 
                    Path.ChangeExtension(openFileDialog.FileName, "csv"));
            }
        }
    }
}
