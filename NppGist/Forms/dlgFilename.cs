using NppGist.Forms;
using System;
using System.Threading;
using System.Windows.Forms;

namespace NppGist
{
    public partial class dlgFilename : Form
    {
        private readonly System.Threading.Timer _detectExtensionTimer;

        public string Filename
        {
            get => tbFilename.Text;
            set => tbFilename.Text = value;
        }

        public dlgFilename(string filename)
        {
            InitializeComponent();

            _detectExtensionTimer = new System.Threading.Timer(_ => GuiUtils.UpdateExtenstionResult(cmbLanguage, tbFilename), null, Timeout.Infinite, Timeout.Infinite);

            tbFilename.Text = filename;
            foreach (var lang in Lists.GistLangs)
                cmbLanguage.Items.Add(lang);

            GuiUtils.UpdateExtenstionResult(cmbLanguage, tbFilename);
        }

        private void tbFilename_KeyPress(object sender, KeyPressEventArgs e)
        {
            GuiUtils.TextBoxKeyPressRestrictInvalidFilenameChars(e);
        }

        private void tbFilename_TextChanged(object sender, EventArgs e)
        {
            GuiUtils.TextBoxTextChangedRestrictInvalidFilenameChars((TextBox)sender);

            _detectExtensionTimer.Change(200, Timeout.Infinite);
        }
    }
}