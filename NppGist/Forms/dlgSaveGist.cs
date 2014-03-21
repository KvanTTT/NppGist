using NppPluginNET;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NppGist.Forms
{
	public partial class dlgSaveGist : Form
	{
		Dictionary<string, Gist> Gists;
		System.Threading.Timer DetectExtensionTimer;
		bool CloseDialog;

		public dlgSaveGist()
		{
			InitializeComponent();

			foreach (var lang in Lists.GistLangs)
				cmbLanguage.Items.Add(lang);

			LangType langType = LangType.L_TEXT;
			Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTLANGTYPE, 0, ref langType);
			var gistNppLang = Lists.GistNppLangs.FirstOrDefault(lang => lang.Value == langType);
			if (gistNppLang.Key != null)
				cmbLanguage.SelectedItem = gistNppLang.Key;
			else
				cmbLanguage.SelectedItem = Lists.GistLangs[0];
			cbCloseDialog.Checked = Main.CloseSaveDialog;

			DetectExtensionTimer = new System.Threading.Timer(_ => GuiUtils.UpdateExtenstionResult(cmbLanguage, tbGistName), null, 0, Timeout.Infinite);
		}

		private void frmSaveGist_Load(object sender, EventArgs e)
		{
			btnUpdate_Click(sender, e);
			var currentFileName = PluginBase.GetFullCurrentFileName();
			tvGists.Select();
			if (currentFileName.StartsWith("new"))
			{
				tvGists.SelectedNode = tvGists.Nodes[0];
			}
			else
			{
				tvGists.SelectedNode = null;
				var shortFileName = Path.GetFileName(currentFileName);
				foreach (var keyGist in Gists)
				{
					var gist = keyGist.Value;
					if (gist.Files.Count == 1 && gist.Files.First().Key.StartsWith("gistfile"))
					{
						if (shortFileName == "gist-" + gist.Id)
						{
							tvGists.SelectedNode = tvGists.Nodes.Find(GuiUtils.GetTreeViewKey(gist, gist.Files.First().Value), true)[0];
							tvGists.SelectedNode.EnsureVisible();
							break;
						}
					}
					else
					{
						var gistFile = gist.Files.FirstOrDefault(file => Utils.GetSafeFilename(file.Key) == shortFileName);
						if (gistFile.Key != null)
						{
							tvGists.SelectedNode = tvGists.Nodes.Find(GuiUtils.GetTreeViewKey(gist, gistFile.Value), true)[0];
							tvGists.SelectedNode.EnsureVisible();
							break;
						}
					}
				}
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
				CloseDialog = false;
				if (tvGists.SelectedNode != null)
				{
					//if (Utils.IsFilenameSafe(tbGistName.Text))
					{
						if (tvGists.SelectedNode.Name == GuiUtils.AllGistsKey)
						{
							// Creating new gist
							var createdGist = CreateGist();
							if (cbCloseDialog.Checked)
								CloseDialog = true;
							else
							{
								GuiUtils.RebuildTreeView(tvGists, Gists, true);
								SelectFileInGist(createdGist, "");
							}
						}
						else
						{
							// Updating existing gist
							var strs = tvGists.SelectedNode.Name.Split('/');
							var gist = Gists[strs[0]];
							var file = gist.Files[strs[1]];
							var gistName = GuiUtils.GetGistName(gist);

							if (gist.Files.Count == 1)
							{
								bool updateGistFile = false;
								bool createGistFile = false;
								if (file.Filename == tbGistName.Text)
								{
									if (MessageBox.Show(string.Format("Do you want to update gist \"{0}\"?", gistName),
										string.Empty, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
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
										CloseDialog = true;
									else
									{
										GuiUtils.RebuildTreeView(tvGists, Gists, true);
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
									if (MessageBox.Show(string.Format(
										"Do you want to update file \"{0}\" from gist \"{1}\"?", file.Filename, gistName),
										string.Empty, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
									{
										updateGistFile = true;
									}
								}
								else
								{
									if (MessageBox.Show(string.Format(
										"Do you want to update and rename file \"{0}\" to \"{1}\" from gist \"{2}\"?",
										file.Filename, tbGistName.Text, gistName),
										string.Empty, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
									{
										renameGistFile = true;
									}
								}

								if (updateGistFile)
								{
									var updatedGist = UpdateOrCreateFileInGist(gist);
									if (cbCloseDialog.Checked)
										CloseDialog = true;
									else
									{
										GuiUtils.RebuildTreeView(tvGists, Gists, true);
										SelectFileInGist(updatedGist, tbGistName.Text);
									}
								}
								else if (renameGistFile)
								{
									var updatedGist = RenameFileInGist(gist, file);
									if (cbCloseDialog.Checked)
										CloseDialog = true;
									else
									{
										GuiUtils.RebuildTreeView(tvGists, Gists, true);
										SelectFileInGist(updatedGist, tbGistName.Text);
									}
								}
							}
						}
					}
					/*else
					{
						MessageBox.Show("File name contains invalid characters");
					}*/
				}
				if (sender is TreeView && CloseDialog)
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
			var creatingGist = new UpdatedGist();
			creatingGist.Description = tbDescription.Text;
			creatingGist.Public = cbPublic.Checked;
			creatingGist.Files = new Dictionary<string, UpdatedFile>()
			{
				{ tbGistName.Text, new UpdatedFile { Content = fileContent }}
			};
			var bytes = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString<UpdatedGist>(creatingGist));

			var gist = Utils.SendJsonRequest<Gist>(string.Format("{0}/gists?access_token={1}", Main.ApiUrl, Main.Token),
				WebRequestMethod.Post, null, bytes);
			Gists.Add(gist.Id, gist);
			Gists = Gists.OrderByDescending(g => g.Value.CreatedAt).ToDictionary(g => g.Key, g => g.Value);
			return gist;
		}

		private Gist UpdateOrCreateFileInGist(Gist gist)
		{
			var fileContent = PluginBase.GetCurrentFileText();
			var editingGist = new UpdatedGist();
			if (gist.Description != tbDescription.Text)
				editingGist.Description = tbDescription.Text;
			editingGist.Files = new Dictionary<string, UpdatedFile>()
			{
				{ tbGistName.Text, new UpdatedFile { Content = fileContent }}
			};
			var bytes = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString<UpdatedGist>(editingGist));

			var responseGist = Utils.SendJsonRequest<Gist>(string.Format("{0}/gists/{1}?access_token={2}", Main.ApiUrl, gist.Id, Main.Token),
				WebRequestMethod.Patch, null, bytes);
			Gists[gist.Id] = responseGist;
			return responseGist;
		}

		private Gist RenameFileInGist(Gist gist, GistFile file)
		{
			var fileContent = PluginBase.GetCurrentFileText();
			var editingGist = new UpdatedGist();
			if (gist.Description != tbDescription.Text)
				editingGist.Description = tbDescription.Text;
			editingGist.Files = new Dictionary<string, UpdatedFile>()
			{
				{ file.Filename, new UpdatedFile { Filename = tbGistName.Text, Content = fileContent }}
			};
			var bytes = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString<UpdatedGist>(editingGist));

			var responseGist = Utils.SendJsonRequest<Gist>(string.Format("{0}/gists/{1}?access_token={2}", Main.ApiUrl, gist.Id, Main.Token),
				WebRequestMethod.Patch, null, bytes);
			Gists[gist.Id] = responseGist;
			return responseGist;
		}

		private void SelectFileInGist(Gist gist, string filename = null)
		{
			var node = tvGists.Nodes.Find(gist.Id + "/" +
				(!string.IsNullOrEmpty(tbGistName.Text) ? tbGistName.Text : gist.Files.First().Key), true);
			if (node != null && node.Length > 0)
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
			try
			{
				var gists = Utils.SendJsonRequest<List<Gist>>(string.Format("{0}/gists?access_token={1}", Main.ApiUrl, Main.Token));
				Gists = gists.ToDictionary<Gist, string>(gist => gist.Id);
				GuiUtils.RebuildTreeView(tvGists, Gists, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Unable to connect to api.github.com. Try to refresh.{0}Error message: {1}",
					Environment.NewLine, ex.Message));
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			GuiUtils.DeleteItem(tvGists, Gists, true);
		}

		private void btnRename_Click(object sender, EventArgs e)
		{
			GuiUtils.RenameItem(tvGists, Gists, true);
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
					cbPublic.Checked = true;
					tbGistName.Text = string.Empty;
					//cmbLanguage.SelectedItem = Lists.GistLangs[0];
				}
				else
				{
					var strs = tvGists.SelectedNode.Name.Split('/');
					var gist = Gists[strs[0]];
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

		private void cbPublic_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CloseDialog = true;
		}

		private void tvGists_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				GuiUtils.DeleteItem(tvGists, Gists, true);
			else if (e.KeyCode == Keys.F2)
				GuiUtils.RenameItem(tvGists, Gists, true);
		}

		private void tbGistName_KeyPress(object sender, KeyPressEventArgs e)
		{
			//GuiUtils.TextBoxKeyPressRestrictInvalidFilenameChars(e);
		}

		private void tbGistName_TextChanged(object sender, EventArgs e)
		{
			//GuiUtils.TextBoxTextChangedRestrictInvalidFilenameChars((TextBox)sender);

			DetectExtensionTimer.Change(200, Timeout.Infinite);
		}

		private void frmSaveGist_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.None && !CloseDialog)
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
			Win32.WritePrivateProfileString("Settings", "CloseSaveDialog", (Convert.ToInt32(Main.CloseSaveDialog)).ToString(), Main.IniFileName);
		}

		private void btnGoToGitHub_Click(object sender, EventArgs e)
		{
			GuiUtils.GoToGitHub(tbGistLink.Text);
		}
	}
}
