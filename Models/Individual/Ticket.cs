using System.ComponentModel.DataAnnotations.Schema;

using LBV_Basics.Models.Common;

namespace LBV_Basics.Models
{
    public class Ticket : IBaseModel
    {
        public override string ToString() { return DateQuestion.ToString() + " | " + Title; }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DateQuestion { get; set; } = DateTime.UtcNow;
        public DateTime DateAnswer { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(User))] public int UserIdQuestion { get; set; }
        public virtual User UserQuestion { get; set; } = new();
        [ForeignKey(nameof(User))] public int? UserIdAnswer { get; set; }
        public virtual User? UserAnswer { get; set; } = new();
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
