using System;
using System.Xml;
using System.Windows.Forms;

namespace Cryptograph_Whois_DNS_Tools
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        Settings settings = new Settings();

        private void btnSave_Click(object sender, EventArgs e)
        {
            string[] information = { txtDNSserver.Text, cacheCheckbox.Checked.ToString().ToLower() };
            if (settings.SettingsWrite(information))
            {
                MessageBox.Show("Changes was saved. You should close and reopen the application of the changes to be active.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("XML Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*XmlTextWriter xmlWrite = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml", System.Text.UTF8Encoding.UTF8);
            xmlWrite.Formatting = Formatting.Indented;
            try
            {
                xmlWrite.WriteStartDocument();
                xmlWrite.WriteStartElement("root");
                xmlWrite.WriteStartElement("settings");
                xmlWrite.WriteElementString("server", txtDNSserver.Text);
                xmlWrite.WriteElementString("cache", cacheCheckbox.Checked.ToString().ToLower());
                xmlWrite.WriteEndElement();
                xmlWrite.WriteEndElement();
                xmlWrite.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtDNSserver.Text = settings.SettingsRead("server");
            cacheCheckbox.Checked = Convert.ToBoolean(settings.SettingsRead("cache"));
        }

        private void frmSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
