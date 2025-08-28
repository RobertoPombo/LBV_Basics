using System.ComponentModel.DataAnnotations;

namespace LBV_Basics.Models.DTOs
{
    public class TicketFullDto : Ticket
    {

    }


    public class TicketUniqPropsDto0 : Mapper<Ticket>
    {
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public DateTime DateQuestion { get; set; } = DateTime.Now;
        [Required] public int UserIdQuestion { get; set; }
    }


    public class TicketAddDto : Mapper<Ticket>
    {
        public string? Title { get; set; }
        public DateTime? DateQuestion { get; set; }
        public DateTime? DateAnswer { get; set; }
        public int? UserIdQuestion { get; set; }
        public int? UserIdAnswer { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }


    public class TicketUpdateDto : TicketAddDto
    {
        [Required] public int Id { get; set; } = GlobalValues.NoId;
    }


    public class TicketFilterDto : TicketAddDto
    {
        public int? Id { get; set; }
        public string? UserQuestion { get; set; }
        public string? UserAnswer { get; set; }
    }


    public class TicketFilterDtos
    {
        public TicketFilterDto Filter { get; set; } = new();
        public TicketFilterDto FilterMin { get; set; } = new();
        public TicketFilterDto FilterMax { get; set; } = new();
    }
}
