using Domain.DTOs.Student;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IStudentService
{
    Task<Response<List<GetStudentDto>>> GetAllAsync(StudentFilter filter);
    Task<Response<GetStudentDto>> GetByIdAsync(int id);
    Task<Response<GetStudentDto>> CreateAsync(CreateStudentDto request);
    Task<Response<GetStudentDto>> UpdateAsync(int id, UpdateStudentDto request);
    Task<Response<string>> DeleteAsync(int id);
}