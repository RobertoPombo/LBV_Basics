using LBV_Basics.Models;
using LBV_Basics.Models.DTOs;

namespace LBV_Basics
{
    public delegate void Notify();

    public static class GlobalValues
    {
        public static readonly string Id = "Id";
        public static readonly int NoId = 0;
        public static readonly int Id0 = 1;
        public static readonly DateTime DateTimeMinValue = new(1000, DateTime.MinValue.Month, DateTime.MinValue.Day, 0, 0, 0, 0, DateTime.MinValue.Kind);
        public static readonly DateTime DateTimeMaxValue = new(DateTime.MaxValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, 0, 0, 0, 0, DateTime.MaxValue.Kind);
        public static readonly List<Type> numericalTypes = [
            typeof(byte), typeof(byte?), typeof(short), typeof(short?), typeof(ushort), typeof(ushort?), typeof(int), typeof(int?), typeof(uint), typeof(uint?),
            typeof(long), typeof(long?), typeof(ulong), typeof(ulong?), typeof(float), typeof(float?), typeof(double), typeof(double?), typeof(decimal), typeof(decimal?),
            typeof(DateTime), typeof(DateTime?), typeof(DateOnly), typeof(DateOnly?), typeof(TimeSpan), typeof(TimeSpan?)
            ];

        private static string baseDirectory = "C:\\Users\\Public\\Documents\\LBV Grundsatz Ticket Messenger app data\\";
        private static string currentLogText = string.Empty;

        public static string BaseDirectory { get { return baseDirectory; } set { baseDirectory = value; } }
        public static string ConfigDirectory { get { return baseDirectory + "config\\"; } }
        public static string DataDirectory { get { return baseDirectory + "data\\"; } }
        public static string DatabaseDirectory { get { return DataDirectory + "database\\"; } }
        public static string DebugDirectory { get { return baseDirectory + "debug\\"; } }
        public static string CurrentLogText { get { return currentLogText; } set { currentLogText = value; OnNewLogText(); } }

        public static event Notify? NewLogText;
        public static void OnNewLogText() { NewLogText?.Invoke(); }

        public static readonly List<Type> ModelTypeList = [
            typeof(User),
            typeof(Ticket)];

        public static readonly Dictionary<Type, string> SqlTableNames = new()
        {
            { typeof(User), "Users" },
            { typeof(Ticket), "Tickets" }
        };

        public static readonly Dictionary<Type, List<Type>> DictUniqPropsDtoModels = new()
        {
            { typeof(User), [typeof(UserUniqPropsDto0)] },
            { typeof(Ticket), [typeof(TicketUniqPropsDto0)] }
        };

        public static readonly Dictionary<Type, Dictionary<DtoType, Type>> DictDtoModels = new()
        {
            {
                typeof(User), new()
                {
                    { DtoType.Full, typeof(UserFullDto) },
                    { DtoType.Add, typeof(UserAddDto) },
                    { DtoType.Update, typeof(UserUpdateDto) },
                    { DtoType.Filter, typeof(UserFilterDto) },
                    { DtoType.Filters, typeof(UserFilterDtos) }
                }
            },
            {
                typeof(Ticket), new()
                {
                    { DtoType.Full, typeof(TicketFullDto) },
                    { DtoType.Add, typeof(TicketAddDto) },
                    { DtoType.Update, typeof(TicketUpdateDto) },
                    { DtoType.Filter, typeof(TicketFilterDto) },
                    { DtoType.Filters, typeof(TicketFilterDtos) }
                }
            }
            /*
            {
                typeof(), new()
                {
                    { DtoType.Full, typeof(FullDto) },
                    { DtoType.Add, typeof(AddDto) },
                    { DtoType.Update, typeof(UpdateDto) },
                    { DtoType.Filter, typeof(FilterDto) },
                    { DtoType.Filters, typeof(FilterDtos) }
                }
            },
            */
        };
    }
}
