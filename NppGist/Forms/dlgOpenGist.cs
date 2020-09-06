using NppNetInf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NppGist.JsonMapping;

namespace NppGist.Forms
{
    public partial class dlgOpenGist : Form
    {
        Dictionary<string, Gist> gists;
        bool closeDialog;

        public dlgOpenGist()
        {
            InitializeComponent();
            cbSaveToLocal.Checked = Main.SaveLocally;
            cbCloseOpenDialog.Checked = Main.CloseOpenDialog;
        }

        private void frmGists_Load(object sender, EventArgs e)
        {
            btnUpdate_Click(this, null);

            toolTip.SetToolTip(btnGoToGitHub, "Open Gist in Browser");
            toolTip.SetToolTip(btnUpdate, "Update Gists");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var gists = await Main.GitHubService.SendJsonRequestAsync<List<Gist>>("gists");
                this.gists = gists.ToDictionary(gist => gist.Id);
                GuiUtils.RebuildTreeView(tvGists, this.gists, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to connect to api.github.com. Try to refresh.{Environment.NewLine}Error message: {ex.Message}");
            }
        }

        private void tvGists_DoubleClick(object sender, EventArgs e)
        {
            if (tvGists.SelectedNode != null)
            {
                if (tvGists.SelectedNode.Parent != null || gists[tvGists.SelectedNode.Name.Split('/')[0]].Files.Count == 1)
                    btnOpen_Click(sender, e);
            }
        }

        private void tvGists_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvGists.SelectedNode != null)
            {
                var strs = tvGists.SelectedNode.Name.Split('/');
                var gist = gists[strs[0]];
                var file = gist.Files[strs[1]];
                if (tvGists.SelectedNode.Parent != null || gist.Files.Count == 1)
                {
                    btnOpen.Enabled = true;
                    btnRename.Enabled = true;
                    if (gist.Files.Count == 1)
                        tbGistLink.Text = gist.HtmlUrl;
                    else if (gist.Files.Count > 1)
                        tbGistLink.Text = gist.HtmlUrl + "#file-" + Utils.ReplaceNotCharactersOnHyphens(file.Filename);
                }
                else
                {
                    btnOpen.Enabled = false;
                    btnRename.Enabled = false;
                    tbGistLink.Text = gist.HtmlUrl;
                }

                cbPublic.Checked = gist.Public;
                tbLanguage.Text = file.Language ?? Lists.GistLangs[0];
                tbCreateDate.Text = gist.CreatedAt.ToString(CultureInfo.InvariantCulture);
                tbUpdateDate.Text = gist.UpdatedAt.ToString(CultureInfo.InvariantCulture);
                tbDescription.Text = gist.Description;
                btnDelete.Enabled = true;
            }
            else
            {
                btnOpen.Enabled = false;
                btnDelete.Enabled = false;
                btnRename.Enabled = false;
            }
        }

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                closeDialog = false;
                if (tvGists.SelectedNode != null)
                {
                    var strs = tvGists.SelectedNode.Name.Split('/');
                    var gist = gists[strs[0]];
                    var file = gist.Files[strs[1]];
                    var fileContent = await Main.GitHubService.SendRequestAsync(file.RawUrl);

                    if (!cbSaveToLocal.Checked)
                    {
                        string clipboardText = null;
                        if (Clipboard.ContainsText())
                        {
                            clipboardText = Clipboard.GetText();
                            Clipboard.SetText(" ");
                        }

                        Win32.SendMessage(PluginBase.NppData._nppHandle, (uint)NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);
                        PluginBase.SetCurrentFileText(fileContent.Remove(50));
                        PluginBase.AppendTextToCurrentFile(fileContent.Substring(50));
                        if (file.Language != null && Lists.GistNppLangs.TryGetValue(file.Language, out var langType))
                            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, 0, (int)langType);

                        if (clipboardText != null)
                            Clipboard.SetText(clipboardText);
                        if (cbCloseOpenDialog.Checked)
                            closeDialog = true;
                    }
                    else
                    {
                        if (!Directory.Exists(PluginBase.UserDataDir))
                            Directory.CreateDirectory(PluginBase.UserDataDir);
                        string gistDirectory;
                        if (gist.Files.Count > 1)
                        {
                            gistDirectory = Path.Combine(PluginBase.UserDataDir, gist.Id);
                            if (!Directory.Exists(gistDirectory))
                                Directory.CreateDirectory(gistDirectory);
                        }
                        else
                            gistDirectory = PluginBase.UserDataDir;

                        string filename;
                        if (gist.Files.Count == 1 && file.Filename.StartsWith("gistfile"))
                            filename = "gist:" + gist.Id;
                        else
                            filename = file.Filename;
                        filename = Path.Combine(gistDirectory, Utils.GetSafeFilename(filename));

                        var rewrite = true;
                        bool notOpenFile = false;
                        if (File.Exists(filename))
                        {
                            if (fileContent == File.ReadAllText(filename))
                            {
                                rewrite = false;
                            }
                            else if (MessageBox.Show(
                                         $"Do you want to replace existing file \"{Path.GetFileName(filename)}\"?", string.Empty, MessageBoxButtons.YesNo)
                                == DialogResult.No)
                            {
                                var dialog = new SaveFileDialog
                                {
                                    FileName = Path.GetFileName(filename),
                                    InitialDirectory = Path.GetFullPath(gistDirectory)
                                };
                                var extension = Path.GetExtension(filename);
                                dialog.Filter = string.IsNullOrEmpty(extension) ? "All files | *.*" : string.Format("(*{0} files) | *{0}", extension);
                                if (dialog.ShowDialog() == DialogResult.OK)
                                    filename = dialog.FileName;
                                else
                                    notOpenFile = true;
                            }
                        }

                        if (!notOpenFile)
                        {
                            if (rewrite)
                                File.WriteAllText(filename, fileContent);
                            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint)NppMsg.NPPM_DOOPEN, 0, filename);
                            if (cbCloseOpenDialog.Checked)
                                closeDialog = true;
                        }
                    }
                }
                if (sender is TreeView && closeDialog)
                    Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load gist." + Environment.NewLine + "Error message: " + ex.Message);
            }
        }

        private async void tvGists_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                await GuiUtils.DeleteItem(tvGists, gists, false);
            else if (e.KeyCode == Keys.F2)
                await GuiUtils.RenameItem(tvGists, gists, false);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            await GuiUtils.DeleteItem(tvGists, gists, false);
        }

        private async void btnRename_Click(object sender, EventArgs e)
        {
            await GuiUtils.RenameItem(tvGists, gists, false);
        }

        private void tbGistLink_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                ((TextBox)sender).SelectAll();
            });
        }

        private void btnGoToGitHub_Click(object sender, EventArgs e)
        {
            GuiUtils.GoToGitHub(tbGistLink.Text);
        }

        private void cbSaveToLocal_CheckedChanged(object sender, EventArgs e)
        {
            Main.SaveLocally = cbSaveToLocal.Checked;
            Win32.WritePrivateProfileString("Settings", "SaveToLocal", (Convert.ToInt32(Main.SaveLocally)).ToString(), Main.IniFileName);
        }

        private void cbCloseOpenDialog_CheckedChanged(object sender, EventArgs e)
        {
            Main.CloseOpenDialog = cbCloseOpenDialog.Checked;
            Win32.WritePrivateProfileString("Settings", "CloseOpenDialog", (Convert.ToInt32(Main.CloseOpenDialog)).ToString(), Main.IniFileName);
        }

        private void dlgOpenGist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None && !closeDialog)
                e.Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeDialog = true;
        }
    }
}
