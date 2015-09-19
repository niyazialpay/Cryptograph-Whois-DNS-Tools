using System;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Net;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        DNS dns = new DNS();
        frmIDN idnform = new frmIDN();
        frmDNS dnsform = new frmDNS();
        private void frmMain_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            txtUrl.Focus();
        }

        private void whoisClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUrl.Text))
            {
                MessageBox.Show("You need to enter the URL information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Thread threadwhois = new Thread(new ThreadStart(whoisfunction));
                Thread threaddns = new Thread(new ThreadStart(dnsqueryfunction));
                Thread threadidn = new Thread(new ThreadStart(idnfunction));

                threadwhois.Priority = ThreadPriority.Highest;
                threaddns.Priority = ThreadPriority.Lowest;
                threadidn.Priority = ThreadPriority.Lowest;

                threadwhois.Start();
                threaddns.Start();
                threadidn.Start();
            }
        }

        public void idnfunction()
        {
            idnform.txtUrl.Text = txtUrl.Text;
            IdnMapping idn = new IdnMapping();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = txtUrl.Text;
            lvi.SubItems.Add(idn.GetAscii(txtUrl.Text));
            idnform.listView1.Items.Add(lvi);
        }

        public void dnsqueryfunction()
        {
            try
            {
                dnsform.txtUrl.Text = txtUrl.Text;
                dnsform.listView1.Items.Clear();
                
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
                            dnsform.listView5.Items.Clear();
                            dnsform.listView5.Items.Add(dns.PTRRecord(aitem));

                            foreach (string wwwAitem in dns.ARecords("www." + txtUrl.Text))
                            {
                                if (String.IsNullOrEmpty(wwwAitem))
                                {
                                    lvi.SubItems.Add("-");
                                }
                                else lvi.SubItems.Add(wwwAitem);
                            }

                            dnsform.listView1.Items.Add(lvi);
                        }
                        catch (FormatException ex)
                        {
                            //listView5.Items.Add("FormatException caught!!!");
                            //listView5.Items.Add("Source : " + ex.Source);
                            dnsform.listView5.Items.Add(ex.Message);
                        }
                        catch (ArgumentNullException ex)
                        {
                            //listView5.Items.Add("ArgumentNullException caught!!!");
                            //listView5.Items.Add("Source : " + ex.Source);
                            dnsform.listView5.Items.Add(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            //listView5.Items.Add("Exception caught!!!");
                            //listView5.Items.Add("Source : " + ex.Source);
                            dnsform.listView5.Items.Add(ex.Message);
                        }
                    }
                }

                dnsform.listView2.Items.Clear();
                
                foreach (string cnameitem in dns.CnameRecord(txtUrl.Text))
                {
                    ListViewItem lvicname = new ListViewItem();
                    if (String.IsNullOrEmpty(cnameitem)) lvicname.Text = "-";
                    else lvicname.Text = cnameitem;

                    foreach (string wwwcnameitem in dns.CnameRecord("www." + txtUrl.Text))
                    {
                        lvicname.SubItems.Add(wwwcnameitem);
                    }
                    dnsform.listView2.Items.Add(lvicname);
                }


                //ns records
                dnsform.listView4.Items.Clear();

                foreach (string nsitem in dns.NSRecords(txtUrl.Text))
                {
                    ListViewItem lvins = new ListViewItem();
                    lvins.Text = nsitem;
                    foreach (string nsaitem in dns.ARecords(nsitem))
                    {
                        lvins.SubItems.Add(nsaitem);
                    }
                    dnsform.listView4.Items.Add(lvins);
                }

                //txt records
                dnsform.listView7.Items.Clear();

                foreach (string txtitem in dns.TxtRecords(txtUrl.Text))
                {
                    dnsform.listView7.Items.Add(txtitem);
                }

                //mx records
                dnsform.listView3.Items.Clear();

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
                            dnsform.listView6.Items.Clear();
                            dnsform.listView6.Items.Add(dns.PTRRecord(mxaitem));
                        }
                        catch (FormatException ex)
                        {
                            //listView6.Items.Add("FormatException caught!!!");
                            //listView6.Items.Add("Source : " + ex.Source);
                            dnsform.listView6.Items.Add(ex.Message);
                        }
                        catch (ArgumentNullException ex)
                        {
                            //listView6.Items.Add("ArgumentNullException caught!!!");
                            //listView6.Items.Add("Source : " + ex.Source);
                            dnsform.listView6.Items.Add(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            //listView6.Items.Add("Exception caught!!!");
                            //listView6.Items.Add("Source : " + ex.Source);
                            dnsform.listView6.Items.Add(ex.Message);
                        }
                    }
                    dnsform.listView3.Items.Add(lvimx);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void whoisfunction()
        {
            whois whois = new whois();
            webBrowser1.DocumentText = whois.query(txtUrl.Text);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://niyazialpay.com");
        }

        private void whoisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //webBrowser1.DocumentText = "";
        }

        private void dNSToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            dnsform.WindowState = this.WindowState;
            dnsform.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ıDNÇeviriciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            idnform.WindowState = this.WindowState;
            idnform.Show();
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
