using Domain.DTOs.StudentGroups;

namespace Domain.DTOs.Student;

public class GetStudentsWithGroupsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public List<GetStudentGroupDto> StudentGroups { get; set; } = [];
}