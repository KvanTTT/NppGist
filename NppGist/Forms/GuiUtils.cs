using NppPluginNET;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace NppGist.Forms
{
	internal class GuiUtils
	{
		internal const string AllGistsKey = "allgists";
		internal static Color SecretGistColor = Color.FromArgb(255, 248, 238, 199);
		internal static Color SecretGistForeColor = Color.FromArgb(255, 161, 136, 43);

		public static DialogResult DeleteItem(TreeView treeView, Dictionary<string, Gist> gists, bool showRoot)
		{
			DialogResult result = DialogResult.None;
			if (treeView.SelectedNode != null && treeView.SelectedNode.Name != AllGistsKey)
			{
				try
				{
					var strs = treeView.SelectedNode.Name.Split('/');
					var gist = gists[strs[0]];
					var file = gist.Files[strs[1]];
					if (treeView.SelectedNode.Parent == null ||
						treeView.SelectedNode.Parent.Name == AllGistsKey)
					{
						if ((result = MessageBox.Show(string.Format("Do you want to delete gist \"{0}\"?", file.Filename), string.Empty, MessageBoxButtons.YesNo))
							== System.Windows.Forms.DialogResult.Yes)
						{
							Utils.SendRequest(string.Format("{0}/gists/{1}?access_token={2}", Main.ApiUrl, gist.Id, Main.Token), WebRequestMethod.Delete);
							gists.Remove(gist.Id);
							GuiUtils.RebuildTreeView(treeView, gists, showRoot);
						}
					}
					else
					{
						if ((result = MessageBox.Show(string.Format("Do you want to delete file \"{0}\" from gist \"{1}\"?", file.Filename, gist.Files.First().Value.Filename), string.Empty, MessageBoxButtons.YesNo)) == System.Windows.Forms.DialogResult.Yes)
						{
							string body = string.Format("{{\"files\":{{\"{0}\":null}}}}", file.Filename);
							var responseGist = Utils.SendJsonRequest<Gist>(string.Format("{0}/gists/{1}?access_token={2}", Main.ApiUrl, gist.Id, Main.Token),
								WebRequestMethod.Patch, null, Encoding.UTF8.GetBytes(body));
							gists[gist.Id] = responseGist;
							GuiUtils.RebuildTreeView(treeView, gists, showRoot);
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Unable to delete gist or file." + Environment.NewLine + "Error message: " + ex.Message);
				}
			}
			return result;
		}

		public static DialogResult RenameItem(TreeView treeView, Dictionary<string, Gist> gists, bool showRoot)
		{
			DialogResult result = DialogResult.None;
			if (treeView.SelectedNode != null && treeView.SelectedNode.Name != AllGistsKey)
			{
				var strs = treeView.SelectedNode.Name.Split('/');
				var gist = gists[strs[0]];
				var file = gist.Files[strs[1]];

				var fileNameDialog = new dlgFilename(file.Filename);
				if ((result = fileNameDialog.ShowDialog()) == DialogResult.OK &&
					fileNameDialog.Filename != file.Filename)
				{
					if (Utils.IsFilenameSafe(fileNameDialog.Filename))
					{
						string oldFileName = file.Filename;
						bool save = true;
						if (gist.Files.FirstOrDefault(gistFile => gistFile.Key == fileNameDialog.Filename).Key != null &&
							(result = MessageBox.Show(string.Format("Do you want to replace existing file \"{0}\"", fileNameDialog.Filename)
								, string.Empty, MessageBoxButtons.YesNo)) != DialogResult.Yes)
						{
							save = false;
							result = DialogResult.No;
						}

						if (save)
						{
							var fileContent = Utils.SendRequest(file.RawUrl);
							var renamingGist = new UpdatedGist();
							renamingGist.Files = new Dictionary<string, UpdatedFile>()
							{
								{ file.Filename,
								new UpdatedFile
								{
									Filename = fileNameDialog.Filename,
									Content = fileContent
								}}
							};
							var bytes = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString<UpdatedGist>(renamingGist));

							try
							{
								var responseGist = Utils.SendJsonRequest<Gist>(string.Format("{0}/gists/{1}?access_token={2}", Main.ApiUrl, gist.Id, Main.Token),
									WebRequestMethod.Patch, null, bytes);
								gists[gist.Id] = responseGist;
								RebuildTreeView(treeView, gists, showRoot);
							}
							catch (Exception ex)
							{
								MessageBox.Show(string.Format("Unable to reaname gist \"{0}\".{1}Error message: {2}",
									file.Filename, Environment.NewLine, ex.Message));
							}
						}
					}
					else
					{
						MessageBox.Show("File containts invalid characters");
					}
				}
			}
			return result;
		}

		public static void RebuildTreeView(TreeView treeView, Dictionary<string, Gist> gists, bool showRoot)
		{
			treeView.Nodes.Clear();
			TreeNodeCollection collection;
			if (showRoot)
				collection = treeView.Nodes.Add(AllGistsKey, "All Gists").Nodes;
			else
				collection = treeView.Nodes;

			foreach (var gist in gists)
			{
				var value = gist.Value;
				if (value.Files.Count == 1)
				{
					var file = value.Files.First().Value;
					var node = new TreeNode(GetGistName(value));
					node.Name = GetTreeViewKey(value, file);
					if (!gist.Value.Public)
					{
						node.BackColor = SecretGistColor;
						node.ForeColor = SecretGistForeColor;
					}
					collection.Add(node);
				}
				else if (value.Files.Count > 1)
				{
					var firstFile = value.Files.First().Value;
					var parent = new TreeNode(GetGistName(value));
					parent.Name = GetTreeViewKey(value, firstFile);
					if (!gist.Value.Public)
					{ 
						parent.BackColor = SecretGistColor;
						parent.ForeColor = SecretGistForeColor;
					}
					collection.Add(parent);
					foreach (var file in value.Files)
					{
						var fileValue = file.Value;
						var node = parent.Nodes.Add(GetTreeViewKey(value, fileValue), fileValue.Filename);
						if (!gist.Value.Public)
						{
							node.BackColor = SecretGistColor;
							node.ForeColor = SecretGistForeColor;
						}
					}
				}
			}
			treeView.ExpandAll();
			treeView.Nodes[0].EnsureVisible();
		}

		public static void TextBoxKeyPressRestrictInvalidFilenameChars(KeyPressEventArgs e)
		{
			e.Handled = !Lists.ControlChars.Contains(e.KeyChar) && Lists.InvalidFilenameCharacters.Contains(e.KeyChar);
		}

		public static void TextBoxTextChangedRestrictInvalidFilenameChars(TextBox textBox)
		{
			if (textBox.Text.IndexOfAny(Lists.InvalidFilenameCharacters) != -1)
			{
				var correctedText = new StringBuilder(textBox.Text);
				foreach (var c in Lists.InvalidFilenameCharacters)
					correctedText.Replace(c.ToString(), "");
				textBox.Text = correctedText.ToString();
				textBox.Select(textBox.TextLength, 0);
			}
		}

		public static void UpdateExtenstionResult(ComboBox cmbLanguage, TextBox tbFilename)
		{
			if (tbFilename.Text == "")
			{
				cmbLanguage.Enabled = true;
				cmbLanguage.SelectedItem = Lists.GistLangs[0];
			}
			else
			{
				cmbLanguage.Enabled = false;
				//languages/detect? is no longer supported by api. See here https://developer.github.com/v3/gists/
				cmbLanguage.SelectedItem = "none";
			}
		}

		public static void GoToGitHub(string link)
		{
			Process.Start(!string.IsNullOrEmpty(link) ? link : string.Format("{0}/{1}", Main.GistUrl, Main.Login));
		}

		public static string GetTreeViewKey(Gist gist, GistFile file)
		{
			return gist.Id + "/" + file.Filename;
		}

		public static GistFile GetFirstNamed(Gist gist)
		{
			var result = gist.Files.FirstOrDefault(g => !g.Key.StartsWith("gistfile"));
			return result.Key != null ? result.Value : null;
		}

		public static string GetGistName(Gist gist)
		{
			var firstNamedFile = GetFirstNamed(gist);
			return firstNamedFile == null ? "gist:" + gist.Id : firstNamedFile.Filename;
		}
	}
}
