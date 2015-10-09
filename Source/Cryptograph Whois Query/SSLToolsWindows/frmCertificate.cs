using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmCertificate : Form
    {
        public frmCertificate()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            contextMenuStrip1.Enabled = false;
            StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\inc\\certificate.cer");
            X509Certificate2 theCertificate;

            try
            {
                // Write the string to a file.

                file.WriteLine(richTextBox1.Text);
                file.Close();
            }
            catch (Exception ex)
            {
                richTextBox2.Text = ex.Message;
            }

            try
            {
                X509Certificate theSigner = X509Certificate.CreateFromCertFile(AppDomain.CurrentDomain.BaseDirectory + "\\inc\\certificate.cer");
                //X509Certificate theSigner = X509Certificate.CreateFromSignedFile("https://niyazialpay.com");
                theCertificate = new X509Certificate2(theSigner);
            }
            catch
            {
                richTextBox2.Text = "No digital signature found";
                return;
            }

            bool chainIsValid = false;

            /*
            * 
            * This section will check that the certificate is from a trusted authority IE 
            * not self-signed.
            * 
            */

            var theCertificateChain = new X509Chain();


            theCertificateChain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;

            /*
            * 
            * Using .Online here means that the validation WILL CALL OUT TO THE INTERNET
            * to check the revocation status of the certificate. Change to .Offline if you
            * don't want that to happen.
            */

            theCertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

            theCertificateChain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);

            theCertificateChain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

            chainIsValid = theCertificateChain.Build(theCertificate);

            try
            {
                if (chainIsValid)
                {
                    Font boldfont = new Font("Arial", 10, FontStyle.Bold);
                    Font boldfont2 = new Font("Arial", 11, FontStyle.Bold);
                    Font normalfont = new Font("Arial", 10, FontStyle.Regular);

                    richTextBox2.Clear();

                    string[] certArray = Functions.explode(",", theCertificate.SubjectName.Name);

                    richTextBox2.SelectionFont = boldfont2;
                    richTextBox2.AppendText("Publisher Information : \n\n");

                    int certCount = certArray.Count();

                    for (int i = 0; i < certCount; i++)
                    {
                        string[] certeElements = Functions.explode("=", certArray[i]);
                        richTextBox2.SelectionFont = boldfont;
                        richTextBox2.AppendText(certeElements[0].Trim() + ": ");
                        richTextBox2.SelectionFont = normalfont;
                        richTextBox2.AppendText(certeElements[1].Trim() + "\n");
                    }

                    richTextBox2.SelectionFont = boldfont;
                    richTextBox2.AppendText("\nValid From: ");
                    richTextBox2.SelectionFont = normalfont;
                    richTextBox2.AppendText(theCertificate.GetEffectiveDateString());

                    richTextBox2.SelectionFont = boldfont;
                    richTextBox2.AppendText("\nValid To: ");
                    richTextBox2.SelectionFont = normalfont;
                    richTextBox2.AppendText(theCertificate.GetExpirationDateString());

                    string[] iusserArray = Functions.explode(",", theCertificate.Issuer);

                    int iusserCount = iusserArray.Count();

                    richTextBox2.SelectionFont = boldfont2;
                    richTextBox2.AppendText("\n\nIssued By: \n\n");

                    for (int i=0; i<iusserCount; i++)
                    {
                        richTextBox2.SelectionFont = boldfont;

                        string[] iusserElements = Functions.explode("=", iusserArray[i]);

                        richTextBox2.AppendText(iusserElements[0].Trim() + ": ");
                        richTextBox2.SelectionFont = normalfont;
                        richTextBox2.AppendText(iusserElements[1].Trim() + "\n");
                    }
                }
                else
                {
                    richTextBox2.Clear();
                    richTextBox2.Text = "Chain Not Valid (certificate is self-signed)";
                }
            }
            catch(Exception ex)
            {
                richTextBox2.Text = ex.Message;
            }
            finally
            {
                richTextBox1.Focus();
                contextMenuStrip1.Enabled = true;
            }
        }

        private void frmCertificate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void richTextBox2_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox1.Focus();
        }

        private void frmCertificate_Load(object sender, EventArgs e)
        {
            richTextBox1.Focus();
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            richTextBox1.Focus();
        }

        private void frmCertificate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\inc\\certificate.cer"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\inc\\certificate.cer");
        }
    }
}
