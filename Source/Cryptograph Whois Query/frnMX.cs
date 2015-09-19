using System;
using System.Windows.Forms;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmMX : Form
    {
        public frmMX()
        {
            InitializeComponent();
        }

        DNS dns = new DNS();
        private void btnQuery_Click(object sender, EventArgs e)
        {
            foreach (string item in dns.MXRecords(txtUrl.Text))
            {
                ListViewItem lvimx = new ListViewItem();
                lvimx.Text = txtUrl.Text;
                lvimx.SubItems.Add(item);
                listView1.Items.Add(lvimx);
            }
            txtUrl.Clear();
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].SubItems[1].Text + " - copied to clipboard", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void frmMX_Load(object sender, EventArgs e)
        {
            txtUrl.Focus();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
