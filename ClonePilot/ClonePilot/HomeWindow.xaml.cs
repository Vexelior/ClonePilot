using ClonePilot.MVVM.ViewModels;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ClonePilot
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        ///<summary>
        /// Name of the repository.
        ///</summary>
        public string? RepoName { get; set; }

        ///<summary>
        /// Name of the repository owner.
        ///</summary>
        public string? RepoOwner { get; set; }

        ///<summary>
        /// The folder where the repository is cloned to.
        ///</summary>
        public string? Folder { get; set; }

        ///<summary>
        /// Name of the branch to clone from.
        ///</summary>
        public string? BranchName { get; set; }

        ///<summary>
        /// Defines whether the clone was successful or not.
        ///</summary>
        public bool CloneSuccessful { get; set; }

        /// <summary>
        /// Constructor for the HomeWindow
        /// </summary>
        public HomeWindow()
        {
            InitializeComponent();

            // Set the DataContext for the HomeWindow to the HomeViewModel
            DataContext = new HomeViewModel();

            // Set the event handler for the radio buttons
            RadioButton[] radioButtons = [MasterBranch, CustomBranch];
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.Checked += HandleRadiobuttons;
            }
        }

        /// <summary>
        /// Event handler for the radio buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleRadiobuttons(object sender, RoutedEventArgs e)
        {
            if (CustomBranch.IsChecked == true)
            {
                BranchBox.IsEnabled = true;
                BranchBox.Cursor = Cursors.Arrow;
            }
            else
            {
                BranchBox.IsEnabled = false;
                BranchBox.Cursor = Cursors.No;
                BranchBox.Clear();
            }
        }

        /// <summary>
        /// Enable the button
        /// </summary>
        /// <param name="button"></param>
        private static void EnableButton(Button button)
        {
            button.IsEnabled = true;
            Mouse.OverrideCursor = null;
        }

        /// <summary>
        /// Disable the button
        /// </summary>
        /// <param name="button"></param>
        private static void DisableButton(Button button)
        {
            button.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        ///<summary>
        /// Outputs a message to the output box with a timestamp.
        ///</summary>
        private void TimeStampMessage(string message, Brush textColor)
        {
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string newMessage = $"[{timeStamp}] {message}";

            Style style = new();
            style.Setters.Add(new Setter(Block.MarginProperty, new Thickness(0)));

            OutputBox.Document.Blocks.Add(new Paragraph(new Run(newMessage)) { Foreground = textColor, Margin = new Thickness(0) });
            OutputBox.ScrollToEnd();
        }

        ///<summary>
        /// Returns true or false depending on whether the user has Git installed on their machine.
        ///</summary>
        private bool CheckForGit()
        {
            string gitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Git");

            if (!Directory.Exists(gitPath))
            {
                TimeStampMessage("Git is not installed on this machine!", Brushes.Yellow);
                return false;
            }

            return true;
        }

        ///<summary>
        /// Populate the specified path in the text box.
        ///</summary>
        private void PopulateDownloadPath(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog open = new();
            open.ShowDialog();

            if (!string.IsNullOrEmpty(open.FolderName))
            {
                PathBox.Text = open.FolderName;
            }
            else
            {
                TimeStampMessage("No folder selected!", Brushes.Yellow);
            }
        }

        ///<summary>
        /// Ensures that a branch is specified if the user selects the "Other" radio button.
        ///</summary>
        private void CheckSpecifiedBranch()
        {
            if (MasterBranch.IsChecked == true)
            {
                return;
            }
            else if (CustomBranch.IsChecked == true)
            {
                BranchName = BranchBox.Text;
            }
            else
            {
                TimeStampMessage("Please select a branch.", Brushes.Yellow);
                EnableButton(SubmitButton);
                return;
            }
        }

        ///<summary>
        /// Clones the repo to the specified path.
        ///</summary>
        private void CloneRepo(string repo)
        {
            string folderName = $"{RepoOwner} - {RepoName}";
            string folderPath = Path.Combine(PathBox.Text, folderName);
            Process process = new();

            if (!string.IsNullOrEmpty(BranchName))
            {
                folderName += $"({BranchName})";
                folderPath = Path.Combine(PathBox.Text, folderName);
            }

            if (Directory.Exists(folderPath))
            {
                for (int i = 1; i <= 1000; i++)
                {
                    string newFolderPath = Path.Combine(PathBox.Text, $"{folderName}({i})");
                    if (!Directory.Exists(newFolderPath))
                    {
                        folderPath = newFolderPath;
                        break;
                    }
                }
            }

            if (CustomBranch.IsChecked == true && !string.IsNullOrEmpty(BranchName))
            {
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = $"clone --branch {BranchName} {repo}";
                process.StartInfo.WorkingDirectory = PathBox.Text;
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
                process.StartInfo.WorkingDirectory = PathBox.Text;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }

            if (process.ExitCode == 0)
            {
                TimeStampMessage("Clone was successful! Attempting to open folder...", Brushes.Green);

                string oldPath = Path.Combine(PathBox.Text, RepoName ?? string.Empty);
                Directory.Move(oldPath, folderPath);
                Folder = folderPath;
                CloneSuccessful = true;
                LinkBox.Clear();
            }
            else if (process.ExitCode != 0)
            {
                TimeStampMessage($"Clone failed! Exit code: {process.ExitCode}.", Brushes.Red);
                CloneSuccessful = false;
                return;
            }
        }

        ///<summary>
        /// Ensures there is a URL and a download path specified.
        ///</summary>
        private bool IsValidInput(string url, string savePath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(savePath))
            {
                TimeStampMessage("Please make sure both fields are populated.", Brushes.Yellow);
                EnableButton(SubmitButton);
                return false;
            }
            return true;
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
                TimeStampMessage("The download path does not exist!", Brushes.Red);
            }
        }

        /// <summary>
        /// Initiates the cloning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitiateClone(object sender, RoutedEventArgs e)
        {
            string url = LinkBox.Text;
            string savePath = PathBox.Text;

            DisableButton(SubmitButton);

            if (!IsValidInput(url, savePath))
            {
                EnableButton(SubmitButton);
                return;
            }

            if (!url.Contains("github.com"))
            {
                TimeStampMessage("Please enter a valid GitHub URL.", Brushes.Yellow);
                EnableButton(SubmitButton);
                return;
            }

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

                if (!CheckForGit())
                {
                    TimeStampMessage("Git is not installed on this machine!", Brushes.Yellow);
                    EnableButton(SubmitButton);
                    return;
                }

                CheckSpecifiedBranch();
                CloneRepo(url);

                if (Directory.Exists(Folder) && CloneSuccessful)
                {
                    OpenDownloadPath(Folder);
                }
            }
            catch (Exception ex)
            {
                TimeStampMessage($"Error: {ex.Message}", Brushes.Red);
            }
            finally
            {
                BranchName = string.Empty;
                EnableButton(SubmitButton);
                MasterBranch.IsChecked = true;
            }
        }
    }
}
