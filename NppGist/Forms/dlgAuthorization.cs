using NppNetInf;
using ServiceStack.Text;
using System;
using System.Linq;
using System.Windows.Forms;
using NppGist.JsonMapping;

namespace NppGist.Forms
{
    public partial class dlgAuthorization : Form
    {
        bool closeDialog = true;

        public dlgAuthorization()
        {
            InitializeComponent();
        }

        private void linkGetAccessToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool error = false;
            User user = null;
            try
            {
                Main.GitHubService = new GitHubService(tbAccessToken.Text.Trim());
                var response = Main.GitHubService.SendRequest("user").Result;
                user = JsonSerializer.DeserializeFromStream<User>(response.Content.ReadAsStreamAsync().Result);

                bool containsGistScope = response.Headers.Any(header => header.Key == "X-OAuth-Scopes" &&
                                                                      header.Value.Any(value => value.Contains("gist")));

                if (!containsGistScope)
                {
                    MessageBox.Show("Entered access token does not contain gist scopes");
                    error = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to send access token: " + ex.Message);
                error = true;
            }

            if (!error)
            {
                Main.Login = user.Login;
                Win32.WritePrivateProfileString("Settings", "Login", Main.Login, Main.IniFileName);
                Win32.WritePrivateProfileString("Settings", "AccessToken", AccessToken.EncryptToken(Main.GitHubService.Token), Main.IniFileName);
                closeDialog = true;
            }
            else
            {
                closeDialog = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeDialog = true;
        }

        private void frmAuthorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None && !closeDialog)
                e.Cancel = true;
        }
    }
}