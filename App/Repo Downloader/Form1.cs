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

            // URL data
            string repoName = "";
            string repoOwner = "";
            string repoURL = "";

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
                    string[] urlSplit = url.Split('/');
                    repoName = urlSplit[urlSplit.Length - 1];
                    repoOwner = urlSplit[urlSplit.Length - 2];
                    repoURL = "http://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/main.zip";
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
                                repoURL = "http://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/master.zip";
                                response = await client.GetAsync(repoURL);
                            }
                            catch (Exception ex)
                            {
                                TimeStampMessage($"Could not find branch master or main! {ex.Message}");
                                EnableButton(submitButton);
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
    }
}
