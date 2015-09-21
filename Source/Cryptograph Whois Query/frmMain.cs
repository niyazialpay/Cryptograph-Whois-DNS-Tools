using System;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        DNS dns = new DNS();
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

                if (tabPage1.Enabled == true)
                {
                    threadwhois.Priority = ThreadPriority.Highest;
                    threaddns.Priority = ThreadPriority.Lowest;
                    threadidn.Priority = ThreadPriority.Lowest;
                }
                else if(tabPage2.Enabled==true)
                {
                    threaddns.Priority = ThreadPriority.Highest;
                    threadwhois.Priority = ThreadPriority.Lowest;
                    threadidn.Priority = ThreadPriority.Lowest;
                }
                else
                {
                    threaddns.Priority = ThreadPriority.Lowest;
                    threadwhois.Priority = ThreadPriority.Lowest;
                    threadidn.Priority = ThreadPriority.Highest;
                }
                threadwhois.Start();
                threaddns.Start();
                threadidn.Start();
            }
        }

        public void idnfunction()
        {
            IdnMapping idn = new IdnMapping();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = txtUrl.Text;
            lvi.SubItems.Add(idn.GetAscii(txtUrl.Text));
            idnListView.Items.Add(lvi);
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
                this.soalistview.Items.Clear();

                foreach (string txtitem in dns.TxtRecords(txtUrl.Text))
                {
                    soalistview.Items.Add(txtitem);
                }

                soalistview.Items.Clear();
                foreach (string soaitem in dns.SOARecord(txtUrl.Text))
                {
                    soalistview.Items.Add(soaitem);
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
            //panelWebbrowser.Visible = true;
            //panelDNS.Visible = false;
        }

        private void dNSToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabPage2.Enabled = true;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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
                MessageBox.Show("Start of Authority Record window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void sRVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSRV frm = new frmSRV();
            if (Application.OpenForms["frmSRV"] == null)
            {
                frm.Name = "frmSRV";
                frm.Show();
            }
            else
            {
                //frm.WindowState = WindowState;
                MessageBox.Show("Service Record window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listviewDoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].SubItems[1].Text + " - copied to clipboard", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            listView6.Items.Clear();
            soalistview.Items.Clear();
            idnListView.Items.Clear();
            txtUrl.Clear();
            webBrowser1.DocumentText = "";
        }

    }
}
