using System;
using System.Windows.Forms;
using System.Globalization;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmIDN : Form
    {
        public frmIDN()
        {
            InitializeComponent();
        }

        private void whoisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain form = new frmMain();
            form.WindowState = this.WindowState;
            form.Show();
        }

        private void frmIDN_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dNSToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDNS form = new frmDNS();
            form.WindowState = this.WindowState;
            form.Show();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUrl.Text))
            {
                MessageBox.Show("You need to enter the URL information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                IdnMapping idn = new IdnMapping();
                ListViewItem lvi = new ListViewItem();
                lvi.Text = txtUrl.Text;
                lvi.SubItems.Add(idn.GetAscii(txtUrl.Text));
                listView1.Items.Add(lvi);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].SubItems[1].Text + " - copied to clipboard", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void temizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://niyazialpay.com");
        }

        private void frmIDN_Load(object sender, EventArgs e)
        {
            txtUrl.Focus();
        }
        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmA form = new frmA();
            frmA frm = new frmA();
            if (Application.OpenForms["frmA"] == null)
            {
                frm.Name = "frmA";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Address Record window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cNAMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCname frm = new frmCname();
            if (Application.OpenForms["frmCname"] == null)
            {
                frm.Name = "frmCname";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Canonical Name Record window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void nSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNS frm = new frmNS();
            if (Application.OpenForms["frmNS"] == null)
            {
                frm.Name = "frmNS";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Name Server Records window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMX frm = new frmMX();
            if (Application.OpenForms["frmMX"] == null)
            {
                frm.Name = "frmMX";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Mail Exchanger Records window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pTRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPTR frm = new frmPTR();
            if (Application.OpenForms["frmPTR"] == null)
            {
                frm.Name = "frmPTR";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Reverse DNS Pointer Record window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTXT frm = new frmTXT();
            if (Application.OpenForms["frmTXT"] == null)
            {
                frm.Name = "frmTXT";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("TXT Records window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
