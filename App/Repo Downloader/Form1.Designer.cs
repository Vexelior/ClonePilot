
namespace Repo_Downloader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            submitButton = new System.Windows.Forms.Button();
            urlEntry = new System.Windows.Forms.TextBox();
            outputBox = new System.Windows.Forms.TextBox();
            title = new System.Windows.Forms.Label();
            savePathEntry = new System.Windows.Forms.TextBox();
            pathButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            mainBranchRadioButton = new System.Windows.Forms.RadioButton();
            branchLabel = new System.Windows.Forms.Label();
            otherBanchRadioButton = new System.Windows.Forms.RadioButton();
            branchEntry = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // submitButton
            // 
            submitButton.Location = new System.Drawing.Point(321, 250);
            submitButton.Name = "submitButton";
            submitButton.Size = new System.Drawing.Size(156, 40);
            submitButton.TabIndex = 0;
            submitButton.Text = "Download";
            submitButton.UseVisualStyleBackColor = true;
            submitButton.Click += Download;
            // 
            // urlEntry
            // 
            urlEntry.Location = new System.Drawing.Point(63, 96);
            urlEntry.Name = "urlEntry";
            urlEntry.Size = new System.Drawing.Size(693, 23);
            urlEntry.TabIndex = 1;
            // 
            // outputBox
            // 
            outputBox.AcceptsReturn = true;
            outputBox.BackColor = System.Drawing.Color.LightGray;
            outputBox.Cursor = System.Windows.Forms.Cursors.No;
            outputBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            outputBox.Location = new System.Drawing.Point(12, 296);
            outputBox.Multiline = true;
            outputBox.Name = "outputBox";
            outputBox.ReadOnly = true;
            outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            outputBox.Size = new System.Drawing.Size(776, 142);
            outputBox.TabIndex = 3;
            // 
            // title
            // 
            title.AutoSize = true;
            title.BackColor = System.Drawing.Color.Transparent;
            title.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            title.ForeColor = System.Drawing.SystemColors.Window;
            title.Location = new System.Drawing.Point(267, 22);
            title.Name = "title";
            title.Size = new System.Drawing.Size(275, 28);
            title.TabIndex = 4;
            title.Text = "Repository Downloader";
            // 
            // savePathEntry
            // 
            savePathEntry.Location = new System.Drawing.Point(63, 149);
            savePathEntry.Name = "savePathEntry";
            savePathEntry.Size = new System.Drawing.Size(693, 23);
            savePathEntry.TabIndex = 5;
            // 
            // pathButton
            // 
            pathButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            pathButton.Location = new System.Drawing.Point(762, 149);
            pathButton.Name = "pathButton";
            pathButton.Size = new System.Drawing.Size(25, 23);
            pathButton.TabIndex = 6;
            pathButton.Text = "...";
            pathButton.UseVisualStyleBackColor = true;
            pathButton.Click += PopulateDownloadPath;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.ForeColor = System.Drawing.SystemColors.Window;
            label1.Location = new System.Drawing.Point(363, 78);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 15);
            label1.TabIndex = 7;
            label1.Text = "GitHub Link";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.ForeColor = System.Drawing.SystemColors.Window;
            label2.Location = new System.Drawing.Point(351, 131);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(96, 15);
            label2.TabIndex = 8;
            label2.Text = "Output Directory";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // mainBranchRadioButton
            // 
            mainBranchRadioButton.AutoSize = true;
            mainBranchRadioButton.BackColor = System.Drawing.Color.Transparent;
            mainBranchRadioButton.ForeColor = System.Drawing.SystemColors.Control;
            mainBranchRadioButton.Location = new System.Drawing.Point(321, 196);
            mainBranchRadioButton.Name = "mainBranchRadioButton";
            mainBranchRadioButton.Size = new System.Drawing.Size(61, 19);
            mainBranchRadioButton.TabIndex = 9;
            mainBranchRadioButton.TabStop = true;
            mainBranchRadioButton.Text = "Master";
            mainBranchRadioButton.UseVisualStyleBackColor = false;
            // 
            // branchLabel
            // 
            branchLabel.AutoSize = true;
            branchLabel.BackColor = System.Drawing.Color.Transparent;
            branchLabel.ForeColor = System.Drawing.SystemColors.Window;
            branchLabel.Location = new System.Drawing.Point(359, 178);
            branchLabel.Name = "branchLabel";
            branchLabel.Size = new System.Drawing.Size(78, 15);
            branchLabel.TabIndex = 10;
            branchLabel.Text = "Select Branch";
            // 
            // otherBanchRadioButton
            // 
            otherBanchRadioButton.AutoSize = true;
            otherBanchRadioButton.BackColor = System.Drawing.Color.Transparent;
            otherBanchRadioButton.ForeColor = System.Drawing.SystemColors.Control;
            otherBanchRadioButton.Location = new System.Drawing.Point(422, 196);
            otherBanchRadioButton.Name = "otherBanchRadioButton";
            otherBanchRadioButton.Size = new System.Drawing.Size(55, 19);
            otherBanchRadioButton.TabIndex = 11;
            otherBanchRadioButton.TabStop = true;
            otherBanchRadioButton.Text = "Other";
            otherBanchRadioButton.UseVisualStyleBackColor = false;
            // 
            // branchEntry
            // 
            branchEntry.Location = new System.Drawing.Point(63, 221);
            branchEntry.Name = "branchEntry";
            branchEntry.Size = new System.Drawing.Size(693, 23);
            branchEntry.TabIndex = 12;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(branchEntry);
            Controls.Add(otherBanchRadioButton);
            Controls.Add(branchLabel);
            Controls.Add(mainBranchRadioButton);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pathButton);
            Controls.Add(savePathEntry);
            Controls.Add(title);
            Controls.Add(outputBox);
            Controls.Add(urlEntry);
            Controls.Add(submitButton);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Repository Downloader";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.TextBox urlEntry;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.TextBox savePathEntry;
        private System.Windows.Forms.Button pathButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.RadioButton mainBranchRadioButton;
        private System.Windows.Forms.Label branchLabel;
        private System.Windows.Forms.RadioButton otherBanchRadioButton;
        private System.Windows.Forms.TextBox branchEntry;
    }
}

