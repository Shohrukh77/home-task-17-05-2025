using Domain.Constants;
using Domain.DTOs.Student;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController(IStudentService studentService)
{
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<List<GetStudentDto>>> GetAllAsync([FromQuery]StudentFilter filter)
    {
        var students = await studentService.GetAllAsync(filter);
        return students;
    }
    
    // [Authorize(Roles = $"{Roles.Admin},{Roles.Student}")]
    // [HttpGet("with-student-groups")]
    // public async Task<Response<List<GetStudentsWithStudentGroupsDto>>> GetStudentsWithStudentGroupsAsync()
    // {
    //     var students = await studentService.GetStudentsWithStudentGroupsAsync();
    //     return students;
    // }

    [HttpGet("{id:int}")]
    public async Task<Response<GetStudentDto>> GetAsync(int id)
    {
        var student = await studentService.GetByIdAsync(id);
        return student;
    }

    [HttpPost]
    public async Task<Response<GetStudentDto>> Create(CreateStudentDto request)
    {
        var response = await studentService.CreateAsync(request);
        return response;
    }

    [HttpPut("{id:int}")]
    public async Task<Response<GetStudentDto>> Update(int id, UpdateStudentDto request)
    {
        var response = await studentService.UpdateAsync(id, request);
        return response;
    }

    [HttpDelete("{id:int}")]
    public async Task<Response<string>> Delete(int id)
    {
        var response = await studentService.DeleteAsync(id);
        return response;
    }
}