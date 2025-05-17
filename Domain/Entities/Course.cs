using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Entities;

public class Course
{
    public int Id { get; set; }
    [MaxLength(30)]
    public string Title { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    public decimal Price { get; set; }
    
    //navigations
    public List<Group> Groups { get; set; }
}