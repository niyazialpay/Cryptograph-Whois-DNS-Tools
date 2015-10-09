using System;
using System.Drawing;
using System.Windows.Forms;
using CERTENROLLLib;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmCSR : Form
    {
        public frmCSR()
        {
            InitializeComponent();
        }

        

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                contextMenuStrip1.Enabled = false;
                string csr = richTextBox1.Text;

                CX509CertificateRequestPkcs10 request = new CX509CertificateRequestPkcs10();
                request.InitializeDecode(csr, EncodingType.XCN_CRYPT_STRING_BASE64_ANY);
                request.CheckSignature();

                string[] csrArray = Functions.explode(",", ((CX500DistinguishedName)request.Subject).Name);

                string[] E = Functions.explode("=", csrArray[0]);
                string[] CN = Functions.explode("=", csrArray[1]);
                string[] OU = Functions.explode("=", csrArray[2]);
                string[] O = Functions.explode("=", csrArray[3]);
                string[] L = Functions.explode("=", csrArray[4]);
                string[] S = Functions.explode("=", csrArray[5]);
                string[] C = Functions.explode("=", csrArray[6]);


                Font boldfont = new Font("Arial", 10, FontStyle.Bold);
                Font normalfont = new Font("Arial", 10, FontStyle.Regular);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("Common Name: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(CN[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nOrganization: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(O[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nOrganization Unit: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(OU[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nLocality: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(L[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nState: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(S[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nCountry: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(C[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nEmail: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(E[1]);

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nPublic Key Lenth: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(request.PublicKey.Length.ToString());

                richTextBox2.SelectionFont = boldfont;
                richTextBox2.AppendText("\nHash Algorithm Friendly Name: ");
                richTextBox2.SelectionFont = normalfont;
                richTextBox2.AppendText(request.HashAlgorithm.FriendlyName.ToString());
            }
            catch
            {
                richTextBox2.Clear();
            }
            finally
            {
                richTextBox1.Focus();
                contextMenuStrip1.Enabled = true;
            }
        }

        private void frmCSR_KeyDown(object sender, KeyEventArgs e)
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

        private void frmCSR_Load(object sender, EventArgs e)
        {
            richTextBox1.Focus();
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            richTextBox1.Focus();
        }
    }
}
