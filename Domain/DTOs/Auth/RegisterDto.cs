using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Domain.DTOs;

public class RegisterDto
{
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}