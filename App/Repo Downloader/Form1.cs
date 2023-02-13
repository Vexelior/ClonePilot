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

        public static void BypassCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        private async void Download(object sender, EventArgs e)
        {
            // Local storage and paths
            string url = urlEntry.Text;
            string savePath = savePathEntry.Text;

            // Disable the submit button
            DisableButton(submitButton);

            if (url == "" || savePath == "")
            {
                TimeStampMessage("Please make sure both fields are populated.");
                EnableButton(submitButton);
                return;
            }

            if (url.Contains("github.com"))
            {
                try 
                {
                    if (url.Contains("tree"))
                    {
                        try
                        {
                            TimeStampMessage("Tree was found... Checking for branches...");
                            string[] treeUrlSplit = url.Split('/');
                            repoName = treeUrlSplit[treeUrlSplit.Length - 3];
                            repoOwner = treeUrlSplit[treeUrlSplit.Length - 4];
                            repoURL = "http://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/main.zip";
                        }
                        catch (Exception ex)
                        {
                            TimeStampMessage($"URL formatting is incorrect. {ex.Message}");
                            EnableButton(submitButton);
                            return;
                        }
                    }
                    else
                    {
                        string[] urlSplit = url.Split('/');
                        repoName = urlSplit[urlSplit.Length - 1];
                        repoOwner = urlSplit[urlSplit.Length - 2];
                        repoURL = "http://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/main.zip";
                    }
                }
                catch (Exception ex)
                {
                    TimeStampMessage($"URL formatting is incorrect. {ex.Message}");
                    EnableButton(submitButton);
                    return;
                }

                string saveFile = savePath + "\\" + repoName + ".zip";

                // Check if the file already exists.
                if (File.Exists(saveFile))
                {
                    TimeStampMessage($"The file, {saveFile}, already exists!");
                    EnableButton(submitButton);
                    return;
                }

                // Download the file from the URL
                using (HttpClient client = new HttpClient())
                {
                    // Save the file to the specified path
                    try
                    {
                        BypassCertificateValidation();
                        HttpResponseMessage response = await client.GetAsync(repoURL);

                        // If the response code is not valid, try master instead of main.
                        if (!response.IsSuccessStatusCode)
                        {
                            try
                            {
                                TimeStampMessage($"Could not find branch master or main! Searching for other branches...");
                                string branchesURL = "https://api.github.com/repos/" + repoOwner + "/" + repoName + "/branches";

                                // Get a list of branches
                                BypassCertificateValidation();
                                HttpResponseMessage branchResponse = await client.GetAsync(branchesURL);

                                // If the response code is not valid, show an error message
                                if (!branchResponse.IsSuccessStatusCode)
                                {
                                    TimeStampMessage($"Could not find any other branches!");
                                    EnableButton(submitButton);
                                    return;
                                }
                                else 
                                {
                                    // Get the content of the response
                                    string branchContent = await branchResponse.Content.ReadAsStringAsync();

                                    // Split the content into an array
                                    string[] branchSplit = branchContent.Split('"');

                                    // Create a list of branches
                                    List<string> branchList = new List<string>();

                                    // Add each branch to the list
                                    for (int i = 0; i < branchSplit.Length; i++)
                                    {
                                        if (branchSplit[i] == "name")
                                        {
                                            branchList.Add(branchSplit[i + 2]);
                                        }
                                    }

                                    // Convert the list to an array
                                    string[] branches = branchList.ToArray();

                                    // Ask the user to select a branch using a popup windows with a dropdown list of branches
                                    Form2 form2 = new Form2();
                                    form2.branchSelection.Items.AddRange(branches);
                                    form2.ShowDialog();

                                    // Add the branchList to the form 2
                                    form2.GetBranchNames(branchList);

                                    // Get the selected branch
                                    
                                }

                            }
                            catch (Exception ex)
                            {
                                TimeStampMessage($"Download failed! {ex.Message}");
                                EnableButton(submitButton);
                                return;
                            }
                        }

                        byte[] content = await response.Content.ReadAsByteArrayAsync();

                        // Write the file to the specified path
                        using (FileStream fileStream = new FileStream(saveFile, FileMode.Create))
                        {
                            await fileStream.WriteAsync(content, 0, content.Length);
                        }

                        // Extract the zip file
                        ZipFile.ExtractToDirectory(saveFile, savePath);

                        // Delete the zip file
                        File.Delete(saveFile);

                        // Show a success message in the output box
                        TimeStampMessage("Download complete!");

                        // Re-enable the submit button
                        EnableButton(submitButton);
                    }
                    catch (Exception ex)
                    {
                        TimeStampMessage($"Download failed! {ex.Message}");
                        EnableButton(submitButton);
                    }
                }
            }
            else
            {
                TimeStampMessage("Invalid URL!");
                EnableButton(submitButton);
            }
        }

        private void PopulateDownloadPath(object sender, EventArgs e)
        {
            // Ask the user to select a folder to save the file to
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
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

        public string GetSelectedBranch(string branchName)
        {
            return branchName;  
        }
    }
}
