using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Security.AccessControl;

namespace Repo_Downloader
{
    public partial class Form1 : Form
    {
        public string repoName { get; set; }
        public string repoOwner { get; set; }
        public string repoURL { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Download(object sender, EventArgs e)
        {
            string url = urlEntry.Text;
            string savePath = savePathEntry.Text;

            DisableButton(submitButton);

            if (!IsValidInput(url, savePath))
            {
                EnableButton(submitButton);
                return;
            }

            if (!url.Contains("github.com"))
            {
                TimeStampMessage("Please enter a valid GitHub URL.");
                EnableButton(submitButton);
                return;
            }
            else
            {
                try
                {
                    string[] urlParts = url.Split('/');
                    repoOwner = urlParts[3];
                    repoName = urlParts[4].Split('.')[0];

                    url = $"https://github.com/{repoOwner}/{repoName}.git";

                    CloneRepo(url);
                }
                catch (Exception ex)
                {
                    TimeStampMessage($"Failed to clone the repo! {ex.Message}");
                    EnableButton(submitButton);
                    return;
                }
            }

            EnableButton(submitButton);
        }


        private bool IsValidInput(string url, string savePath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(savePath))
            {
                TimeStampMessage("Please make sure both fields are populated.");
                EnableButton(submitButton);
                return false;
            }
            return true;
        }


        private void CloneRepo(string repo)
        {
            Process process = new();
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = $"clone {repo}";
            process.StartInfo.WorkingDirectory = savePathEntry.Text;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                TimeStampMessage("Download was successful!");
            }
            else
            {
                TimeStampMessage("There was an issue downloading the repo.");
                return;
            }
        }


        private void PopulateDownloadPath(object sender, EventArgs e)
        {
            // Ask the user to select a folder to save the file to
            FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.ShowNewFolderButton = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                savePathEntry.Text = folderBrowserDialog.SelectedPath;
            }
        }


        private void TimeStampMessage(string message)
        {
            // Add a timestamp with the date to the message
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            // Make a new line and add the message
            string newMessage = $"[{timeStamp}] {message}";

            if (outputBox.Text != "")
            {
                outputBox.Text = outputBox.Text + Environment.NewLine + newMessage;
            }
            else
            {
                outputBox.Text = newMessage;
            }
        }

        private void EnableButton(Button button)
        {
            button.Text = "Download";
            button.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void DisableButton(Button button)
        {
            button.Text = "Downloading...";
            button.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
        }
    }
}
