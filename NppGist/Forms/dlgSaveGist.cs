using NppNetInf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using NppGist.JsonMapping;

namespace NppGist.Forms
{
    public partial class dlgSaveGist : Form
    {
        Dictionary<string, Gist> gists;
        readonly System.Threading.Timer detectExtensionTimer;
        bool closeDialog;

        public dlgSaveGist()
        {
            InitializeComponent();

            foreach (var lang in Lists.GistLangs)
                cmbLanguage.Items.Add(lang);

            LangType langType = LangType.L_TEXT;
            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint) NppMsg.NPPM_GETCURRENTLANGTYPE, 0, ref langType);
            var gistNppLang = Lists.GistNppLangs.FirstOrDefault(lang => lang.Value == langType);
            cmbLanguage.SelectedItem = gistNppLang.Key ?? Lists.GistLangs[0];
            cbCloseDialog.Checked = Main.CloseSaveDialog;

            detectExtensionTimer = new System.Threading.Timer(_ => GuiUtils.UpdateExtenstionResult(cmbLanguage, tbGistName), null, 0, Timeout.Infinite);

            toolTip.SetToolTip(btnGoToGitHub, "Open Gist in Browser");
            toolTip.SetToolTip(btnUpdate, "Update Gists");
        }

        private void frmSaveGist_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UpdateGists())
                {
                    return;
                }

                var currentFileName = PluginBase.GetFullCurrentFileName();
                tvGists.Select();
                if (currentFileName.StartsWith("new"))
                {
                    tvGists.SelectedNode = tvGists.Nodes[0];
                }
                else
                {
                    tvGists.SelectedNode = null;
                    bool nodeFound = false;
                    var shortFileName = Path.GetFileName(currentFileName);
                    foreach (var keyGist in gists)
                    {
                        var gist = keyGist.Value;
                        if (gist.Files.Count == 1 && gist.Files.First().Key.StartsWith("gistfile"))
                        {
                            if (shortFileName == "gist-" + gist.Id)
                            {
                                tvGists.SelectedNode = tvGists.Nodes.Find(GuiUtils.GetTreeViewKey(gist, gist.Files.First().Value), true)[0];
                                nodeFound = true;
                                break;
                            }
                        }
                        else
                        {
                            var gistFile = gist.Files.FirstOrDefault(file => Utils.GetSafeFilename(file.Key) == shortFileName);
                            if (gistFile.Key != null)
                            {
                                tvGists.SelectedNode = tvGists.Nodes.Find(GuiUtils.GetTreeViewKey(gist, gistFile.Value), true)[0];
                                nodeFound = true;
                                break;
                            }
                        }
                    }
                    if (!nodeFound)
                    {
                        tvGists.SelectedNode = tvGists.Nodes[0];
                        tbGistName.Text = shortFileName;
                    }
                    tvGists.SelectedNode.EnsureVisible();
                    GuiUtils.UpdateExtenstionResult(cmbLanguage, tbGistName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load gist." + Environment.NewLine + "Error message: " + ex.Message);
            }
        }

        private void tvGists_DoubleClick(object sender, EventArgs e)
        {
            if (tvGists.SelectedNode != null)
                btnSave_Click(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                closeDialog = false;
                if (tvGists.SelectedNode != null)
                {
                    if (tvGists.SelectedNode.Name == GuiUtils.AllGistsKey)
                    {
                        // Creating new gist
                        var createdGist = CreateGist();
                        if (cbCloseDialog.Checked)
                        {
                            closeDialog = true;
                        }
                        else
                        {
                            GuiUtils.RebuildTreeView(tvGists, gists, true);
                            SelectFileInGist(createdGist, "");
                        }
                    }
                    else
                    {
                        // Updating existing gist
                        var strs = tvGists.SelectedNode.Name.Split('/');
                        var gist = gists[strs[0]];
                        var file = gist.Files[strs[1]];
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
                                var updatedGist = UpdateOrCreateFileInGist(gist);
                                if (cbCloseDialog.Checked)
                                    closeDialog = true;
                                else
                                {
                                    GuiUtils.RebuildTreeView(tvGists, gists, true);
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
                                var updatedGist = UpdateOrCreateFileInGist(gist);
                                if (cbCloseDialog.Checked)
                                    closeDialog = true;
                                else
                                {
                                    GuiUtils.RebuildTreeView(tvGists, gists, true);
                                    SelectFileInGist(updatedGist, tbGistName.Text);
                                }
                            }
                            else if (renameGistFile)
                            {
                                var updatedGist = RenameFileInGist(gist, file);
                                if (cbCloseDialog.Checked)
                                    closeDialog = true;
                                else
                                {
                                    GuiUtils.RebuildTreeView(tvGists, gists, true);
                                    SelectFileInGist(updatedGist, tbGistName.Text);
                                }
                            }
                        }
                    }
                }
                if (sender is TreeView && closeDialog)
                    Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save gist." + Environment.NewLine + "Error message: " + ex.Message);
            }
        }

        private Gist CreateGist()
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
            var gist = Utils.SendJsonRequest<Gist>($"gists", Main.Token, HttpMethod.Post, creatingGist);
            gists.Add(gist.Id, gist);
            gists = gists.OrderByDescending(g => g.Value.CreatedAt)
                .ToDictionary(g => g.Key, g => g.Value);
            return gist;
        }

        private Gist UpdateOrCreateFileInGist(Gist gist)
        {
            var fileContent = PluginBase.GetCurrentFileText();
            var editingGist = new UpdatedGist();
            if (gist.Description != tbDescription.Text)
                editingGist.Description = tbDescription.Text;
            editingGist.Files = new Dictionary<string, UpdatedFile>
            {
                { tbGistName.Text, new UpdatedFile { Content = fileContent }}
            };
            var responseGist = Utils.SendJsonRequest<Gist>($"gists/{gist.Id}", Main.Token, Utils.PatchHttpMethod, editingGist);
            gists[gist.Id] = responseGist;
            return responseGist;
        }

        private Gist RenameFileInGist(Gist gist, GistFile file)
        {
            var fileContent = PluginBase.GetCurrentFileText();
            var editingGist = new UpdatedGist();
            if (gist.Description != tbDescription.Text)
                editingGist.Description = tbDescription.Text;
            editingGist.Files = new Dictionary<string, UpdatedFile>
            {
                { file.Filename, new UpdatedFile { Filename = tbGistName.Text, Content = fileContent }}
            };
            var responseGist = Utils.SendJsonRequest<Gist>($"gists/{gist.Id}", Main.Token,
                Utils.PatchHttpMethod, editingGist);
            gists[gist.Id] = responseGist;
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
                    node[0].EnsureVisible();
                    tvGists.Select();
                    tvGists.SelectedNode = node[0];
                });
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateGists();
        }

        private bool UpdateGists()
        {
            try
            {
                var gists = Utils.SendJsonRequest<List<Gist>>("gists", Main.Token);
                this.gists = gists.ToDictionary(gist => gist.Id);
                GuiUtils.RebuildTreeView(tvGists, this.gists, true);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to connect to api.github.com. Try to refresh.{Environment.NewLine}Error message: {ex.Message}");
            }

            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GuiUtils.DeleteItem(tvGists, gists, true);
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            GuiUtils.RenameItem(tvGists, gists, true);
        }

        private void tvGists_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvGists.SelectedNode != null)
            {
                if (tvGists.SelectedNode.Name == GuiUtils.AllGistsKey)
                {
                    ToggleInputElementsEnable(true);
                    btnDelete.Enabled = false;
                    btnRename.Enabled = false;
                    cbPublic.Enabled = true;
                    cbPublic.Checked = false;
                    tbGistName.Text = string.Empty;
                    tbGistLink.Text = string.Empty;
                }
                else
                {
                    var strs = tvGists.SelectedNode.Name.Split('/');
                    var gist = gists[strs[0]];
                    var file = gist.Files[strs[1]];
                    if (gist.Files.Count == 1 || tvGists.SelectedNode.Parent.Name != GuiUtils.AllGistsKey)
                    {
                        ToggleInputElementsEnable(true);
                        if (gist.Files.Count == 1)
                            tbGistLink.Text = gist.HtmlUrl;
                        else if (gist.Files.Count > 1)
                            tbGistLink.Text = gist.HtmlUrl + "#file-" + Utils.ReplaceNotCharactersOnHyphens(file.Filename);
                    }
                    else
                    {
                        ToggleInputElementsEnable(false);
                        tbGistLink.Text = gist.HtmlUrl;
                    }

                    btnDelete.Enabled = true;
                    cbPublic.Enabled = false;
                    cbPublic.Checked = gist.Public;
                    tbGistName.Text = file.Filename;
                    tbCreateDate.Text = gist.CreatedAt.ToString();
                    tbUpdateDate.Text = gist.UpdatedAt.ToString();
                    tbDescription.Text = gist.Description;
                }
            }
            else
            {
                ToggleInputElementsEnable(false);
                btnDelete.Enabled = false;
                btnRename.Enabled = false;
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

        private void tvGists_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                GuiUtils.DeleteItem(tvGists, gists, true);
            else if (e.KeyCode == Keys.F2)
                GuiUtils.RenameItem(tvGists, gists, true);
        }

        private void tbGistName_TextChanged(object sender, EventArgs e)
        {
            detectExtensionTimer.Change(200, Timeout.Infinite);
        }

        private void frmSaveGist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None && !closeDialog)
                e.Cancel = true;
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
            Main.CloseSaveDialog = cbCloseDialog.Checked;
            Win32.WritePrivateProfileString("Settings", "CloseSaveDialog",
                Convert.ToInt32(Main.CloseSaveDialog).ToString(), Main.IniFileName);
        }

        private void btnGoToGitHub_Click(object sender, EventArgs e)
        {
            GuiUtils.GoToGitHub(tbGistLink.Text);
        }
    }
}
