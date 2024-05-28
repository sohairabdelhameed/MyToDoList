using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyToDoList.Models
{
    public class ToDo
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please Enter a Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "please select a course")]
        public string SubjectId { get; set; } = null!;

        [ValidateNever]
        public Subject subject { get; set; } = null!;

        [Required(ErrorMessage = "Please select a Status")]
        public string StatusId { get; set; } = string.Empty;

        [ValidateNever]
        public Status Status { get; set; } = null!;

        public bool Overdue => StatusId == "open" && DueDate < DateTime.Today;
    }

}
