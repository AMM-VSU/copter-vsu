using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FindMarkerUI
{
    public partial class FrmMain : Form
    {
        // Import C++ functions
        private const string FindMarkerCalcDLL = @"C:\GIT\copter-vsu\VideoProcessing\OpenCVTest\x64\Release\FindMarkerCalc.dll";
        [DllImport(FindMarkerCalcDLL)]
        private static extern double Add(double a, double b);


        private const string DefSrcFileName = 
            @"C:\GIT\copter-vsu\VideoProcessing\OpenCVTest\FindMarkerUI\bin\Release\input.jpg";


        public FrmMain()
        {
            InitializeComponent();
        }


        private void AddToLog(string msg)
        {
            txtLog.Text += msg + Environment.NewLine;
        }

        private void ShowError(string msg)
        {
            AddToLog(msg);
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtSrcFileName.Text = DefSrcFileName;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                pbImage.Load(txtSrcFileName.Text);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnExecAction_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(Add(1, 2).ToString());
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}
