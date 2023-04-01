
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.submitButton = new System.Windows.Forms.Button();
            this.urlEntry = new System.Windows.Forms.TextBox();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.Label();
            this.savePathEntry = new System.Windows.Forms.TextBox();
            this.pathButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(322, 178);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(156, 43);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "Download";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.Download);
            // 
            // urlEntry
            // 
            this.urlEntry.Location = new System.Drawing.Point(63, 96);
            this.urlEntry.Name = "urlEntry";
            this.urlEntry.Size = new System.Drawing.Size(693, 23);
            this.urlEntry.TabIndex = 1;
            // 
            // outputBox
            // 
            this.outputBox.AcceptsReturn = true;
            this.outputBox.Cursor = System.Windows.Forms.Cursors.No;
            this.outputBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.outputBox.Location = new System.Drawing.Point(12, 227);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(776, 211);
            this.outputBox.TabIndex = 3;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.BackColor = System.Drawing.Color.Transparent;
            this.title.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.title.ForeColor = System.Drawing.SystemColors.Window;
            this.title.Location = new System.Drawing.Point(267, 22);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(275, 28);
            this.title.TabIndex = 4;
            this.title.Text = "Repository Downloader";
            // 
            // savePathEntry
            // 
            this.savePathEntry.Location = new System.Drawing.Point(63, 149);
            this.savePathEntry.Name = "savePathEntry";
            this.savePathEntry.Size = new System.Drawing.Size(693, 23);
            this.savePathEntry.TabIndex = 5;
            // 
            // pathButton
            // 
            this.pathButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pathButton.Location = new System.Drawing.Point(762, 149);
            this.pathButton.Name = "pathButton";
            this.pathButton.Size = new System.Drawing.Size(25, 23);
            this.pathButton.TabIndex = 6;
            this.pathButton.Text = "...";
            this.pathButton.UseVisualStyleBackColor = true;
            this.pathButton.Click += new System.EventHandler(this.PopulateDownloadPath);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(363, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "GitHub Link";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(351, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Output Directory";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pathButton);
            this.Controls.Add(this.savePathEntry);
            this.Controls.Add(this.title);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.urlEntry);
            this.Controls.Add(this.submitButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Repository Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

