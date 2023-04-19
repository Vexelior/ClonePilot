﻿using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Repo_Downloader
{
    public partial class Form1 : Form
    {
        public string RepoName { get; set; }
        public string RepoOwner { get; set; }

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

                    if (urlParts.Length > 4)
                    {
                        urlParts = urlParts.Take(5).ToArray();
                    }

                    RepoOwner = urlParts[3];
                    RepoName = urlParts[4].Split('.')[0];

                    url = $"https://github.com/{RepoOwner}/{RepoName}.git";


                    if (CheckForGit())
                    {
                        CloneRepo(url);

                        if (CheckForGitFolder(savePath))
                        {
                            RemoveGitFolder(savePath);
                        }
                    }
                    else
                    {
                        TimeStampMessage("Git is not installed on this machine!");
                        EnableButton(submitButton);
                        return;
                    }

                    Process.Start(savePath);
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


        private bool CheckForGit()
        {
            string gitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Git");

            if (!Directory.Exists(gitPath))
            {
                TimeStampMessage("Git is not installed on this machine!");
                return false;
            }

            return true;
        }

        
        private bool CheckForGitFolder(string path)
        {
            string gitPath = Path.Combine(path, ".git");

            if (Directory.Exists(gitPath))
            {
                return true;
            }

            return false;
        }

        private void RemoveGitFolder(string path)
        {
            string gitPath = Path.Combine(path, ".git");

            try
            {
                foreach (var file in Directory.GetFiles(gitPath))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }

                Directory.Delete(gitPath, true);
            }
            catch (Exception ex)
            {
                TimeStampMessage($"Failed to remove the .git folder! {ex.Message}");
                return;
            }
        }


        private void CloneRepo(string repo)
        {
            if (Directory.Exists(Path.Combine(savePathEntry.Text, RepoName)))
            {
                TimeStampMessage("The folder already exists!");
                return;
            }

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
                Directory.Move(Path.Combine(savePathEntry.Text, RepoName), Path.Combine(savePathEntry.Text, $"{RepoOwner} - {RepoName}"));
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
            FolderBrowserDialog folderBrowserDialog = new()
            {
                ShowNewFolderButton = true
            };

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
