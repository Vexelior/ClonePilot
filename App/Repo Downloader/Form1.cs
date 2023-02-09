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
            string url = urlEntry.Text;
            string savePath = savePathEntry.Text;


            if (url == "" || savePath == "")
            {
                TimeStampMessage("Please make sure both fields are populated.");
                return;
            }

            if (url.Contains("github.com"))
            {
                string[] urlSplit = url.Split('/');
                string repoName = urlSplit[urlSplit.Length - 1];
                string repoOwner = urlSplit[urlSplit.Length - 2];
                string repoURL = "http://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/main.zip";

                if (savePath == "")
                {
                    savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                string saveFile = savePath + "\\" + repoName + ".zip";

                // Check if the file already exists.
                if (File.Exists(saveFile))
                {
                    TimeStampMessage($"The file, {saveFile} already exists!");
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
                                TimeStampMessage("Could not find branch master or main!\n\n" + ex.Message);
                            }
                        }

                        // Get the total bytes of the file
                        long totalBytes = response.Content.Headers.ContentLength.Value;

                        // Update the progress bar with the total bytes
                        progressBar.Maximum = (int)totalBytes;

                        // Create a stream to write the file to
                        using (Stream stream = await response.Content.ReadAsStreamAsync())
                        {
                            // Create a stream to read the file from
                            using (FileStream fileStream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                // Create a buffer to store the file in
                                byte[] buffer = new byte[8192];
                                int bytesRead;

                                // Read the file in chunks
                                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    // Write the chunk to the file
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                                    // Update the progress bar
                                    progressBar.Value += bytesRead;

                                    // Get the percentage of the file that has been downloaded and display it
                                    double percentage = (double)progressBar.Value / (double)progressBar.Maximum * 100;
                                    progressLabel.Text = Math.Round(percentage, 2) + "%";
                                }
                            }
                        }

                        // Extract the zip file
                        ZipFile.ExtractToDirectory(saveFile, savePath);

                        // Delete the zip file
                        File.Delete(saveFile);

                        // Show a success message in the output box
                        TimeStampMessage("Download complete!");
                    }
                    catch (Exception ex)
                    {
                        TimeStampMessage("Download failed!\n\n" + ex.Message);
                    }
                }
            }
            else
            {
                TimeStampMessage("Invalid URL!");
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
    }
}
