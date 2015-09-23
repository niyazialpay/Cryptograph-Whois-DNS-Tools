using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Cryptograph_Whois_DNS_Tools
{
    public class Settings
    {
        private string server, cache;
        public string SettingsRead(string name)
        {
            XmlTextReader xmlRead = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            try
            {
                while (xmlRead.Read())
                {
                    if (xmlRead.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlRead.Name)
                        {
                            case "server":
                                server = xmlRead.ReadString();
                                break;
                            case "cache":
                                cache = xmlRead.ReadString();
                                break;
                        }
                    }
                }
                xmlRead.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("XML Error: " + ex.Message);
            }
            if (name == "server") return server;
            else return cache;
        }

        public bool SettingsWrite(string[] information)
        {
            XmlTextWriter xmlWrite = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml", System.Text.UTF8Encoding.UTF8);
            xmlWrite.Formatting = Formatting.Indented;
            try
            {
                xmlWrite.WriteStartDocument();
                xmlWrite.WriteStartElement("root");
                xmlWrite.WriteStartElement("settings");
                xmlWrite.WriteElementString("server", information[0]);
                xmlWrite.WriteElementString("cache", information[1]);
                xmlWrite.WriteEndElement();
                xmlWrite.WriteEndElement();
                xmlWrite.Close();

                DNS dns = new DNS();
                dns._resolver.DnsServer = information[0];
                dns._resolver.UseCache = Convert.ToBoolean(information[1]);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("XML Error: " + ex.Message);
                return false;
            }
        }
    }
}
