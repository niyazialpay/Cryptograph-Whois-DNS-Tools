using System;
using System.Windows.Forms;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmTXT : Form
    {
        public frmTXT()
        {
            InitializeComponent();
        }

        DNS dns = new DNS();
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string item in dns.TxtRecords(txtUrl.Text))
                {
                    ListViewItem lvimx = new ListViewItem();
                    lvimx.Text = txtUrl.Text;
                    lvimx.SubItems.Add(item);
                    listView1.Items.Add(lvimx);
                }
                txtUrl.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].SubItems[1].Text + " - copied to clipboard", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void frmTXT_Load(object sender, EventArgs e)
        {
            txtUrl.Focus();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
