using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace NppGist.Forms
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            lblName.Text = "NppGist " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void linkSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((LinkLabel)sender).Text);
        }
    }
}