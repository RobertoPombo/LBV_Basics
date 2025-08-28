using System.ComponentModel.DataAnnotations;


namespace LBV_Basics.Models.DTOs
{
    public class UserFullDto : User
    {
        public string FullName
        {
            get { return GetFullName(this); }
            set { }
        }

        public string ShortName
        {
            get { return GetShortName(this); }
            set { }
        }

        public static string GetFullName(User obj)
        {
            return obj.FirstName + " " + obj.LastName;
        }

        public static string GetShortName(User obj)
        {
            string _shortName = string.Empty;
            List<char> cList = [' ', '-'];
            if (obj.FirstName is not null)
            {
                for (int index = 0; index < obj.FirstName.Length - 1; index++)
                {
                    if (_shortName.Length == 0 && !cList.Contains(obj.FirstName[index]))
                    {
                        _shortName = obj.FirstName[index].ToString() + ".";
                    }
                    else if (cList.Contains(obj.FirstName[index]) && !cList.Contains(obj.FirstName[index + 1]))
                    {
                        _shortName += obj.FirstName[index].ToString() + obj.FirstName[index + 1].ToString() + ".";
                    }
                }
                _shortName += " " + obj.LastName;
            }
            else { _shortName = obj.LastName; }
            return _shortName;
        }
    }


    public class UserUniqPropsDto0 : Mapper<User>
    {
        [Required] public string WindowsUserName { get; set; } = string.Empty;
    }


    public class UserAddDto : Mapper<User>
    {
        public string? WindowsUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAdress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsAllowedToAnswer { get; set; }
        public bool? IsAdmin { get; set; }
    }


    public class UserUpdateDto : UserAddDto
    {
        [Required] public int Id { get; set; } = GlobalValues.NoId;
    }


    public class UserFilterDto : UserAddDto
    {
        public int? Id { get; set; }
    }


    public class UserFilterDtos
    {
        public UserFilterDto Filter { get; set; } = new();
        public UserFilterDto FilterMin { get; set; } = new();
        public UserFilterDto FilterMax { get; set; } = new();
    }
}
