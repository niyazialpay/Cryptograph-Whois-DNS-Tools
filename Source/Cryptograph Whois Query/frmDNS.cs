using System;
using System.Windows.Forms;
using System.Net;
using System.Threading;


namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmDNS : Form
    {
        DNS dns = new DNS();
        public frmDNS()
        {
            InitializeComponent();
        }

        private void frmDNS_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void whoisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain form = new frmMain();
            form.WindowState = this.WindowState;
            form.Show();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://niyazialpay.com");
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(dnsqueryfunction));
            thread.Start();
        }

        public void dnsqueryfunction()
        {
            try
            {
                this.listView1.Items.Clear();
                foreach (string aitem in dns.ARecords(txtUrl.Text))
                {
                    ListViewItem lvi = new ListViewItem();
                    if (String.IsNullOrEmpty(aitem)) lvi.Text = "-";
                    else
                    {
                        lvi.Text = aitem;
                        try
                        {
                            //a ptr records
                            this.listView5.Items.Clear();
                            listView5.Items.Add(dns.PTRRecord(aitem));

                            foreach (string wwwAitem in dns.ARecords("www." + txtUrl.Text))
                            {
                                if (String.IsNullOrEmpty(wwwAitem))
                                {
                                    lvi.SubItems.Add("-");
                                }
                                else lvi.SubItems.Add(wwwAitem);
                            }

                            listView1.Items.Add(lvi);
                        }
                        catch (FormatException ex)
                        {
                            //listView5.Items.Add("FormatException caught!!!");
                            //listView5.Items.Add("Source : " + ex.Source);
                            listView5.Items.Add(ex.Message);
                        }
                        catch (ArgumentNullException ex)
                        {
                            //listView5.Items.Add("ArgumentNullException caught!!!");
                            //listView5.Items.Add("Source : " + ex.Source);
                            listView5.Items.Add(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            //listView5.Items.Add("Exception caught!!!");
                            //listView5.Items.Add("Source : " + ex.Source);
                            listView5.Items.Add(ex.Message);
                        }
                    }
                }

                this.listView2.Items.Clear();
                
                foreach (string cnameitem in dns.CnameRecord(txtUrl.Text))
                {
                    ListViewItem lvicname = new ListViewItem();
                    if (String.IsNullOrEmpty(cnameitem)) lvicname.Text = "-";
                    else lvicname.Text = cnameitem;

                    foreach (string wwwcnameitem in dns.CnameRecord("www." + txtUrl.Text))
                    {
                        if (String.IsNullOrEmpty(wwwcnameitem))
                        {
                            lvicname.SubItems.Add("-");
                        }
                        else lvicname.SubItems.Add(wwwcnameitem);
                    }

                    listView2.Items.Add(lvicname);
                }

                

                //ns records
                this.listView4.Items.Clear();

                foreach (string nsitem in dns.NSRecords(txtUrl.Text))
                {
                    ListViewItem lvins = new ListViewItem();
                    lvins.Text = nsitem;
                    foreach (string nsaitem in dns.ARecords(nsitem))
                    {
                        lvins.SubItems.Add(nsaitem);
                    }
                    listView4.Items.Add(lvins);
                }

                //txt records
                this.listView7.Items.Clear();

                foreach (string txtitem in dns.TxtRecords(txtUrl.Text))
                {
                    listView7.Items.Add(txtitem);
                }

                //mx records
                this.listView3.Items.Clear();

                foreach (string mxitem in dns.MXRecords(txtUrl.Text))
                {
                    ListViewItem lvimx = new ListViewItem();
                    lvimx.Text = mxitem;
                    foreach (string mxaitem in dns.ARecords(mxitem))
                    {
                        lvimx.SubItems.Add(mxaitem);
                        //mx ptr records
                        try
                        {
                            this.listView6.Items.Clear();
                            
                            listView6.Items.Add(dns.PTRRecord(mxaitem));
                        }
                        catch (FormatException ex)
                        {
                            //listView6.Items.Add("FormatException caught!!!");
                            //listView6.Items.Add("Source : " + ex.Source);
                            listView6.Items.Add(ex.Message);
                        }
                        catch (ArgumentNullException ex)
                        {
                            //listView6.Items.Add("ArgumentNullException caught!!!");
                            //listView6.Items.Add("Source : " + ex.Source);
                            listView6.Items.Add(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            //listView6.Items.Add("Exception caught!!!");
                            //listView6.Items.Add("Source : " + ex.Source);
                            listView6.Items.Add(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ıDNÇeviriciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmIDN form = new frmIDN();
            form.WindowState = this.WindowState;
            form.Show();
        }

        private void frmDNS_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            txtUrl.Focus();
        }

        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
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

        private void sOAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSOA frm = new frmSOA();
            if (Application.OpenForms["frmSOA"] == null)
            {
                frm.Name = "frmSOA";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Start of Authority Record Records window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
