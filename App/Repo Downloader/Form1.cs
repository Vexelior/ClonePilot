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

            if (url.Contains("github.com"))
            {
                string[] urlSplit = url.Split('/');
                string repoName = urlSplit[urlSplit.Length - 1];
                string repoOwner = urlSplit[urlSplit.Length - 2];
                string repoURL = "http://github.com/" + repoOwner + "/" + repoName + "/archive/refs/heads/main.zip";
                string savePath = savePathEntry.Text;

                if (savePath == "")
                {
                    savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                string saveFile = savePath + "\\" + repoName + ".zip";

                // Download the file from the URL
                using (HttpClient client = new HttpClient())
                {

                    // Save the file to the specified path
                    try
                    {
                        BypassCertificateValidation();
                        HttpResponseMessage response = await client.GetAsync(repoURL);
                        byte[] content = await response.Content.ReadAsByteArrayAsync();

                        File.WriteAllBytes(saveFile, content);

                        // Extract the file
                        ZipFile.ExtractToDirectory(saveFile, savePath);

                        // Delete the zip file
                        File.Delete(saveFile);

                        MessageBox.Show("Download complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Download failed!\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid GitHub URL");
            }
        }

        private void PopulateDownloadPath(object sender, EventArgs e)
        {
            // Ask the user to select a folder to save the file to
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select a folder to save the file to...";
            folderBrowserDialog.ShowNewFolderButton = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                savePathEntry.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
