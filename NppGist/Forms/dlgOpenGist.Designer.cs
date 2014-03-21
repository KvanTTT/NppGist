namespace NppGist.Forms
{
	partial class dlgOpenGist
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgOpenGist));
			this.label1 = new System.Windows.Forms.Label();
			this.btnOpen = new System.Windows.Forms.Button();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbCreateDate = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbUpdateDate = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tvGists = new System.Windows.Forms.TreeView();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbPublic = new System.Windows.Forms.CheckBox();
			this.cbSaveToLocal = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbLanguage = new System.Windows.Forms.TextBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnRename = new System.Windows.Forms.Button();
			this.tbGistLink = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cbCloseOpenDialog = new System.Windows.Forms.CheckBox();
			this.btnGoToGitHub = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Available Gists";
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOpen.Enabled = false;
			this.btnOpen.Location = new System.Drawing.Point(351, 417);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(98, 23);
			this.btnOpen.TabIndex = 5;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// tbDescription
			// 
			this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDescription.Location = new System.Drawing.Point(351, 179);
			this.tbDescription.Multiline = true;
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.ReadOnly = true;
			this.tbDescription.Size = new System.Drawing.Size(239, 202);
			this.tbDescription.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(348, 155);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Description:";
			// 
			// tbCreateDate
			// 
			this.tbCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCreateDate.Location = new System.Drawing.Point(425, 102);
			this.tbCreateDate.Name = "tbCreateDate";
			this.tbCreateDate.ReadOnly = true;
			this.tbCreateDate.Size = new System.Drawing.Size(165, 20);
			this.tbCreateDate.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(348, 105);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(47, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Created:";
			// 
			// tbUpdateDate
			// 
			this.tbUpdateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUpdateDate.Location = new System.Drawing.Point(425, 128);
			this.tbUpdateDate.Name = "tbUpdateDate";
			this.tbUpdateDate.ReadOnly = true;
			this.tbUpdateDate.Size = new System.Drawing.Size(165, 20);
			this.tbUpdateDate.TabIndex = 13;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(348, 131);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(51, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Updated:";
			// 
			// tvGists
			// 
			this.tvGists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tvGists.Location = new System.Drawing.Point(12, 45);
			this.tvGists.Name = "tvGists";
			this.tvGists.Size = new System.Drawing.Size(319, 336);
			this.tvGists.TabIndex = 14;
			this.tvGists.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGists_AfterSelect);
			this.tvGists.DoubleClick += new System.EventHandler(this.tvGists_DoubleClick);
			this.tvGists.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvGists_KeyUp);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(456, 417);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(98, 23);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cbPublic
			// 
			this.cbPublic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPublic.AutoSize = true;
			this.cbPublic.Enabled = false;
			this.cbPublic.Location = new System.Drawing.Point(425, 21);
			this.cbPublic.Name = "cbPublic";
			this.cbPublic.Size = new System.Drawing.Size(55, 17);
			this.cbPublic.TabIndex = 18;
			this.cbPublic.Text = "Public";
			this.cbPublic.UseVisualStyleBackColor = true;
			// 
			// cbSaveToLocal
			// 
			this.cbSaveToLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSaveToLocal.AutoSize = true;
			this.cbSaveToLocal.Location = new System.Drawing.Point(456, 394);
			this.cbSaveToLocal.Name = "cbSaveToLocal";
			this.cbSaveToLocal.Size = new System.Drawing.Size(88, 17);
			this.cbSaveToLocal.TabIndex = 19;
			this.cbSaveToLocal.Text = "Save to local";
			this.cbSaveToLocal.UseVisualStyleBackColor = true;
			this.cbSaveToLocal.CheckedChanged += new System.EventHandler(this.cbSaveToLocal_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(348, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 20;
			this.label2.Text = "Language:";
			// 
			// tbLanguage
			// 
			this.tbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLanguage.Location = new System.Drawing.Point(425, 76);
			this.tbLanguage.Name = "tbLanguage";
			this.tbLanguage.ReadOnly = true;
			this.tbLanguage.Size = new System.Drawing.Size(165, 20);
			this.tbLanguage.TabIndex = 21;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(12, 390);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(98, 23);
			this.btnDelete.TabIndex = 22;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnRename
			// 
			this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRename.Enabled = false;
			this.btnRename.Location = new System.Drawing.Point(116, 390);
			this.btnRename.Name = "btnRename";
			this.btnRename.Size = new System.Drawing.Size(98, 23);
			this.btnRename.TabIndex = 23;
			this.btnRename.Text = "Rename";
			this.btnRename.UseVisualStyleBackColor = true;
			this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
			// 
			// tbGistLink
			// 
			this.tbGistLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGistLink.Location = new System.Drawing.Point(425, 50);
			this.tbGistLink.Name = "tbGistLink";
			this.tbGistLink.ReadOnly = true;
			this.tbGistLink.Size = new System.Drawing.Size(137, 20);
			this.tbGistLink.TabIndex = 45;
			this.tbGistLink.Enter += new System.EventHandler(this.tbGistLink_Enter);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(348, 53);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(30, 13);
			this.label7.TabIndex = 44;
			this.label7.Text = "Link:";
			// 
			// cbCloseOpenDialog
			// 
			this.cbCloseOpenDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbCloseOpenDialog.AutoSize = true;
			this.cbCloseOpenDialog.Location = new System.Drawing.Point(351, 394);
			this.cbCloseOpenDialog.Name = "cbCloseOpenDialog";
			this.cbCloseOpenDialog.Size = new System.Drawing.Size(85, 17);
			this.cbCloseOpenDialog.TabIndex = 47;
			this.cbCloseOpenDialog.Text = "Close Dialog";
			this.cbCloseOpenDialog.UseVisualStyleBackColor = true;
			this.cbCloseOpenDialog.CheckedChanged += new System.EventHandler(this.cbCloseOpenDialog_CheckedChanged);
			// 
			// btnGoToGitHub
			// 
			this.btnGoToGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGoToGitHub.Image = global::NppGist.Properties.Resources.hyperlink;
			this.btnGoToGitHub.Location = new System.Drawing.Point(566, 47);
			this.btnGoToGitHub.Name = "btnGoToGitHub";
			this.btnGoToGitHub.Size = new System.Drawing.Size(24, 24);
			this.btnGoToGitHub.TabIndex = 46;
			this.btnGoToGitHub.UseVisualStyleBackColor = true;
			this.btnGoToGitHub.Click += new System.EventHandler(this.btnGoToGitHub_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
			this.btnUpdate.Location = new System.Drawing.Point(303, 11);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(28, 28);
			this.btnUpdate.TabIndex = 16;
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// dlgOpenGist
			// 
			this.AcceptButton = this.btnOpen;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(602, 456);
			this.Controls.Add(this.cbCloseOpenDialog);
			this.Controls.Add(this.btnGoToGitHub);
			this.Controls.Add(this.tbGistLink);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.btnRename);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.tbLanguage);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbSaveToLocal);
			this.Controls.Add(this.cbPublic);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.tvGists);
			this.Controls.Add(this.tbUpdateDate);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbCreateDate);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbDescription);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "dlgOpenGist";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Open Gist";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dlgOpenGist_FormClosing);
			this.Load += new System.EventHandler(this.frmGists_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbCreateDate;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbUpdateDate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TreeView tvGists;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.CheckBox cbPublic;
		private System.Windows.Forms.CheckBox cbSaveToLocal;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbLanguage;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnRename;
		private System.Windows.Forms.TextBox tbGistLink;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnGoToGitHub;
		private System.Windows.Forms.CheckBox cbCloseOpenDialog;
	}
}