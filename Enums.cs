namespace LBV_Basics
{
    public enum TimeUnit
    {
        Miliseconds = 0,
        Seconds = 1,
        Minutes = 2,
        Hours = 3,
        Days = 4,
        Weeks = 5,
        Months = 6,
        Years = 7
    }

    public enum DayOfWeekend
    {
        Friday = 1,
        Saturday = 2,
        Sunday = 3
    }

    public enum DtoType
    {
        Full = 0,
        UniqProps = 1,
        Add = 2,
        Update = 3,
        Filter = 4,
        Filters = 5,
    }

    public enum HttpRequestType
    {
        Get = 0,
        Add = 1,
        Delete = 2,
        Update = 3,
    }

    public enum ProtocolType
    {
        None = 0,
        http = 1,
        https = 2,
    }

    public enum NetworkType
    {
        Localhost = 0,
        IpAdress = 1,
    }

    public enum IpAdressType
    {
        IPv4 = 0,
        IPv6 = 1,
    },
	
    public enum DataSourceType
    {
        None = 0,
        txt = 1,
        xlsx = 2,
        xls = 3,
        csv = 4,
        tsv = 5
    }
}
