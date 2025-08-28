using System.Reflection;

namespace LBV_Basics.Models.Common
{
    public interface IBaseModel
    {
        public static readonly List<List<PropertyInfo>> UniqProps = [[]];
        public int Id { get; set; }
    }
}
