using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmSSLCheck : Form
    {
        public frmSSLCheck()
        {
            InitializeComponent();
        }

        private void frmSSLCheck_Load(object sender, EventArgs e)
        {
            richTextBox1.Focus();
        }

        private void richTextBox2_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void frmSSLCheck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            btnQuery.Enabled = false;
            richTextBox1.Enabled = false;
            richTextBox2.Clear();
            contextMenuStrip1.Enabled = false;
            backgroundWorker1.RunWorkerAsync(new Dictionary<string, string>()
                {
                    { "url", richTextBox1.Text },
                });
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, string> UserInputs = e.Argument as Dictionary<string, string>;
            if (UserInputs != null)
            {
                try
                {
                    //Do webrequest to get info on secure site
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + UserInputs["url"]);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    backgroundWorker1.ReportProgress(20);
                    response.Close();
                    backgroundWorker1.ReportProgress(25);

                    //retrieve the ssl cert and assign it to an X509Certificate object
                    X509Certificate cert = request.ServicePoint.Certificate;

                    //convert the X509Certificate to an X509Certificate2 object by passing it into the constructor
                    X509Certificate2 cert2 = new X509Certificate2(cert);

                    Font boldfont = new Font("Arial", 10, FontStyle.Bold);
                    Font boldfont2 = new Font("Arial", 11, FontStyle.Bold);
                    Font normalfont = new Font("Arial", 10, FontStyle.Regular);

                    richTextBox2.Clear();
                    backgroundWorker1.ReportProgress(20);
                    string[] certArray = Functions.explode(",", cert2.SubjectName.Name);
                    backgroundWorker1.ReportProgress(30);

                    richTextBox2.SelectionFont = boldfont2;
                    richTextBox2.AppendText("Publisher Information : \r\n\r\n");

                    int certCount = certArray.Count();

                    for (int i = 0; i < certCount; i++)
                    {
                        string[] certeElements = Functions.explode("=", certArray[i]);
                        richTextBox2.SelectionFont = boldfont;
                        richTextBox2.AppendText(certeElements[0].Trim() + ": ");
                        richTextBox2.SelectionFont = normalfont;
                        richTextBox2.AppendText(certeElements[1].Trim() + "\n");
                    }
                    backgroundWorker1.ReportProgress(55);

                    richTextBox2.SelectionFont = boldfont;
                    richTextBox2.AppendText("\r\nValid From: ");
                    richTextBox2.SelectionFont = normalfont;
                    richTextBox2.AppendText(cert2.GetEffectiveDateString());

                    richTextBox2.SelectionFont = boldfont;
                    richTextBox2.AppendText("\r\nValid To: ");
                    richTextBox2.SelectionFont = normalfont;
                    richTextBox2.AppendText(cert2.GetExpirationDateString());

                    backgroundWorker1.ReportProgress(75);

                    string[] iusserArray = Functions.explode(",", cert2.Issuer);
                    int iusserCount = iusserArray.Count();

                    richTextBox2.SelectionFont = boldfont2;
                    richTextBox2.AppendText("\r\n\r\nIssued By: \r\n\r\n");

                    for (int i = 0; i < iusserCount; i++)
                    {
                        richTextBox2.SelectionFont = boldfont;

                        string[] iusserElements = Functions.explode("=", iusserArray[i]);

                        richTextBox2.AppendText(iusserElements[0].Trim() + ": ");
                        richTextBox2.SelectionFont = normalfont;
                        richTextBox2.AppendText(iusserElements[1].Trim() + "\n");
                        backgroundWorker1.ReportProgress(100);
                    }
                    
                }
                catch (Exception ex)
                {
                    richTextBox2.Text = ex.Message;
                }
                finally
                {
                    richTextBox1.Enabled = true;
                    btnQuery.Enabled = true;
                    progressBar1.Visible = false;
                    richTextBox1.Focus();
                    contextMenuStrip1.Enabled = true;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            richTextBox1.Focus();
        }
    }
}
