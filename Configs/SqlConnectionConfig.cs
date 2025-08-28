using Dapper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text;

namespace LBV_Basics.Configs
{
    public class SqlConnectionConfig : ConnectionConfig
    {
        public static readonly List<SqlConnectionConfig> List = [];
        private static readonly string path = GlobalValues.ConfigDirectory + "config dbsql.json";
        private static readonly string defConStr = "Data Source=@\\@,@;Initial Catalog=@;User ID=@;Password=@;Integrated Security=True@;TrustServerCertificate=true";

        public SqlConnectionConfig()
        {
            List.Add(this);
            Name = name;
        }

        private string name = "Preset #1";
        private bool isActive = false;

        public string Name
        {
            get { return name; }
            set
            {
                if (value.Length > 0)
                {
                    name = value;
                    int nr = 1;
                    string delimiter = " #";
                    string defName = name;
                    string[] defNameList = defName.Split(delimiter);
                    if (defNameList.Length > 1 && int.TryParse(defNameList[^1], out _)) { defName = defName[..^(defNameList[^1].Length + delimiter.Length)]; }
                    while (!IsUniqueName())
                    {
                        name = defName + delimiter + nr.ToString();
                        nr++; if (nr == int.MaxValue) { break; }
                    }
                }
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { if (value != isActive) { if (value) { DeactivateAllConnections(); } isActive = value; } }
        }

        [JsonIgnore] public string ConnectionString
        {
            get
            {
                string[] _defConStr = defConStr.Split("@");
                _defConStr[1] += SourceName;
                _defConStr[2] += Port.ToString();
                _defConStr[3] += CatalogName;
                _defConStr[4] += UserId;
                _defConStr[5] += Password;
                if (NetworkType == NetworkType.Localhost)
                {
                    _defConStr[0] += PcName;
                    _defConStr[2] = string.Empty;
                    _defConStr[4] = string.Empty;
                    _defConStr[5] = string.Empty;
                }
                else
                {
                    _defConStr[0] += Ipv6;
                    _defConStr[6] = string.Empty;
                }
                return string.Join(string.Empty, _defConStr);
            }
            set { }
        }

        public bool Reseed(Type modelType)
        {
            if (Connectivity() && GlobalValues.SqlTableNames.TryGetValue(modelType, out string? _tableName))
            {
                string tableName = _tableName ?? string.Empty;
                string SqlQry = "DELETE FROM " + tableName + "; DBCC CHECKIDENT(" + tableName + ", RESEED, 0);";
                try
                {
                    SqlConnection con = new(ConnectionString);
                    con.Open();
                    SqlCommand SqlCmd = new(SqlQry, con);
                    SqlCmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch { return false; }
            }
            else { return false; }
        }

        public bool IsUniqueName()
        {
            int listIndexThis = List.IndexOf(this);
            for (int conNr = 0; conNr < List.Count; conNr++)
            {
                if (List[conNr].Name == name && conNr != listIndexThis) { return false; }
            }
            return true;
        }

        public bool Connectivity()
        {
            foreach (string tableName in GlobalValues.SqlTableNames.Values)
            {
                string SqlQry = "SELECT TOP 0 * FROM " + tableName ?? string.Empty + ";";
                try
                {
                    SqlConnection con = new(ConnectionString);
                    _ = con.Query<dynamic>(SqlQry).ToList();
                }
                catch { return false; }
            }
            return true;
        }

        public static void LoadJson()
        {
            if (!File.Exists(path)) { File.WriteAllText(path, JsonConvert.SerializeObject(List, Formatting.Indented), Encoding.Unicode); }
            try
            {
                List.Clear();
                _ = JsonConvert.DeserializeObject<List<SqlConnectionConfig>>(File.ReadAllText(path, Encoding.Unicode)) ?? [];
                GlobalValues.CurrentLogText = "SQL-Database connection settings restored.";
            }
            catch { GlobalValues.CurrentLogText = "Restore SQL-Database connection settings failed!"; }
            if (List.Count == 0) { _ = new SqlConnectionConfig(); }
        }

        public static void SaveJson()
        {
            string text = JsonConvert.SerializeObject(List, Formatting.Indented);
            File.WriteAllText(path, text, Encoding.Unicode);
            GlobalValues.CurrentLogText = "SQL-Database connection settings saved.";
        }

        public static SqlConnectionConfig? GetActiveConnection()
        {
            foreach (SqlConnectionConfig con in List) { if (con.IsActive) { return con; } }
            return null;
        }

        public static void DeactivateAllConnections()
        {
            SqlConnectionConfig? con = GetActiveConnection();
            if (con is not null) { con.IsActive = false; }
        }
    }
}
