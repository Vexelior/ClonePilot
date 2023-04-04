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
        public string branchName { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        public static void BypassCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public static HttpClient HttpClientOverride(HttpClient client)
        {
            // Create a handler to bypass the certificate validation
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            client = new HttpClient(clientHandler);

            // Return the client
            return client;
        }

        private async void Download(object sender, EventArgs e)
        {
            // Local storage and paths
            string url = urlEntry.Text;
            string savePath = savePathEntry.Text;

            // Disable the submit button
            DisableButton(submitButton);

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(savePath))
            {
                TimeStampMessage("Please make sure both fields are populated.");
                EnableButton(submitButton);
                return;
            }

            if (url.Contains("github.com"))
            {
                BypassCertificateValidation();

                try
                {
                    if (url.Contains("tree"))
                    {
                        try
                        {
                            string[] treeUrlSplit = url.Split('/');
                            /* If the length of the array is greater than 7, remove everything after the 7th index.
                            * This is to account for the possibility of a URL that contains a branch name with a slash in it.
                            */
                            if (treeUrlSplit.Length > 7)
                            {
                                treeUrlSplit = treeUrlSplit.Take(7).ToArray();
                            }
                            repoName = treeUrlSplit[^3];
                            repoOwner = treeUrlSplit[^4];
                            branchName = treeUrlSplit[^1];
                            repoURL = "https://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/" + branchName + ".zip";

                            // Create a handler to bypass the certificate validation.
                            HttpClientHandler clientHandler = new();
                            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                            // Pass the handler.
                            HttpClient client = new();
                            client = HttpClientOverride(client);

                            // Check the response code to see if the branch exists
                            using (client)
                            {
                                try
                                {
                                    HttpResponseMessage response = await client.GetAsync(repoURL);

                                    // If there is an SSL certificate error, try again with the bypass. \\
                                    if (response.StatusCode == HttpStatusCode.Forbidden)
                                    {
                                        try
                                        {
                                            BypassCertificateValidation();
                                            response = await client.GetAsync(repoURL);
                                        }
                                        catch (Exception ex)
                                        {
                                            TimeStampMessage($"Download failed! {ex.Message}");
                                            EnableButton(submitButton);
                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TimeStampMessage($"Download failed! {ex.Message}");
                                    EnableButton(submitButton);
                                    return;
                                }
                            }

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
                        TimeStampMessage(url.Length.ToString());
                        repoName = urlSplit[^1];
                        repoOwner = urlSplit[^2];
                        branchName = "main";
                        repoURL = $"https://github.com/{repoOwner}/{repoName}/archive/refs/heads/main.zip";

                        // Check the response code to see if the branch exists
                        using (HttpClient client = new())
                        {
                            try
                            {
                                BypassCertificateValidation();
                                HttpResponseMessage response = await client.GetAsync(repoURL);

                                if (!response.IsSuccessStatusCode)
                                {
                                    try
                                    {
                                        TimeStampMessage($"Could not find branch 'main'. Trying master...");
                                        branchName = "master";
                                        string masterZipURL = $"https://github.com/{repoOwner}/{repoName}/archive/refs/heads/{branchName}.zip";

                                        response = await client.GetAsync(masterZipURL);

                                        if (!response.IsSuccessStatusCode)
                                        {
                                            TimeStampMessage($"Could not find branch, '{branchName}', or repository, '{repoName}'. Please try again.");
                                            EnableButton(submitButton);
                                            return;
                                        }

                                        repoURL = masterZipURL;
                                    }
                                    catch (Exception ex)
                                    {
                                        TimeStampMessage($"Download failed! {ex.Message}");
                                        EnableButton(submitButton);
                                        return;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                TimeStampMessage($"URL formatting is incorrect. {ex.Message}");
                                EnableButton(submitButton);
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TimeStampMessage($"URL formatting is incorrect. {ex.Message}");
                    EnableButton(submitButton);
                    return;
                }


                string saveFile = $"{savePath}\\{repoName}.zip";

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
                                TimeStampMessage($"Could not find branch 'main'. Trying master...");
                                string masterZipURL = $"https://github.com/{repoOwner}/{repoName}/archive/refs/heads/master.zip";

                                response = await client.GetAsync(masterZipURL);
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
                        using (FileStream fileStream = new(saveFile, FileMode.Create))
                        {
                            await fileStream.WriteAsync(content, 0, content.Length);
                        }

                        // Extract the zip file
                        ZipFile.ExtractToDirectory(saveFile, savePath);

                        // Delete the zip file
                        File.Delete(saveFile);

                        // Show a success message in the output box
                        TimeStampMessage("Download complete! File was saved to " + savePath);

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

        public string GetSelectedBranch(string branchName)
        {
            return branchName;
        }
    }
}
