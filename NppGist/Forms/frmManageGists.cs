using NppNetInf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NppGist.JsonMapping;

namespace NppGist.Forms
{
    public partial class frmManageGists : Form
    {
        private readonly System.Threading.Timer detectExtensionTimer;
        private bool closeDialog;
        private Paginator paginator;

        public frmManageGists()
        {
            InitializeComponent();

            paginator = new Paginator(tvGists, btnPrevPage, btnNextPage, tbPageNumber, true);

            foreach (var lang in Lists.GistLangs)
                cmbLanguage.Items.Add(lang);

            LangType langType = LangType.L_TEXT;
            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint) NppMsg.NPPM_GETCURRENTLANGTYPE, 0, ref langType);
            var gistNppLang = Lists.GistNppLangs.FirstOrDefault(lang => lang.Value == langType);
            cmbLanguage.SelectedItem = gistNppLang.Key ?? Lists.GistLangs[0];
            cbCloseDialog.Checked = Main.CloseDialog;

            detectExtensionTimer =
                new System.Threading.Timer(_ => GuiUtils.UpdateExtenstionResult(cmbLanguage, tbGistName), null, 0,
                    Timeout.Infinite);

            toolTip.SetToolTip(btnGoToGitHub, "Open Gist in Browser");
            toolTip.SetToolTip(btnUpdate, "Update Gists");
        }

        private async void frmManageGists_Load(object sender, EventArgs e)
        {
            try
            {
                if (!await paginator.UpdateGists(PageStatus.Init))
                {
                    return;
                }

                var currentFileName = PluginBase.GetFullCurrentFileName();
                tvGists.Select();
                if (currentFileName.StartsWith("new"))
                {
                    tvGists.SelectedNode = tvGists.Nodes.Count > 0 ? tvGists.Nodes[0] : null;
                }
                else
                {
                    tvGists.SelectedNode = null;
                    bool nodeFound = false;
                    var shortFileName = Path.GetFileName(currentFileName);
                    foreach (var keyGist in paginator.Gists)
                    {
                        var gist = keyGist.Value;
                        if (gist.Files.Count == 1 && gist.Files.First().Key.StartsWith("gistfile"))
                        {
                            if (shortFileName == "gist-" + gist.Id)
                            {
                                tvGists.SelectedNode = tvGists.Nodes.Find(GuiUtils.GetTreeViewKey(gist, gist.Files.First().Value), true).FirstOrDefault();
                                nodeFound = true;
                                break;
                            }
                        }
                        else
                        {
                            var gistFile =
                                gist.Files.FirstOrDefault(file => Utils.GetSafeFilename(file.Key) == shortFileName);
                            if (gistFile.Key != null)
                            {
                                tvGists.SelectedNode = tvGists.Nodes.Find(GuiUtils.GetTreeViewKey(gist, gistFile.Value), true).FirstOrDefault();
                                nodeFound = true;
                                break;
                            }
                        }
                    }
                    if (!nodeFound)
                    {
                        tvGists.SelectedNode = tvGists.Nodes.Count > 0 ? tvGists.Nodes[0] : null;
                        tbGistName.Text = shortFileName;
                    }
                    tvGists.SelectedNode?.EnsureVisible();
                    GuiUtils.UpdateExtenstionResult(cmbLanguage, tbGistName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load gist." + Environment.NewLine + "Error message: " + ex.Message);
            }
        }

        private void frmManageGists_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None && !closeDialog)
                e.Cancel = true;
        }

        private void tvGists_DoubleClick(object sender, EventArgs e)
        {
            var selectedNode = tvGists.SelectedNode;
            if (selectedNode != null)
            {
                var parts = selectedNode.Name.Split(new[] {'/'}, 2);
                if (paginator.Gists.TryGetValue(parts[0], out Gist gist))
                {
                    if (parts.Length == 1 || parts.Length > 1 && gist.Files.ContainsKey(parts[1]))
                    {
                        btnOpen_Click(sender, e);
                    }
                }
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                closeDialog = false;
                if (tvGists.SelectedNode != null)
                {
                    if (tvGists.SelectedNode.Name == GuiUtils.AllGistsKey)
                    {
                        // Creating new gist
                        var createdGist = await CreateGist();
                        if (cbCloseDialog.Checked)
                        {
                            closeDialog = true;
                        }
                        else
                        {
                            await paginator.UpdateGists(PageStatus.Init);
                            SelectFileInGist(createdGist, "");
                        }
                    }
                    else
                    {
                        // Updating existing gist
                        var parts = tvGists.SelectedNode.Name.Split('/');
                        var gist = paginator.Gists[parts[0]];
                        var file = gist.Files[parts[1]];
                        var gistName = GuiUtils.GetGistName(gist);

                        if (gist.Files.Count == 1)
                        {
                            bool updateGistFile = false;
                            bool createGistFile = false;
                            if (file.Filename == tbGistName.Text)
                            {
                                if (MessageBox.Show($"Do you want to update gist \"{gistName}\"?",
                                    string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    updateGistFile = true;
                                }
                            }
                            else
                                createGistFile = true;

                            if (updateGistFile || createGistFile)
                            {
                                var updatedGist = await UpdateOrCreateFileInGist(gist);
                                if (cbCloseDialog.Checked)
                                {
                                    closeDialog = true;
                                }
                                else
                                {
                                    GuiUtils.RebuildTreeView(tvGists, paginator.Gists, true);
                                    SelectFileInGist(updatedGist, tbGistName.Text);
                                }
                            }
                        }
                        else
                        {
                            // file selected
                            bool updateGistFile = false;
                            bool renameGistFile = false;
                            if (file.Filename == tbGistName.Text)
                            {
                                if (MessageBox.Show(
                                        $"Do you want to update file \"{file.Filename}\" from gist \"{gistName}\"?",
                                    string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    updateGistFile = true;
                                }
                            }
                            else
                            {
                                if (MessageBox.Show(
                                        $"Do you want to update and rename file \"{file.Filename}\" to \"{tbGistName.Text}\" from gist \"{gistName}\"?",
                                    string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    renameGistFile = true;
                                }
                            }

                            if (updateGistFile)
                            {
                                var updatedGist = await UpdateOrCreateFileInGist(gist);
                                if (cbCloseDialog.Checked)
                                {
                                    closeDialog = true;
                                }
                                else
                                {
                                    GuiUtils.RebuildTreeView(tvGists, paginator.Gists, true);
                                    SelectFileInGist(updatedGist, tbGistName.Text);
                                }
                            }
                            else if (renameGistFile)
                            {
                                var updatedGist = await RenameFileInGist(gist, file);
                                if (cbCloseDialog.Checked)
                                {
                                    closeDialog = true;
                                }
                                else
                                {
                                    GuiUtils.RebuildTreeView(tvGists, paginator.Gists, true);
                                    SelectFileInGist(updatedGist, tbGistName.Text);
                                }
                            }
                        }
                    }
                }

                if (closeDialog)
                    Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save gist." + Environment.NewLine + "Error message: " + ex.Message);
            }
        }

        private async Task<Gist> CreateGist()
        {
            var fileContent = PluginBase.GetCurrentFileText();
            var creatingGist = new UpdatedGist
            {
                Description = tbDescription.Text,
                Public = cbPublic.Checked,
                Files = new Dictionary<string, UpdatedFile>
                {
                    {tbGistName.Text, new UpdatedFile {Content = fileContent}}
                }
            };
            return await Main.GitHubService.SendJsonRequestAsync<Gist>("gists", HttpMethod.Post, creatingGist);
        }

        private async Task<Gist> UpdateOrCreateFileInGist(Gist gist)
        {
            var fileContent = PluginBase.GetCurrentFileText();
            var editingGist = new UpdatedGist();
            if (gist.Description != tbDescription.Text)
                editingGist.Description = tbDescription.Text;
            editingGist.Files = new Dictionary<string, UpdatedFile>
            {
                { tbGistName.Text, new UpdatedFile { Content = fileContent }}
            };
            var responseGist =
                await Main.GitHubService.SendJsonRequestAsync<Gist>($"gists/{gist.Id}", GitHubService.PatchHttpMethod,
                    editingGist);
            paginator.Gists[gist.Id] = responseGist;
            return responseGist;
        }

        private async Task<Gist> RenameFileInGist(Gist gist, GistFile file)
        {
            var fileContent = PluginBase.GetCurrentFileText();
            var editingGist = new UpdatedGist();
            if (gist.Description != tbDescription.Text)
                editingGist.Description = tbDescription.Text;
            editingGist.Files = new Dictionary<string, UpdatedFile>
            {
                { file.Filename, new UpdatedFile { Filename = tbGistName.Text, Content = fileContent }}
            };
            var responseGist =
                await Main.GitHubService.SendJsonRequestAsync<Gist>($"gists/{gist.Id}", GitHubService.PatchHttpMethod,
                    editingGist);
            paginator.Gists[gist.Id] = responseGist;
            return responseGist;
        }

        private void SelectFileInGist(Gist gist, string filename = null)
        {
            var node = tvGists.Nodes.Find(gist.Id + "/" +
                (!string.IsNullOrEmpty(tbGistName.Text) ? tbGistName.Text : gist.Files.First().Key), true);
            if (node.Length > 0)
            {
                BeginInvoke((Action)delegate
                {
                    if (node.Length > 0)
                    {
                        node[0].EnsureVisible();
                        tvGists.Select();
                        tvGists.SelectedNode = node[0];
                    }
                });
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e) => await paginator.UpdateGists(PageStatus.Update);

        private async void btnDelete_Click(object sender, EventArgs e) =>
            await GuiUtils.DeleteItem(tvGists, paginator.Gists, true);

        private async void btnRename_Click(object sender, EventArgs e) =>
            await GuiUtils.RenameItem(tvGists, paginator.Gists, true);

        private void tvGists_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvGists.SelectedNode != null)
            {
                if (tvGists.SelectedNode.Name == GuiUtils.AllGistsKey)
                {
                    ToggleInputElementsEnable(true);
                    btnOpen.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRename.Enabled = false;
                    cbPublic.Enabled = true;
                    cbPublic.Checked = false;
                    tbGistName.Text = string.Empty;
                    tbGistLink.Text = string.Empty;
                }
                else
                {
                    var parts = tvGists.SelectedNode.Name.Split('/');
                    var gist = paginator.Gists[parts[0]];
                    var file = gist.Files[parts[1]];
                    if (gist.Files.Count == 1 || tvGists.SelectedNode.Parent.Name != GuiUtils.AllGistsKey)
                    {
                        ToggleInputElementsEnable(true);
                        btnOpen.Enabled = true;
                        if (gist.Files.Count == 1)
                            tbGistLink.Text = gist.HtmlUrl;
                        else if (gist.Files.Count > 1)
                            tbGistLink.Text = gist.HtmlUrl + "#file-" + Utils.ReplaceNotCharactersOnHyphens(file.Filename);
                    }
                    else
                    {
                        ToggleInputElementsEnable(false);
                        btnOpen.Enabled = false;
                        tbGistLink.Text = gist.HtmlUrl;
                    }

                    btnDelete.Enabled = true;
                    cbPublic.Enabled = false;
                    cbPublic.Checked = gist.Public;
                    tbGistName.Text = file.Filename;
                    tbCreateDate.Text = gist.CreatedAt.ToString(CultureInfo.CurrentCulture);
                    tbUpdateDate.Text = gist.UpdatedAt.ToString(CultureInfo.CurrentCulture);
                    tbDescription.Text = gist.Description;
                }
            }
            else
            {
                ToggleInputElementsEnable(false);
                btnDelete.Enabled = false;
                cbPublic.Enabled = false;
            }
        }

        private void ToggleInputElementsEnable(bool enabled)
        {
            if (enabled)
            {
                btnSave.Enabled = true;
                cmbLanguage.Enabled = true;
                tbGistName.Enabled = true;
                tbDescription.Enabled = true;
                btnRename.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                cmbLanguage.Enabled = false;
                tbGistName.Enabled = false;
                tbDescription.Enabled = false;
                btnRename.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeDialog = true;
        }

        private async void tvGists_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                await GuiUtils.DeleteItem(tvGists, paginator.Gists, true);
            else if (e.KeyCode == Keys.F2)
                await GuiUtils.RenameItem(tvGists, paginator.Gists, true);
        }

        private void tbGistName_TextChanged(object sender, EventArgs e)
        {
            detectExtensionTimer.Change(200, Timeout.Infinite);
        }

        private void tbGistLink_Click(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                ((TextBox)sender).SelectAll();
            });
        }

        private void cbCloseDialog_CheckedChanged(object sender, EventArgs e)
        {
            Main.CloseDialog = cbCloseDialog.Checked;
            Win32.WritePrivateProfileString("Settings", "CloseSaveDialog",
                Convert.ToInt32(Main.CloseDialog).ToString(), Main.IniFileName);
        }

        private void btnGoToGitHub_Click(object sender, EventArgs e)
        {
            GuiUtils.GoToGitHub(tbGistLink.Text);
        }

        private async void btnPrevPage_Click(object sender, EventArgs e) => await paginator.UpdateGists(PageStatus.Prev);

        private async void btnNextPage_Click(object sender, EventArgs e) => await paginator.UpdateGists(PageStatus.Next);

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                closeDialog = false;
                if (tvGists.SelectedNode != null)
                {
                    var parts = tvGists.SelectedNode.Name.Split('/');
                    var gist = paginator.Gists[parts[0]];
                    var file = gist.Files[parts[1]];
                    var fileContent = await Main.GitHubService.SendRequestAsync(file.RawUrl);

                    if (!cbSaveToLocal.Checked)
                    {
                        string clipboardText = null;
                        if (Clipboard.ContainsText())
                        {
                            clipboardText = Clipboard.GetText();
                            Clipboard.SetText(" ");
                        }

                        Win32.SendMessage(PluginBase.NppData._nppHandle, (uint) NppMsg.NPPM_MENUCOMMAND, 0,
                            NppMenuCmd.IDM_FILE_NEW);
                        PluginBase.SetCurrentFileText(fileContent.Remove(50));
                        PluginBase.AppendTextToCurrentFile(fileContent.Substring(50));
                        if (file.Language != null && Lists.GistNppLangs.TryGetValue(file.Language, out var langType))
                            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint) NppMsg.NPPM_SETCURRENTLANGTYPE, 0,
                                (int) langType);

                        if (clipboardText != null)
                            Clipboard.SetText(clipboardText);
                        if (cbCloseDialog.Checked)
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
                            else if (MessageBox.Show($"Do you want to replace existing file \"{Path.GetFileName(filename)}\"?",
                                         string.Empty, MessageBoxButtons.YesNo)
                                == DialogResult.No)
                            {
                                var dialog = new SaveFileDialog
                                {
                                    FileName = Path.GetFileName(filename),
                                    InitialDirectory = Path.GetFullPath(gistDirectory)
                                };
                                var extension = Path.GetExtension(filename);
                                dialog.Filter = string.IsNullOrEmpty(extension)
                                    ? "All files | *.*"
                                    : string.Format("(*{0} files) | *{0}", extension);
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
                            if (cbCloseDialog.Checked)
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

        private void cbSaveToLocal_CheckedChanged(object sender, EventArgs e)
        {
            Main.SaveLocally = cbSaveToLocal.Checked;
            Win32.WritePrivateProfileString("Settings", "SaveToLocal", Convert.ToInt32(Main.SaveLocally).ToString(),
                Main.IniFileName);
        }
    }
}
