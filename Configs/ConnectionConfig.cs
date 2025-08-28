namespace LBV_Basics.Configs
{
    public class ConnectionConfig
    {
        private string ipv4 = "127.0.0.1";
        private string ipv6 = "0:0:0:0:0:0:0:1";

        public ProtocolType ProtocolType { get; set; } = ProtocolType.http;

        public NetworkType NetworkType { get; set; } = NetworkType.Localhost;

        public IpAdressType IpAdressType { get; set; } = IpAdressType.IPv6;

        public string Ipv4
        {
            get { return ipv4; }
            set { if (value.Split(".").Length == 4 && long.TryParse(value.Replace(".", ""), out _)) { ipv4 = value; } }
        }

        public string Ipv6
        {
            get { return ipv6; }
            set { if (value.Split(":").Length == 8 && value.Split(":").All(i => i.Length < 5)) { ipv6 = value.ToLower(); } }
        }

        public int Port { get; set; } = 1433;

        public string SourceName { get; set; } = string.Empty;

        public string CatalogName { get; set; } = string.Empty;

        public string PcName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
