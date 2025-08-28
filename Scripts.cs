using System.Reflection;

using LBV_Basics.Models;

namespace LBV_Basics
{
    public static class Scripts
    {
        public static void CreateDirectories()
        {
            if (!Directory.Exists(GlobalValues.BaseDirectory)) { Directory.CreateDirectory(GlobalValues.BaseDirectory); }
            if (!Directory.Exists(GlobalValues.ConfigDirectory)) { Directory.CreateDirectory(GlobalValues.ConfigDirectory); }
            if (!Directory.Exists(GlobalValues.DataDirectory)) { Directory.CreateDirectory(GlobalValues.DataDirectory); }
            if (!Directory.Exists(GlobalValues.DatabaseDirectory)) { Directory.CreateDirectory(GlobalValues.DatabaseDirectory); }
            if (!Directory.Exists(GlobalValues.DebugDirectory)) { Directory.CreateDirectory(GlobalValues.DebugDirectory); }
        }

        public static bool IsCompositeKey(string modelName)
        {
            foreach (Type modelType in GlobalValues.ModelTypeList)
            {
                if (modelName.Contains(modelType.Name) && modelName != modelType.Name) { return true; }
            }
            return false;
        }

        public static dynamic Map(dynamic sourceObj, dynamic returnObj, bool acceptNull = false)
        {
            foreach (PropertyInfo sourceProperty in sourceObj.GetType().GetProperties())
            {
                foreach (PropertyInfo returnProperty in returnObj.GetType().GetProperties())
                {
                    if (sourceProperty.Name == returnProperty.Name &&
                        ((acceptNull && Nullable.GetUnderlyingType(returnProperty.PropertyType) is not null) || sourceProperty.GetValue(sourceObj) is not null))
                    {
                        returnProperty.SetValue(returnObj, sourceProperty.GetValue(sourceObj));
                        break;
                    }
                }
            }
            return returnObj;
        }

