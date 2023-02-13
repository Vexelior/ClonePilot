using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Repo_Downloader
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void GetBranchNames(List<string> branchNames)
        {
            foreach (string branchName in branchNames)
            {
                branchSelection.Items.Add(branchName);
            }
        }

        private void branchSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBranch = branchSelection.SelectedItem.ToString();
            Form1 form1 = new Form1();
            form1.GetSelectedBranch(selectedBranch);
            this.Close();
        }
    }
}
