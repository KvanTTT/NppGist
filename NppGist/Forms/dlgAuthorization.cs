using NppPluginNET;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NppGist.Forms
{
	public partial class dlgAuthorization : Form
	{
		bool CloseDialog = true;

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
				Dictionary<string, string> responseHeaders;
				var response = Utils.SendRequest(string.Format("{0}/user?access_token={1}", Main.ApiUrl, tbAccessToken.Text), out responseHeaders);
				user = JsonSerializer.DeserializeFromString<User>(response);
				string scopes;
				if (!responseHeaders.TryGetValue("X-OAuth-Scopes", out scopes) || !scopes.Contains("gist"))
				{
					MessageBox.Show("Entered access token does not contains gist scopes");
					error = true;
				}
			}
			catch
			{
				MessageBox.Show("Bad internet connection or wrong access token");
				error = true;
			}

			if (!error)
			{
				Main.Login = user.Login;
				Main.Token = tbAccessToken.Text;
				Win32.WritePrivateProfileString("Settings", "Login", Main.Login, Main.IniFileName);
				Win32.WritePrivateProfileString("Settings", "AccessToken", AccessToken.EncryptToken(Main.Token), Main.IniFileName);
				CloseDialog = true;
			}
			else
				CloseDialog = false;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CloseDialog = true;
		}

		private void frmAuthorization_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.None && !CloseDialog)
				e.Cancel = true;
		}
	}
}
