using System;
using System.Collections.Generic;
using Heijden.DNS;

namespace Cryptograph_Whois_DNS_Tools
{
    public class DNS
    {
        public Resolver _resolver;

        Settings settings = new Settings();

        public DNS()
        {
            _resolver = new Resolver();
            _resolver.Recursion = true;
            _resolver.UseCache = Convert.ToBoolean(settings.SettingsRead("cache")); ;
            _resolver.DnsServer = settings.SettingsRead("server");

            _resolver.TimeOut = 1000;
            _resolver.Retries = 3;
            _resolver.TransportType = TransportType.Udp;
        }

        public IList<string> TxtRecords(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.TXT;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordTXT record in response.RecordsTXT)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> srvRecords(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.SRV;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordSRV record in response.RecordsSRV)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> ARecords(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.A;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordA record in response.RecordsA)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> CnameRecord(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.CNAME;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordCNAME record in response.RecordsCNAME)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> NSRecords(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.NS;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordNS record in response.RecordsNS)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> MXRecords(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.MX;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordMX record in response.RecordsMX)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public string PTRRecord(string name)
        {
            try
            {
                System.Net.IPAddress hostIPAddress = System.Net.IPAddress.Parse(name);
                System.Net.IPHostEntry hostInfo = System.Net.Dns.GetHostEntry(hostIPAddress);
                return hostInfo.HostName;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public IList<string> GetQTypes()
        {
            IList<string> items = new List<string>();
            Array types = Enum.GetValues(typeof(QType));

            for (int index = 0; index < types.Length; index++)
            {
                items.Add(types.GetValue(index).ToString());
            }

            return items;
        }

        public IList<string> SOARecord(string name)
        {
            IList<string> records = new List<string>();
            const QType qType = QType.SOA;
            const QClass qClass = QClass.IN;

            Response response = _resolver.Query(name, qType, qClass);

            foreach (RecordSOA record in response.RecordsSOA)
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> GetQClasses()
        {
            IList<string> items = new List<string>();
            Array types = Enum.GetValues(typeof(QClass));

            for (int index = 0; index < types.Length; index++)
            {
                items.Add(types.GetValue(index).ToString());
            }

            return items;
        }
    }
}
