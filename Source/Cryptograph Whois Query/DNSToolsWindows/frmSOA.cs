using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmSOA : Form
    {
        public frmSOA()
        {
            InitializeComponent();
        }
        DNS dns = new DNS();
        private void btnQuery_Click(object sender, EventArgs e)
        {
            progressbar.Visible = true;
            btnQuery.Enabled = false;
            txtUrl.Enabled = false;
            contextMenuStrip1.Enabled = false;
            backgroundWorker1.RunWorkerAsync(new Dictionary<string, string>()
                {
                    { "url", txtUrl.Text },
                });
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressbar.Value = e.ProgressPercentage;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, string> UserInputs = e.Argument as Dictionary<string, string>;
            if (UserInputs != null)
            {
                try
                {
                    backgroundWorker1.ReportProgress(50);
                    foreach (string item in dns.SOARecord(UserInputs["url"]))
                    {
                        ListViewItem lvimx = new ListViewItem();
                        lvimx.Text = UserInputs["url"];
                        lvimx.SubItems.Add(item);
                        listView1.Items.Add(lvimx);
                    }
                    txtUrl.Clear();
                    backgroundWorker1.ReportProgress(100);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    progressbar.Visible = false;
                    btnQuery.Enabled = true;
                    txtUrl.Enabled = true;
                    backgroundWorker1.ReportProgress(0);
                    progressbar.Value = 0;
                    txtUrl.Focus();
                    contextMenuStrip1.Enabled = true;
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].SubItems[1].Text + " - copied to clipboard", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void frmSOA_Load(object sender, EventArgs e)
        {
            txtUrl.Focus();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtUrl.Focus();
            txtUrl.Clear();
            listView1.Items.Clear();
        }

        private void frmSOA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