        public static bool IsSimilar(dynamic obj1, dynamic obj2, bool acceptNull = false)
        {
            foreach (PropertyInfo property1 in obj1.GetType().GetProperties())
            {
                foreach (PropertyInfo property2 in obj2.GetType().GetProperties())
                {
                    var value1 = Scripts.GetCastedValue(obj1, property1);
                    var value2 = Scripts.GetCastedValue(obj2, property2);
                    if (property1.Name == property2.Name && (acceptNull || value1 is not null) && value1 != value2 &&
                        (value1?.ToString().Length > 0 || value2?.ToString().Length > 0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsForeignId(string propertyName)
        {
            if (GetTypeForeignId(propertyName) is null) { return false; }
            else { return true; }
        }

        public static Type? GetTypeForeignId(string propertyName)
        {
            if (propertyName.Length > GlobalValues.Id.Length && propertyName[^GlobalValues.Id.Length..] == GlobalValues.Id)
            {
                foreach (Type type in GlobalValues.ModelTypeList)
                {
                    if (propertyName[..^GlobalValues.Id.Length] == type.Name) { return type; }
                }
                return null;
            }
            return null;
        }

        public static bool ListContainsId(dynamic list, dynamic obj)
        {
            try { foreach (dynamic _obj in list) { if (_obj.Id == obj.Id) { return true; } } }
            catch { return false; }
            return false;
        }

        public static List<Ticket> SortByDateQuestion(List<Ticket> list)
        {
            for (int index1 = 0; index1 < list.Count - 1; index1++)
            {
                for (int index2 = index1; index2 < list.Count; index2++)
                {
                    if (list[index1].DateQuestion > list[index2].DateQuestion)
                    {
                        (list[index2], list[index1]) = (list[index1], list[index2]);
                    }
                }
            }
            return list;
        }

        public static List<Ticket> SortByDateAnswer(List<Ticket> list)
        {
            for (int index1 = 0; index1 < list.Count - 1; index1++)
            {
                for (int index2 = index1; index2 < list.Count; index2++)
                {
                    if (list[index1].DateAnswer > list[index2].DateAnswer)
                    {
                        (list[index2], list[index1]) = (list[index1], list[index2]);
                    }
                }
            }
            return list;
        }

        public static bool CheckDoTimeSpansOverlap(DateTime start1, DateTime start2, DateTime end1, DateTime end2)
        {
            if (start1 >= end2) { return false; }
            if (end1 <= start2) { return false; }
            return true;
        }

        public static bool CheckDoTimeSpansOverlap(DateOnly start1, DateOnly start2, DateOnly end1, DateOnly end2)
        {
            if (start1 >= end2) { return false; }
            if (end1 <= start2) { return false; }
            return true;
        }

        public static string RemoveSpaceStartEnd(string s)
        {
            s ??= string.Empty;
            while (s.Length > 0 && s[..1] == " ") { s = s[1..]; }
            while (s.Length > 0 && s.Substring(s.Length - 1, 1) == " ") { s = s[..^1]; }
            return s;
        }

        public static string StrRemoveSpecialLetters(string str)
        {
            str = str.Replace("ß", "ss");
            str = str.Replace("Ä", "AE"); str = str.Replace("ä", "ae");
            str = str.Replace("Ö", "OE"); str = str.Replace("ö", "oe");
            str = str.Replace("Ü", "UE"); str = str.Replace("ü", "ue");
            str = str.Replace("Á", "A"); str = str.Replace("á", "a");
            str = str.Replace("É", "E"); str = str.Replace("é", "e");
            str = str.Replace("Í", "I"); str = str.Replace("í", "i");
            str = str.Replace("Ó", "O"); str = str.Replace("ó", "o");
            str = str.Replace("Ú", "U"); str = str.Replace("ú", "u");
            str = str.Replace("À", "A"); str = str.Replace("à", "a");
            str = str.Replace("È", "E"); str = str.Replace("è", "e");
            str = str.Replace("Ì", "I"); str = str.Replace("ì", "i");
            str = str.Replace("Ò", "O"); str = str.Replace("ò", "o");
            str = str.Replace("Ù", "U"); str = str.Replace("ù", "u");
            str = str.Replace("Ñ", "N"); str = str.Replace("ñ", "n");
            return str;
        }

        public static string StrRemoveVocals(string str)
        {
            List<string> vocals = ["a", "e", "i", "o", "u"];
            foreach (string vocal in vocals)
            {
                str = str.Replace(vocal.ToLower(), string.Empty);
                str = str.Replace(vocal.ToUpper(), string.Empty);
            }
            return str;
        }

        public static string ValidatedPath(string path0, string? path = null)
        {
            string pathStart;
            string pathName;

            path ??= path0;

            if (path.Length < 3 || path.Substring(1, 2) != ":\\")
            {
                pathStart = "//";
                pathName = path;
            }
            else
            {
                pathStart = path[..3];
                pathName = path[pathStart.Length..];
                if (!Directory.Exists(pathStart))
                {
                    pathStart = "//";
                }
            }

            pathName = PathRemoveBlacklistedChars(pathName);

            if (pathName.Length > 0 && pathName[^1..] != "\\")
            {
                pathName += "\\";
            }

            while (pathName.Length > 0 && pathName[..1] == "\\")
            {
                pathName = pathName[1..];
            }

            while (pathName.Contains("\\\\"))
            {
                pathName = pathName.Remove(pathName.IndexOf("\\\\"), 1);
            }

            path = pathStart + pathName;

            if (path.Length >= path0.Length && path[..path0.Length] == path0)
            {
                path = "//" + path[path0.Length..];
            }

            return path;
        }

        public static string PathRemoveBlacklistedChars(string path)
        {
            List<string> blacklist = ["/", ":", "*", "?", "\"", "<", ">", "|"];
            foreach (string blacklistItem in blacklist)
            {
                while (path.Contains(blacklistItem))
                {
                    path = path.Replace(blacklistItem, string.Empty);
                }
            }
            return path;
        }

        public static string RelativePath2AbsolutePath(string path0, string path)
        {
            if (path.Length > 0 && path[..2] == "//")
            {
                path = path0 + path[2..];
            }
            return path;
        }

        public static bool PathIsParentOf(string path0, string path1, string path2)
        {
            path1 = RelativePath2AbsolutePath(path0, path1);
            path2 = RelativePath2AbsolutePath(path0, path2);
            if (path2.Length >= path1.Length && path1 == path2[..path1.Length]) { return true; }
            else { return false; }
        }

        public static string Date2String(DateTime _date, string parseType)
        {
            int secondint = _date.Second;
            int minuteint = _date.Minute;
            int hourint = _date.Hour;
            int dayint = _date.Day;
            int monthint = _date.Month;
            int yearint = _date.Year;
            string secondstr = secondint.ToString();
            string minutestr = minuteint.ToString();
            string hourstr = hourint.ToString();
            string daystr = dayint.ToString();
            string monthstr = monthint.ToString();
            string yearstr = yearint.ToString();
            if (secondint < 10) { secondstr = "0" + secondstr; }
            if (minuteint < 10) { minutestr = "0" + minutestr; }
            if (hourint < 10) { hourstr = "0" + hourstr; }
            if (dayint < 10) { daystr = "0" + daystr; }
            if (monthint < 10) { monthstr = "0" + monthstr; }
            if (yearint < 10) { yearstr = "0" + yearstr; }
            if (yearint < 100) { yearstr = "0" + yearstr; }
            if (yearint < 1000) { yearstr = "0" + yearstr; }
            int seconddigitcount = 0;
            int minutedigitcount = 0;
            int hourdigitcount = 0;
            int daydigitcount = 0;
            int monthdigitcount = 0;
            int yeardigitcount = 0;
            string text = string.Empty;
            foreach (char currentChar in parseType.Reverse())
            {
                switch (currentChar)
                {
                    case 's': seconddigitcount++; if (seconddigitcount <= secondstr.Length) { text = secondstr.Substring(secondstr.Length - seconddigitcount, 1) + text; } break;
                    case 'm': minutedigitcount++; if (minutedigitcount <= minutestr.Length) { text = minutestr.Substring(minutestr.Length - minutedigitcount, 1) + text; } break;
                    case 'h': hourdigitcount++; if (hourdigitcount <= hourstr.Length) { text = hourstr.Substring(hourstr.Length - hourdigitcount, 1) + text; } break;
                    case 'D': daydigitcount++; if (daydigitcount <= daystr.Length) { text = daystr.Substring(daystr.Length - daydigitcount, 1) + text; } break;
                    case 'M': monthdigitcount++; if (monthdigitcount <= monthstr.Length) { text = monthstr.Substring(monthstr.Length - monthdigitcount, 1) + text; } break;
                    case 'Y': yeardigitcount++; if (yeardigitcount <= yearstr.Length) { text = yearstr.Substring(yearstr.Length - yeardigitcount, 1) + text; } break;
                    default: text = currentChar + text; break;
                }
            }
            return text;
        }

        public static string TimeRemaining2String(long timeRemainingSec)
        {
            if (timeRemainingSec > 2 * 60 * 60 * 24 * 365) { return ((int)Math.Ceiling((double)timeRemainingSec / (60 * 60 * 24 * 365))).ToString() + " Years"; }
            else if (timeRemainingSec > 2 * 60 * 60 * 24) { return ((int)Math.Ceiling((double)timeRemainingSec / (60 * 60 * 24))).ToString() + " Days"; }
            else if (timeRemainingSec > 2 * 60 * 60) { return ((int)Math.Ceiling((double)timeRemainingSec / (60 * 60))).ToString() + " h"; }
            else if (timeRemainingSec > 2 * 60) { return ((int)Math.Ceiling((double)timeRemainingSec / 60)).ToString() + " min"; }
            else { return timeRemainingSec.ToString() + " sec"; }
        }

        public static string TimeRemaining2StringPrecise(long timeRemainingSec, bool showAll = false, string delimiter = null)
        {
            bool useCustomDelimiter = true;
            if (delimiter is null) { useCustomDelimiter = false; }
            int years = (int)Math.Floor((double)timeRemainingSec / (365 * 24 * 60 * 60));
            int days = (int)Math.Floor((double)(timeRemainingSec - (years * 365 * 24 * 60 * 60)) / (24 * 60 * 60));
            int hours = (int)Math.Floor((double)(timeRemainingSec - (years * 365 * 24 * 60 * 60) - (days * 24 * 60 * 60)) / (60 * 60));
            int minutes = (int)Math.Floor((double)(timeRemainingSec - (years * 365 * 24 * 60 * 60) - (days * 24 * 60 * 60) - (hours * 60 * 60)) / 60);
            int seconds = (int)timeRemainingSec - (years * 365 * 24 * 60 * 60) - (days * 24 * 60 * 60) - (hours * 60 * 60) - (minutes * 60);
            string yearsStr = years.ToString();
            if (years < 10) { yearsStr = "0" + yearsStr; }
            string daysStr = days.ToString();
            if (days < 10) { daysStr = "0" + daysStr; }
            string hoursStr = hours.ToString();
            if (hours < 10) { hoursStr = "0" + hoursStr; }
            string minutesStr = minutes.ToString();
            if (minutes < 10) { minutesStr = "0" + minutesStr; }
            string secondsStr = seconds.ToString();
            if (seconds < 10) { secondsStr = "0" + secondsStr; }
            string text = string.Empty;
            if (!useCustomDelimiter) { delimiter = " Years "; }
            if (showAll || years > 0) { text += yearsStr + delimiter; }
            if (!useCustomDelimiter) { delimiter = " Days "; }
            if (showAll || years > 0 || days > 0) { text += daysStr + delimiter; }
            if (!useCustomDelimiter) { delimiter = " h "; }
            if (showAll || years > 0 || days > 0 || hours > 0) { text += hoursStr + delimiter; }
            if (!useCustomDelimiter) { delimiter = " min "; }
            if (showAll || years > 0 || days > 0 || hours > 0 || minutes > 0) { text += minutesStr + delimiter; }
            if (!useCustomDelimiter) { delimiter = " sec"; } else { delimiter = string.Empty; }
            if (showAll || years > 0 || days > 0 || hours > 0 || minutes > 0 || seconds > 0) { text += secondsStr + delimiter; }
            return text;
        }

        public static string Ms2String(int ms, string parseType)
        {
            if (ms == int.MinValue) { ms = int.MaxValue; }
            float flo_input = Math.Abs(ms);
            flo_input /= 1000;
            int hInt = Convert.ToInt32(Math.Floor(flo_input / 3600));
            int minInt = Convert.ToInt32(Math.Floor((flo_input / 60) - 60 * hInt));
            int secInt = Convert.ToInt32(Math.Floor(flo_input - 60 * (minInt + 60 * hInt)));
            int msInt = Convert.ToInt32(Math.Round((flo_input - secInt - 60 * (minInt + 60 * hInt)) * 1000));
            string msStr = msInt.ToString();
            string secStr = secInt.ToString();
            string minStr = minInt.ToString();
            string hStr = hInt.ToString();
            if (msInt < 10) { msStr = "0" + msStr; }
            if (msInt < 100) { msStr = "0" + msStr; }
            if (secInt < 10) { secStr = "0" + secStr; }
            if (minInt < 10) { minStr = "0" + minStr; }
            if (hInt < 10) { hStr = "0" + hStr; }
            int msDigitCount = 0;
            int secDigitCount = 0;
            int minDigitCount = 0;
            int hDigitCount = 0;
            string text = string.Empty;
            foreach (char currentChar in parseType.Reverse())
            {
                switch (currentChar)
                {
                    case 'x': msDigitCount++; if (msDigitCount <= msStr.Length) { text = msStr.Substring(msDigitCount - 1, 1) + text; } break;
                    case 's': secDigitCount++; if (secDigitCount <= secStr.Length) { text = secStr.Substring(secStr.Length - secDigitCount, 1) + text; } break;
                    case 'm': minDigitCount++; if (minDigitCount <= minStr.Length) { text = minStr.Substring(minStr.Length - minDigitCount, 1) + text; } break;
                    case 'h': hDigitCount++; if (hDigitCount <= hStr.Length) { text = hStr.Substring(hStr.Length - hDigitCount, 1) + text; } break;
                    default: text = currentChar + text; break;
                }
            }
            return text;
        }

        public static List<PropertyInfo> GetPropertyList(Type type)
        {
            List<PropertyInfo> list = [];
            foreach (PropertyInfo property in type.GetProperties()) { list.Add(property); }
            return list;
        }

        public static dynamic? GetCastedValue(dynamic obj, PropertyInfo property) { try { return CastValue(property, property.GetValue(obj)); } catch { return null; } }

        public static dynamic? CastValue(PropertyInfo property, dynamic? Value)
        {
            string? strValue = Value?.ToString();
            Type type = property.PropertyType;
            if (type == typeof(string) || GlobalValues.ModelTypeList.Contains(type)) { return strValue; }
            else if (type == typeof(bool)) { if (bool.TryParse(strValue, out bool cv)) { return cv; } else { return null; } }
            else if (type == typeof(bool?)) { if (bool.TryParse(strValue, out bool cv)) { return cv; } else { return null; } }
            else if (type == typeof(byte)) { if (byte.TryParse(strValue, out byte cv)) { return cv; } else { return null; } }
            else if (type == typeof(byte?)) { if (byte.TryParse(strValue, out byte cv)) { return cv; } else { return null; } }
            else if (type == typeof(short)) { if (short.TryParse(strValue, out short cv)) { return cv; } else { return null; } }
            else if (type == typeof(short?)) { if (short.TryParse(strValue, out short cv)) { return cv; } else { return null; } }
            else if (type == typeof(ushort)) { if (ushort.TryParse(strValue, out ushort cv)) { return cv; } else { return null; } }
            else if (type == typeof(ushort?)) { if (ushort.TryParse(strValue, out ushort cv)) { return cv; } else { return null; } }
            else if (type == typeof(int)) { if (int.TryParse(strValue, out int cv)) { return cv; } else { return null; } }
            else if (type == typeof(int?)) { if (int.TryParse(strValue, out int cv)) { return cv; } else { return null; } }
            else if (type == typeof(uint)) { if (uint.TryParse(strValue, out uint cv)) { return cv; } else { return null; } }
            else if (type == typeof(uint?)) { if (uint.TryParse(strValue, out uint cv)) { return cv; } else { return null; } }
            else if (type == typeof(long)) { if (long.TryParse(strValue, out long cv)) { return cv; } else { return null; } }
            else if (type == typeof(long?)) { if (long.TryParse(strValue, out long cv)) { return cv; } else { return null; } }
            else if (type == typeof(ulong)) { if (ulong.TryParse(strValue, out ulong cv)) { return cv; } else { return null; } }
            else if (type == typeof(ulong?)) { if (ulong.TryParse(strValue, out ulong cv)) { return cv; } else { return null; } }
            else if (type == typeof(float)) { if (float.TryParse(strValue, out float cv)) { return cv; } else { return null; } }
            else if (type == typeof(float?)) { if (float.TryParse(strValue, out float cv)) { return cv; } else { return null; } }
            else if (type == typeof(double)) { if (double.TryParse(strValue, out double cv)) { return cv; } else { return null; } }
            else if (type == typeof(double?)) { if (double.TryParse(strValue, out double cv)) { return cv; } else { return null; } }
            else if (type == typeof(decimal)) { if (decimal.TryParse(strValue, out decimal cv)) { return cv; } else { return null; } }
            else if (type == typeof(decimal?)) { if (decimal.TryParse(strValue, out decimal cv)) { return cv; } else { return null; } }
            else if (type == typeof(DateTime)) { if (DateTime.TryParse(strValue, out DateTime cv)) { return cv; } else { return null; } }
            else if (type == typeof(DateTime?)) { if (DateTime.TryParse(strValue, out DateTime cv)) { return cv; } else { return null; } }
            else if (type == typeof(DateOnly)) { if (DateOnly.TryParse(strValue, out DateOnly cv)) { return cv; } else { return null; } }
            else if (type == typeof(DateOnly?)) { if (DateOnly.TryParse(strValue, out DateOnly cv)) { return cv; } else { return null; } }
            else if (type == typeof(TimeSpan)) { if (TimeSpan.TryParse(strValue, out TimeSpan cv)) { return cv; } else { return null; } }
            else if (type == typeof(TimeSpan?)) { if (TimeSpan.TryParse(strValue, out TimeSpan cv)) { return cv; } else { return null; } }
            else if (type == typeof(TimeUnit)) { if (Enum.TryParse(strValue, out TimeUnit cv)) { return cv; } else { return null; } }
            else if (type == typeof(TimeUnit?)) { if (Enum.TryParse(strValue, out TimeUnit cv)) { return cv; } else { return null; } }
            else if (type == typeof(DayOfWeekend)) { if (Enum.TryParse(strValue, out DayOfWeekend cv)) { return cv; } else { return null; } }
            else if (type == typeof(DayOfWeekend?)) { if (Enum.TryParse(strValue, out DayOfWeekend cv)) { return cv; } else { return null; } }
            else if (type == typeof(DtoType)) { if (Enum.TryParse(strValue, out DtoType cv)) { return cv; } else { return null; } }
            else if (type == typeof(DtoType?)) { if (Enum.TryParse(strValue, out DtoType cv)) { return cv; } else { return null; } }
            else if (type == typeof(HttpRequestType)) { if (Enum.TryParse(strValue, out HttpRequestType cv)) { return cv; } else { return null; } }
            else if (type == typeof(HttpRequestType?)) { if (Enum.TryParse(strValue, out HttpRequestType cv)) { return cv; } else { return null; } }
            else if (type == typeof(ProtocolType)) { if (Enum.TryParse(strValue, out ProtocolType cv)) { return cv; } else { return null; } }
            else if (type == typeof(ProtocolType?)) { if (Enum.TryParse(strValue, out ProtocolType cv)) { return cv; } else { return null; } }
            else if (type == typeof(NetworkType)) { if (Enum.TryParse(strValue, out NetworkType cv)) { return cv; } else { return null; } }
            else if (type == typeof(NetworkType?)) { if (Enum.TryParse(strValue, out NetworkType cv)) { return cv; } else { return null; } }
            else if (type == typeof(IpAdressType)) { if (Enum.TryParse(strValue, out IpAdressType cv)) { return cv; } else { return null; } }
            else if (type == typeof(IpAdressType?)) { if (Enum.TryParse(strValue, out IpAdressType cv)) { return cv; } else { return null; } }
            else if (type == typeof(System.Drawing.Color)) { return null; }
            else if (type == typeof(System.Drawing.Color?)) { return null; }
            else { return null; }
        }
    }
}
