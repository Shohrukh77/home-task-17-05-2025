using Domain.Enums;

namespace Domain.DTOs.StudentGroups;

public class GetStudentGroupDto
{
    public int StudentId { get; set; } // 1
    public int GroupId { get; set; } // 1
    public StudentGroupStatus StudentGroupStatus { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
}