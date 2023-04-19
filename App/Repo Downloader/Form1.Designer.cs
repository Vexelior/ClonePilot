
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
            submitButton = new System.Windows.Forms.Button();
            urlEntry = new System.Windows.Forms.TextBox();
            outputBox = new System.Windows.Forms.TextBox();
            title = new System.Windows.Forms.Label();
            savePathEntry = new System.Windows.Forms.TextBox();
            pathButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // submitButton
            // 
            submitButton.Location = new System.Drawing.Point(322, 178);
            submitButton.Name = "submitButton";
            submitButton.Size = new System.Drawing.Size(156, 43);
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
            outputBox.Location = new System.Drawing.Point(12, 227);
            outputBox.Multiline = true;
            outputBox.Name = "outputBox";
            outputBox.ReadOnly = true;
            outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            outputBox.Size = new System.Drawing.Size(776, 211);
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
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

