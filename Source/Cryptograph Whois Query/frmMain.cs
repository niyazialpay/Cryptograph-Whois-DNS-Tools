using System;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;

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
                progressbar.Visible = true;
                btnQuery.Enabled = false;
                txtUrl.Enabled = false;
                backgroundWorker1.RunWorkerAsync(new Dictionary<string, string>()
                {
                    { "url", txtUrl.Text },
                });
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://niyazialpay.com");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmA frm = new frmA();
            if (Application.OpenForms["frmA"] == null)
            {
                frm.Name = "frmA";
                frm.Show();
            }
            else
            {
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
                MessageBox.Show("Service Record window already open", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listviewDoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(aRecordView.SelectedItems[0].SubItems[1].Text + " - copied to clipboard", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetText(aRecordView.SelectedItems[0].SubItems[1].Text);
        }

        private void whoisTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aRecordView.Items.Clear();
            cnameRecordView.Items.Clear();
            mxRecordView.Items.Clear();
            nsRecordView.Items.Clear();
            aPTRview.Items.Clear();
            mxPTRview.Items.Clear();
            soaView.Items.Clear();
            idnListView.Items.Clear();
            txtUrl.Clear();
            whoisTextBox.Clear();
        }

        private void toolStripSplitButton1_Click(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
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

                    whois whois = new whois();
                    whoisTextBox.Text = whois.query(UserInputs["url"]);

                    backgroundWorker1.ReportProgress(5);

                    IdnMapping idn = new IdnMapping();
                    ListViewItem idnlvi = new ListViewItem();
                    idnlvi.Text = txtUrl.Text;
                    idnlvi.SubItems.Add(idn.GetAscii(UserInputs["url"]));
                    idnListView.Items.Add(idnlvi);

                    backgroundWorker1.ReportProgress(10);
                    //a record
                    this.aRecordView.Items.Clear();
                    foreach (string aitem in dns.ARecords(UserInputs["url"]))
                    {
                        ListViewItem lvi = new ListViewItem();
                        if (String.IsNullOrEmpty(aitem)) lvi.Text = "-";
                        else
                        {
                            lvi.Text = aitem;
                            try
                            {
                                //a ptr records
                                this.aPTRview.Items.Clear();
                                aPTRview.Items.Add(dns.PTRRecord(aitem));

                                foreach (string wwwAitem in dns.ARecords("www." + UserInputs["url"]))
                                {
                                    if (String.IsNullOrEmpty(wwwAitem))
                                    {
                                        lvi.SubItems.Add("-");
                                    }
                                    else lvi.SubItems.Add(wwwAitem);
                                }
                                backgroundWorker1.ReportProgress(20);
                                aRecordView.Items.Add(lvi);
                            }
                            catch (FormatException ex)
                            {
                                //listView5.Items.Add("FormatException caught!!!");
                                //listView5.Items.Add("Source : " + ex.Source);
                                aPTRview.Items.Add(ex.Message);
                            }
                            catch (ArgumentNullException ex)
                            {
                                //listView5.Items.Add("ArgumentNullException caught!!!");
                                //listView5.Items.Add("Source : " + ex.Source);
                                aPTRview.Items.Add(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                //listView5.Items.Add("Exception caught!!!");
                                //listView5.Items.Add("Source : " + ex.Source);
                                aPTRview.Items.Add(ex.Message);
                            }
                        }
                    }
                    //cname record
                    this.cnameRecordView.Items.Clear();

                    foreach (string cnameitem in dns.CnameRecord(UserInputs["url"]))
                    {
                        ListViewItem lvicname = new ListViewItem();
                        if (String.IsNullOrEmpty(cnameitem)) lvicname.Text = "-";
                        else lvicname.Text = cnameitem;

                        foreach (string wwwcnameitem in dns.CnameRecord("www." + UserInputs["url"]))
                        {
                            lvicname.SubItems.Add(wwwcnameitem);
                        }

                        cnameRecordView.Items.Add(lvicname);
                    }
                    backgroundWorker1.ReportProgress(40);


                    //ns records
                    this.nsRecordView.Items.Clear();

                    foreach (string nsitem in dns.NSRecords(UserInputs["url"]))
                    {
                        ListViewItem lvins = new ListViewItem();
                        lvins.Text = nsitem;
                        foreach (string nsaitem in dns.ARecords(nsitem))
                        {
                            lvins.SubItems.Add(nsaitem);
                        }
                        nsRecordView.Items.Add(lvins);
                    }
                    backgroundWorker1.ReportProgress(60);

                    //txt records
                    this.soaView.Items.Clear();

                    foreach (string txtitem in dns.TxtRecords(UserInputs["url"]))
                    {
                        soaView.Items.Add(txtitem);
                    }
                    backgroundWorker1.ReportProgress(70);

                    //soa record
                    soaView.Items.Clear();
                    foreach (string soaitem in dns.SOARecord(UserInputs["url"]))
                    {
                        soaView.Items.Add(soaitem);
                    }
                    backgroundWorker1.ReportProgress(80);

                    //mx records
                    this.mxRecordView.Items.Clear();

                    foreach (string mxitem in dns.MXRecords(UserInputs["url"]))
                    {
                        ListViewItem lvimx = new ListViewItem();
                        lvimx.Text = mxitem;
                        foreach (string mxaitem in dns.ARecords(mxitem))
                        {
                            lvimx.SubItems.Add(mxaitem);
                            //mx ptr records
                            try
                            {
                                this.mxPTRview.Items.Clear();

                                mxPTRview.Items.Add(dns.PTRRecord(mxaitem));
                            }
                            catch (FormatException ex)
                            {
                                //listView6.Items.Add("FormatException caught!!!");
                                //listView6.Items.Add("Source : " + ex.Source);
                                mxPTRview.Items.Add(ex.Message);
                            }
                            catch (ArgumentNullException ex)
                            {
                                //listView6.Items.Add("ArgumentNullException caught!!!");
                                //listView6.Items.Add("Source : " + ex.Source);
                                mxPTRview.Items.Add(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                //listView6.Items.Add("Exception caught!!!");
                                //listView6.Items.Add("Source : " + ex.Source);
                                mxPTRview.Items.Add(ex.Message);
                            }
                        }
                    }
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
                }
            }
        }
    }
}
