using Scada.Comm.Devices;
using Scada.Data;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpAeroQuad
{
    /// <summary>
    /// Форма управления и сервиса
    /// </summary>
    public partial class FrmControl : Form
    {
        private int kpNum;     // номер КП
        private string cmdDir; // директория команд


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(int kpNum, string cmdDir)
        {
            FrmControl frmControl = new FrmControl();
            frmControl.kpNum = kpNum;
            frmControl.cmdDir = cmdDir;
            frmControl.ShowDialog();
        }


        private void FrmControl_Load(object sender, EventArgs e)
        {
            if (kpNum > 0)
                Text += ". Device: " + kpNum;
            btnSend_i.Enabled = kpNum > 0;
        }

        private void btnSend_i_Click(object sender, EventArgs e)
        {
            Command cmd = new Command(BaseValues.CmdTypes.Binary);
            cmd.KPNum = kpNum;
            cmd.CmdNum = 1;
            cmd.CmdData = new byte[] { 0x69 }; // символ i
            string msg;

            if (CommUtils.SaveCmd(cmdDir, "KpAeroQuad", cmd, out msg))
                ScadaUiUtils.ShowInfo(msg);
            else
                ScadaUiUtils.ShowError(msg);
        }

        private void btnRecordOnOff_Click(object sender, EventArgs e)
        {
            Command cmd = new Command(BaseValues.CmdTypes.Standard);
            cmd.KPNum = kpNum;
            cmd.CmdNum = 2;
            cmd.CmdVal = sender == btnRecordOn ? 1.0 : 0.0;
            string msg;

            if (CommUtils.SaveCmd(cmdDir, "KpAeroQuad", cmd, out msg))
                ScadaUiUtils.ShowInfo(msg);
            else
                ScadaUiUtils.ShowError(msg);
        }

        private void btnConvertToCsv_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileFormats.ConvertFileToCsv(openFileDialog.FileName,
                        Path.ChangeExtension(openFileDialog.FileName, "csv"));
                    ScadaUiUtils.ShowInfo("Conversion completed successfully.");
                }
                catch (Exception ex)
                {
                    ScadaUiUtils.ShowError(ex.Message);
                }
            }
        }
    }
}
