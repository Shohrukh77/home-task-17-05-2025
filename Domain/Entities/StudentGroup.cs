using Domain.Enums;

namespace Domain.Entities;

public class StudentGroup
{
    public int StudentId { get; set; } // 1
    public int GroupId { get; set; } // 1
    public StudentGroupStatus StudentGroupStatus { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
    
    //navigations
    public Student Student { get; set; }
    public Group Group { get; set; }
}