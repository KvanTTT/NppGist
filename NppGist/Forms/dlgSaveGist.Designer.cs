﻿namespace NppGist.Forms
{
    partial class dlgSaveGist
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgSaveGist));
            this.label2 = new System.Windows.Forms.Label();
            this.cbPublic = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tvGists = new System.Windows.Forms.TreeView();
            this.tbUpdateDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCreateDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbGistName = new System.Windows.Forms.TextBox();
            this.btnRename = new System.Windows.Forms.Button();
            this.tbGistLink = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCloseDialog = new System.Windows.Forms.CheckBox();
            this.btnGoToGitHub = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.tbPageNumber = new System.Windows.Forms.TextBox();
            this.lblPageNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(540, 191);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 35;
            this.label2.Text = "Language:";
            // 
            // cbPublic
            // 
            this.cbPublic.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPublic.AutoSize = true;
            this.cbPublic.Enabled = false;
            this.cbPublic.Location = new System.Drawing.Point(661, 228);
            this.cbPublic.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbPublic.Name = "cbPublic";
            this.cbPublic.Size = new System.Drawing.Size(77, 24);
            this.cbPublic.TabIndex = 33;
            this.cbPublic.Text = "Public";
            this.cbPublic.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(732, 631);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(147, 36);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tvGists
            // 
            this.tvGists.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tvGists.Location = new System.Drawing.Point(18, 59);
            this.tvGists.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tvGists.Name = "tvGists";
            this.tvGists.Size = new System.Drawing.Size(498, 553);
            this.tvGists.TabIndex = 30;
            this.tvGists.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGists_AfterSelect);
            this.tvGists.DoubleClick += new System.EventHandler(this.tvGists_DoubleClick);
            this.tvGists.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvGists_KeyUp);
            // 
            // tbUpdateDate
            // 
            this.tbUpdateDate.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUpdateDate.Location = new System.Drawing.Point(656, 141);
            this.tbUpdateDate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbUpdateDate.Name = "tbUpdateDate";
            this.tbUpdateDate.ReadOnly = true;
            this.tbUpdateDate.Size = new System.Drawing.Size(221, 26);
            this.tbUpdateDate.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(540, 147);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 28;
            this.label6.Text = "Updated:";
            // 
            // tbCreateDate
            // 
            this.tbCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCreateDate.Location = new System.Drawing.Point(656, 101);
            this.tbCreateDate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbCreateDate.Name = "tbCreateDate";
            this.tbCreateDate.ReadOnly = true;
            this.tbCreateDate.Size = new System.Drawing.Size(221, 26);
            this.tbCreateDate.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(540, 107);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "Created:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(540, 308);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 25;
            this.label4.Text = "Description:";
            // 
            // tbDescription
            // 
            this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDescription.Enabled = false;
            this.tbDescription.Location = new System.Drawing.Point(545, 347);
            this.tbDescription.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(332, 265);
            this.tbDescription.TabIndex = 24;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(545, 631);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(147, 36);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 22;
            this.label1.Text = "Available Gists";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.Enabled = false;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(656, 187);
            this.cmbLanguage.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(221, 28);
            this.cmbLanguage.TabIndex = 37;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(18, 666);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(147, 36);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(540, 268);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 39;
            this.label3.Text = "Name:";
            // 
            // tbGistName
            // 
            this.tbGistName.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGistName.Enabled = false;
            this.tbGistName.Location = new System.Drawing.Point(656, 263);
            this.tbGistName.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbGistName.Name = "tbGistName";
            this.tbGistName.Size = new System.Drawing.Size(221, 26);
            this.tbGistName.TabIndex = 40;
            this.tbGistName.TextChanged += new System.EventHandler(this.tbGistName_TextChanged);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRename.Location = new System.Drawing.Point(174, 666);
            this.btnRename.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(147, 36);
            this.btnRename.TabIndex = 41;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // tbGistLink
            // 
            this.tbGistLink.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGistLink.Location = new System.Drawing.Point(656, 61);
            this.tbGistLink.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbGistLink.Name = "tbGistLink";
            this.tbGistLink.ReadOnly = true;
            this.tbGistLink.Size = new System.Drawing.Size(178, 26);
            this.tbGistLink.TabIndex = 43;
            this.tbGistLink.Click += new System.EventHandler(this.tbGistLink_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(540, 67);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 20);
            this.label7.TabIndex = 42;
            this.label7.Text = "Link:";
            // 
            // cbCloseDialog
            // 
            this.cbCloseDialog.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCloseDialog.AutoSize = true;
            this.cbCloseDialog.Location = new System.Drawing.Point(548, 678);
            this.cbCloseDialog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbCloseDialog.Name = "cbCloseDialog";
            this.cbCloseDialog.Size = new System.Drawing.Size(124, 24);
            this.cbCloseDialog.TabIndex = 44;
            this.cbCloseDialog.Text = "Close Dialog";
            this.cbCloseDialog.UseVisualStyleBackColor = true;
            this.cbCloseDialog.CheckedChanged += new System.EventHandler(this.cbCloseDialog_CheckedChanged);
            // 
            // btnGoToGitHub
            // 
            this.btnGoToGitHub.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoToGitHub.Image = global::NppGist.Properties.Resources.hyperlink;
            this.btnGoToGitHub.Location = new System.Drawing.Point(845, 59);
            this.btnGoToGitHub.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnGoToGitHub.Name = "btnGoToGitHub";
            this.btnGoToGitHub.Size = new System.Drawing.Size(36, 37);
            this.btnGoToGitHub.TabIndex = 45;
            this.btnGoToGitHub.UseVisualStyleBackColor = true;
            this.btnGoToGitHub.Click += new System.EventHandler(this.btnGoToGitHub_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Image = ((System.Drawing.Image) (resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(476, 9);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(42, 43);
            this.btnUpdate.TabIndex = 32;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNextPage.Enabled = false;
            this.btnNextPage.Location = new System.Drawing.Point(171, 628);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(35, 26);
            this.btnNextPage.TabIndex = 55;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevPage.Enabled = false;
            this.btnPrevPage.Location = new System.Drawing.Point(127, 628);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(35, 26);
            this.btnPrevPage.TabIndex = 54;
            this.btnPrevPage.Text = "<";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // tbPageNumber
            // 
            this.tbPageNumber.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPageNumber.Location = new System.Drawing.Point(73, 628);
            this.tbPageNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPageNumber.Name = "tbPageNumber";
            this.tbPageNumber.ReadOnly = true;
            this.tbPageNumber.Size = new System.Drawing.Size(45, 26);
            this.tbPageNumber.TabIndex = 53;
            this.tbPageNumber.Text = "1";
            // 
            // lblPageNumber
            // 
            this.lblPageNumber.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPageNumber.Location = new System.Drawing.Point(18, 631);
            this.lblPageNumber.Name = "lblPageNumber";
            this.lblPageNumber.Size = new System.Drawing.Size(48, 23);
            this.lblPageNumber.TabIndex = 52;
            this.lblPageNumber.Text = "Page";
            // 
            // dlgSaveGist
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(910, 717);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.tbPageNumber);
            this.Controls.Add(this.lblPageNumber);
            this.Controls.Add(this.btnGoToGitHub);
            this.Controls.Add(this.cbCloseDialog);
            this.Controls.Add(this.tbGistLink);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.tbGistName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.label2);
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
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSaveGist";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Gist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSaveGist_FormClosing);
            this.Load += new System.EventHandler(this.frmSaveGist_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblPageNumber;
        private System.Windows.Forms.TextBox tbPageNumber;

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbPublic;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TreeView tvGists;
        private System.Windows.Forms.TextBox tbUpdateDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCreateDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbGistName;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.TextBox tbGistLink;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbCloseDialog;
        private System.Windows.Forms.Button btnGoToGitHub;
        private System.Windows.Forms.ToolTip toolTip;
    }
}