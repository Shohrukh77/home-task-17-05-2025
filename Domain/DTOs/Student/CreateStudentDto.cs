using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Student;

public class CreateStudentDto
{
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; }
    [MaxLength(30)]
    public string? LastName { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
}