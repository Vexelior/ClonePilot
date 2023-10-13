using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Repo_Downloader
{
    public partial class Form1 : Form
    {
        ///<summary>
            /// Name of the repository.
        ///</summary>
        public string RepoName { get; set; }

        ///<summary>
            /// Name of the repository owner.
        ///</summary>
        public string RepoOwner { get; set; }

        ///<summary>
            /// The folder where the repository is cloned to.
        ///</summary>
        public string Folder { get; set; }

        ///<summary>
            /// Name of the branch to clone from.
        ///</summary>
        public string BranchName { get; set; }

        ///<summary>
            /// Defines whether the clone was successful or not.
        ///</summary>
        public bool CloneSuccessful { get; set; }

        public Form1()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(savePathEntry.Text))
            {
                InitialDownloadPath();
            }

            AcceptButton = submitButton;
            mainBranchRadioButton.Checked = true;
            branchEntry.Enabled = false;

            RadioButton[] radios = new[] { mainBranchRadioButton, otherBanchRadioButton };
            foreach (RadioButton radio in radios)
            {
                radio.CheckedChanged += HandleRadioButtons;
            }
        }

        ///<summary>
            /// Event handler for the branch radio buttons.
        ///</summary>
        private void HandleRadioButtons(object sender, EventArgs e)
        {
            if (otherBanchRadioButton.Checked)
            {
                branchEntry.Enabled = true;
            }
            else
            {
                branchEntry.Enabled = false;
                branchEntry.Cursor = Cursors.No;
                branchEntry.Clear();
            }
        }


        ///<summary>
            /// Starts the download process.
        ///</summary>
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
                        CheckSpecifiedBranch();

                        CloneRepo(url);

                        if (Directory.Exists(Folder) && CloneSuccessful == true)
                        {
                            OpenDownloadPath(Folder);
                        }
                    }
                    else
                    {
                        TimeStampMessage("Git is not installed on this machine!");
                        EnableButton(submitButton);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    TimeStampMessage($"Error: {ex.Message}");
                    EnableButton(submitButton);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(BranchName))
            {
                BranchName = string.Empty;
            }

            EnableButton(submitButton);
        }


        ///<summary>
            /// Ensures there is a URL and a download path specified.
        ///</summary>
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


        ///<summary>
            /// Returns true or false depending on whether the user has Git installed on their machine.
        ///</summary>
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


        ///<summary>
            /// Ensures that a branch is specified if the user selects the "Other" radio button.
        ///</summary>
        private void CheckSpecifiedBranch()
        {
            if (mainBranchRadioButton.Checked)
            {
                return;
            }
            else if (otherBanchRadioButton.Checked)
            {
                BranchName = branchEntry.Text;
            }
            else
            {
                TimeStampMessage("Please select a branch.");
                EnableButton(submitButton);
                return;
            }
        }

        ///<summary>
            /// Clones the repo to the specified path.
        ///</summary>
        private void CloneRepo(string repo)
        {
            string folderName = $"{RepoOwner} - {RepoName}";
            string folderPath = Path.Combine(savePathEntry.Text, folderName);
            Process process = new();

            if (!string.IsNullOrEmpty(BranchName))
            {
                folderName += $"({BranchName})";
                folderPath = Path.Combine(savePathEntry.Text, folderName);
            }

            if (Directory.Exists(folderPath))
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string newFolderPath = Path.Combine(savePathEntry.Text, $"{folderName}({i})");
                    if (!Directory.Exists(newFolderPath))
                    {
                        folderPath = newFolderPath;
                        break;
                    }
                }
            }

            if (otherBanchRadioButton.Checked && !string.IsNullOrEmpty(BranchName))
            {
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = $"clone --branch {BranchName} {repo}";
                process.StartInfo.WorkingDirectory = savePathEntry.Text;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }
            else
            {
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = $"clone {repo}";
                process.StartInfo.WorkingDirectory = savePathEntry.Text;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }

            if (process.ExitCode == 0)
            {
                TimeStampMessage("Download was successful! Attempting to open folder...");

                string oldPath = Path.Combine(savePathEntry.Text, RepoName);
                Directory.Move(oldPath, folderPath);
                Folder = folderPath;
                CloneSuccessful = true;
                urlEntry.Clear();
            }
            else if (process.ExitCode != 0)
            {
                TimeStampMessage($"Download failed! Exit code: {process.ExitCode}.");
                CloneSuccessful = false;
                return;
            }
        }


        ///<summary>
            /// Populate the specified path in the text box.
        ///</summary>
        private void PopulateDownloadPath(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new()
            {
                ShowNewFolderButton = true
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                savePathEntry.Text = folderBrowserDialog.SelectedPath;
            }
        }


        ///<summary>
            /// Populate the download path with the user's Downloads folder or Desktop if the Downloads folder does not exist.
        ///</summary>
        private void InitialDownloadPath()
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (Directory.Exists(downloadsPath))
            {
                savePathEntry.Text = downloadsPath;
            }
            else
            {
                try
                {
                    savePathEntry.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                catch (Exception ex)
                {
                    TimeStampMessage($"Error populating output path. {ex.Message}");
                }
            }
        }


        ///<summary>
            /// Opens the download path in File Explorer.
        ///</summary>
        private void OpenDownloadPath(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new(path);
                directoryInfo.Attributes &= ~FileAttributes.ReadOnly;

                Process.Start("explorer.exe", path);
            }
            else
            {
                TimeStampMessage("The download path does not exist!");
            }
        }


        ///<summary>
            /// Outputs a message to the output box with a timestamp.
        ///</summary>
        private void TimeStampMessage(string message)
        {
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string newMessage = $"[{timeStamp}] {message}";

            if (outputBox.Text != "")
            {
                outputBox.Text = outputBox.Text + Environment.NewLine + newMessage;
            }
            else
            {
                outputBox.Text = newMessage;
            }

            outputBox.SelectionStart = outputBox.Text.Length;
            outputBox.ScrollToCaret();
        }


        ///<summary>
            /// Enable the download button.
        ///</summary>
        private static void EnableButton(Button button)
        {
            button.Text = "Download";
            button.Enabled = true;
            Cursor.Current = Cursors.Default;
        }


        ///<summary>
            /// Disable the download button.
        ///</summary>
        private static void DisableButton(Button button)
        {
            button.Text = "Downloading...";
            button.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
        }
    }
}
