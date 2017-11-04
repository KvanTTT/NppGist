using NppPluginNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NppGist.Forms
{
    public partial class dlgOpenGist : Form
    {
        Dictionary<string, Gist> Gists;
        bool CloseDialog;

        public dlgOpenGist()
        {
            InitializeComponent();
            cbSaveToLocal.Checked = Main.SaveLocally;
            cbCloseOpenDialog.Checked = Main.CloseOpenDialog;
        }

        private void frmGists_Load(object sender, EventArgs e)
        {
            btnUpdate_Click(this, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var gists = Utils.SendJsonRequest<List<Gist>>(string.Format("{0}/gists?access_token={1}", Main.ApiUrl, Main.Token));
                Gists = gists.ToDictionary<Gist, string>(gist => gist.Id);
                GuiUtils.RebuildTreeView(tvGists, Gists, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Unable to connect to api.github.com. Try to refresh.{0}Error message: {1}",
                    Environment.NewLine, ex.Message));
            }
        }

        private void tvGists_DoubleClick(object sender, EventArgs e)
        {
            if (tvGists.SelectedNode != null)
            {
                if (tvGists.SelectedNode.Parent != null || Gists[tvGists.SelectedNode.Name.Split('/')[0]].Files.Count == 1)
                    btnOpen_Click(sender, e);
            }
        }

        private void tvGists_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvGists.SelectedNode != null)
            {
                var strs = tvGists.SelectedNode.Name.Split('/');
                var gist = Gists[strs[0]];
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
                tbCreateDate.Text = gist.CreatedAt.ToString();
                tbUpdateDate.Text = gist.UpdatedAt.ToString();
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

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                CloseDialog = false;
                if (tvGists.SelectedNode != null)
                {
                    var strs = tvGists.SelectedNode.Name.Split('/');
                    var gist = Gists[strs[0]];
                    var file = gist.Files[strs[1]];
                    var fileContent = Utils.SendRequest(file.RawUrl);

                    if (!cbSaveToLocal.Checked)
                    {
                        string clipboardText = null;
                        if (Clipboard.ContainsText())
                        {
                            clipboardText = Clipboard.GetText();
                            Clipboard.SetText(" ");
                        }

                        Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);
                        LangType langType;
                        PluginBase.SetCurrentFileText(fileContent.Remove(50));
                        PluginBase.AppendTextToCurrentFile(fileContent.Substring(50));
                        if (file.Language != null && Lists.GistNppLangs.TryGetValue(file.Language, out langType))
                            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_SETCURRENTLANGTYPE, 0, (int)langType);

                        if (clipboardText != null)
                            Clipboard.SetText(clipboardText);
                        if (cbCloseOpenDialog.Checked)
                            CloseDialog = true;
                    }
                    else
                    {
                        var dir = PluginBase.GetPluginsConfigDir() + @"\..\" + Main.PluginName;
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        string gistDirectory;
                        if (gist.Files.Count > 1)
                        {
                            gistDirectory = dir + @"\" + gist.Id;
                            if (!Directory.Exists(gistDirectory))
                                Directory.CreateDirectory(gistDirectory);
                        }
                        else
                            gistDirectory = dir;

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
                            else if (MessageBox.Show("Do you want to replace existing file \"" + Path.GetFileName(filename) + "\"?", string.Empty, MessageBoxButtons.YesNo)
                                == System.Windows.Forms.DialogResult.No)
                            {
                                var dialog = new SaveFileDialog();
                                dialog.FileName = Path.GetFileName(filename);
                                dialog.InitialDirectory = Path.GetFullPath(gistDirectory);
                                var extension = Path.GetExtension(filename);
                                dialog.Filter = string.IsNullOrEmpty(extension) ? "All files | *.*" : string.Format("(*{0} files) | *{0}", extension);
                                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    filename = dialog.FileName;
                                else
                                    notOpenFile = true;
                            }
                        }

                        if (!notOpenFile)
                        {
                            if (rewrite)
                                File.WriteAllText(filename, fileContent);
                            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DOOPEN, 0, filename);
                            if (cbCloseOpenDialog.Checked)
                                CloseDialog = true;
                        }
                    }
                }
                if (sender is TreeView && CloseDialog)
                    Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load gist." + Environment.NewLine + "Error message: " + ex.Message);
            }
        }

        private void tvGists_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                GuiUtils.DeleteItem(tvGists, Gists, false);
            else if (e.KeyCode == Keys.F2)
                GuiUtils.RenameItem(tvGists, Gists, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GuiUtils.DeleteItem(tvGists, Gists, false);
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            GuiUtils.RenameItem(tvGists, Gists, false);
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
            if (e.CloseReason == CloseReason.None && !CloseDialog)
                e.Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseDialog = true;
        }
    }
}
